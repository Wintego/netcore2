﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels.Product;
using WebStore.Infrastructure.Map;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        public IViewComponentResult Invoke(string BrandId)
        {
            var brand_id = int.TryParse(BrandId, out var id) ? id : (int?)null;
            return View(new BrandCompleteViewModel
            {
                Brands = GetBrands(),
                CurrentBrandId = brand_id
            });
        }

        private IEnumerable<BrandViewModel> GetBrands() =>
            _ProductData.GetBrands().Select(BrandViewModelMapper.CreateViewModel);
    }
}
 