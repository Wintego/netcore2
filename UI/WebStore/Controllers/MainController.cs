using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult ContactUs() => View();
        //public IActionResult Checkout() => View();
        public IActionResult BlogSingle() => View();
        public IActionResult Blog() => View();
        public IActionResult NotFound() => View();

        public IActionResult ErrorStatusCode(string id)
        {
            switch (id)
            {
                case "404": return RedirectToAction(nameof(NotFound));
                default: return Content($"Ошибка {id}");
            }
                     
        }
    }
}