using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Domain.Entities.User.RoleAdmin)]
    public class HomeController : Controller
    {
        private readonly IProductData productData;

        public HomeController(IProductData productData)
        {
            this.productData = productData;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductList()
        {
            return View(productData.GetProducts(new Domain.Entities.ProductFilter()));
        }
    }
}