﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using WebStore.Controllers;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels.Product;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;
namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void ProductDetails_Returns_View_With_Correct_Item()
        {
            const int expected_id = 1;
            string expected_name = $"Item id {expected_id}";
            decimal expected_price = 10m;
            string expected_brand_name = $"Brand of item id {expected_id}";

            var product_data_mock = new Mock<IProductData>();
            product_data_mock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns<int>(id => new ProductDTO
                {
                    Id = id,
                    Name = $"Item id {id}",
                    ImageUrl = $"Image_id_{id}.png",
                    Order = 0,
                    Price = expected_price,
                    Brand = new BrandDTO { Id = 1, Name = $"Brand of item id {id}" }
                });
            var controller = new CatalogController(product_data_mock.Object);

            var result = controller.ProductDetails(expected_id);

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(view_result.ViewData.Model);
            Assert.Equal(expected_id, model.Id);
            Assert.Equal(expected_name, model.Name);
            Assert.Equal(expected_price, model.Price);
            Assert.Equal(expected_brand_name, model.Brand);
        }
        [TestMethod, Description("Описание модульного теста"), Timeout(700), Priority(1)]
        public void ProductDetails_Returns_NotFound()
        {
            var product_data_mock = new Mock<IProductData>();
            product_data_mock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns(default(ProductDTO));

            var controller = new CatalogController(product_data_mock.Object);

            var result = controller.ProductDetails(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void Shop_Returns_Correct_View()
        {
            var product_data_mock = new Mock<IProductData>();
            product_data_mock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns<ProductFilter>(filter => new[]
                {
                    new ProductDTO
                    {
                        Id = 1,
                        Name = "Product 1",
                        Order = 0,
                        ImageUrl = "Product1.png",
                        Price= 10m,
                        Brand = new BrandDTO { Id = 1, Name = "Brand of Product 1" }
                    },
                    new ProductDTO
                    {
                        Id = 2,
                        Name = "Product 2",
                        Order = 0,
                        ImageUrl = "Product2.png",
                        Price= 10m,
                        Brand = new BrandDTO { Id = 2, Name = "Brand of Product 2" }
                    },
                    new ProductDTO
                    {
                        Id = 3,
                        Name = "Product 3",
                        Order = 0,
                        ImageUrl = "Product3.png",
                        Price= 10m,
                        Brand = new BrandDTO { Id = 3, Name = "Brand of Product 3" }
                    }
                });

            var controller = new CatalogController(product_data_mock.Object);

            const int expected_section_id = 1;
            const int expected_brand_id = 5;

            var result = controller.Shop(expected_section_id, expected_brand_id);

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CatalogViewModel>(view_result.ViewData.Model);

            Assert.Equal(3, model.Products.Count());
            Assert.Equal(expected_brand_id, model.BrandId);
            Assert.Equal(expected_section_id, model.SectionId);
            Assert.Equal("Brand of Product 1", model.Products.First().Brand);
        }
    }
}
