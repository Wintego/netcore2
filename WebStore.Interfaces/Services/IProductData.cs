using System.Collections.Generic;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();
        IEnumerable<Brand> GetBrands();
        IEnumerable<ProductDTO> GetProducts(ProductFilter filter);
        ProductDTO GetProductById(int id);
        Section GetSectionById(int id);
        Brand GetBrandById(int id);
    }
}
