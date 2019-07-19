using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(IConfiguration Configuration) : base(Configuration, "api/products") { }

        public IEnumerable<Section> GetSections()
        {
            return Get<List<Section>>($"{_ServiceAddress}/sections");
        }

        public IEnumerable<Brand> GetBrands()
        {
            return Get<List<Brand>>($"{_ServiceAddress}/brands");
        }

        public PagedProductsDTO GetProducts(ProductFilter Filter)
        {
            var response = Post(_ServiceAddress, Filter);
            return response.Content.ReadAsAsync<PagedProductsDTO>().Result;
        }

        public ProductDTO GetProductById(int id)
        {
            return Get<ProductDTO>($"{_ServiceAddress}/{id}");
        }

        public Section GetSectionById(int id)
        {
            return Get<Section>($"{_ServiceAddress}/sections/{id}");
        }

        public Brand GetBrandById(int id)
        {
            return Get<Brand>($"{_ServiceAddress}/brands/{id}");

        }
    }
}
