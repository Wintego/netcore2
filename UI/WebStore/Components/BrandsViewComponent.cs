using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Map;

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
 