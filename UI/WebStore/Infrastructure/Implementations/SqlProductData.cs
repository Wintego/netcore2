using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Implementations
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreContext _db;
        public SqlProductData(WebStoreContext db)
        {
            _db = db;
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _db.Brands.Include(s => s.Products).AsEnumerable();

        }

        public Product GetProductById(int id)
        {
            return _db.Products
                .Include(product => product.Brand)
                .Include(product => product.Section)
                .FirstOrDefault(product => product.Id == id);
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter)
        {
            IQueryable<Product> products = _db.Products;
            if (filter is null) return products.AsEnumerable();
            if (filter.SectionId != null) products = products.Where(p => p.SectionId == filter.SectionId);
            if (filter.BrandId != null) products = products.Where(p => p.BrandId == filter.BrandId);
            return products.AsEnumerable();
        }

        public IEnumerable<Section> GetSections()
        {
            return _db.Sections.Include(s => s.Products).AsEnumerable();
        }
    }
}
