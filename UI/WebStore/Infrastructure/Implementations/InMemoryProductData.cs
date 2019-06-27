using WebStore.Data;
using WebStore.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Infrastructure.Implementations
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;


        public IEnumerable<Section> GetSections() => TestData.Sections;
        public IEnumerable<Product> GetProducts(ProductFilter filter)
        {
            var products = TestData.Products;
            if (filter is null) return products;
            //if (filter.BrandId != null)
            //    products = products.Where(p => p.BrandId == filter.BrandId);
            //if (filter.SectionId != null)
            //    products = products.Where(p => p.SectionId == filter.SectionId);
            return products;  

        }
        public Product GetProductById(int id)
        {
            return TestData.Products.FirstOrDefault(product => product.Id == id);
        }
    }
}
