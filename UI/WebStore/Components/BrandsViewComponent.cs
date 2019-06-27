using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels.Product;
using WebStore.Infrastructure.Map;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BrandsViewComponent: ViewComponent
    {
        private readonly IProductData _ProductData;
        public BrandsViewComponent(IProductData productData)
        {
            _ProductData = productData;
        }
        public IViewComponentResult Invoke()
        {
            var brand = GetBrands();
            return View(brand);
        }

        private IEnumerable<BrandViewModel> GetBrands()
        {
            var brands = _ProductData.GetBrands();
            return brands.Select(brand => brand.CreateViewModel());
        }
    }
}
 