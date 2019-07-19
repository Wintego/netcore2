using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels.Product;
using WebStore.Infrastructure.Map;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IConfiguration configuration;

        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _ProductData = productData;
            this.configuration = configuration;
        }

        public IActionResult Shop(int? SectionId, int? BrandId, int Page = 1)
        {
            var page_size = int.Parse(configuration["PageSize"]);
            var paged_products = _ProductData.GetProducts(new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Page = Page, 
                PageSize = page_size
            });

            var catalog_model = new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = paged_products.Products
                   .Select(p => new ProductViewModel
                   {
                       Id = p.Id,
                       Name = p.Name,
                       Brand = p.Brand?.Name,
                       Order = p.Order,
                       Price = p.Price,
                       ImageUrl = p.ImageUrl
                   }),
                PageModel = new PageViewModel
                {
                    PageNumber = Page,
                    PageSize = page_size,
                    TotalItems = paged_products.TotalCount
                }
                //.Select(ProductViewModelMapper.CreateViewModel)
            };

            return View(catalog_model);
        }
        public IActionResult ProductDetails(int id)
        {
            var product = _ProductData.GetProductById(id);
            if (product is null) return NotFound();
            return View(new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Order = product.Order,
                ImageUrl = product.ImageUrl,
                Brand = product.Brand?.Name
            });
        }
    }
}