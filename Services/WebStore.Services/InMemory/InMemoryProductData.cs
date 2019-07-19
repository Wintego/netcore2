using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;
using WebStore.Services.Map;

namespace WebStore.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;


        public IEnumerable<Section> GetSections() => TestData.Sections;
        public PagedProductsDTO GetProducts(ProductFilter Filter)
        {
            IEnumerable<Product> products = TestData.Products;
            if (Filter?.BrandId != null)
                products = products.Where(product => product.BrandId == Filter.BrandId);
            if (Filter?.SectionId != null)
                products = products.Where(product => product.SectionId == Filter.SectionId);

            var total_count = products.Count();
            if (Filter?.PageSize != null)
                products = products.Skip((Filter.Page - 1) * (int)Filter.PageSize).Take((int)Filter.PageSize);
            return new PagedProductsDTO
            {
                Products = products.AsEnumerable().ToDTO(),
                TotalCount = total_count
            };
        }
        public ProductDTO GetProductById(int id)
        {
            var p = TestData.Products.FirstOrDefault(product => product.Id == id);
            return p is null ? null : new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Brand = p.Brand is null
                    ? null
                    : new BrandDTO
                    {
                        Id = p.Brand.Id,
                        Name = p.Brand.Name
                    }
            };
        }

        public Section GetSectionById(int id)
        {
            return TestData.Sections.FirstOrDefault(s => s.Id == id);
        }

        public Brand GetBrandById(int id)
        {
            return TestData.Brands.FirstOrDefault(b => b.Id == id);
        }
    }
}
