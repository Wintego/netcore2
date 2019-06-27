using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IOrderService orderService;

        public CartController(ICartService cartService, IOrderService orderService)
        {
            this.cartService = cartService;
            this.orderService = orderService;
        }

        public IActionResult Details()
        {
            var model = new DetailsViewModel
            {
                CartViewModel = cartService.TransFromCart(),
                OrderViewModel = new OrderViewModel()
            };
            return View(model);
        } 
        public IActionResult DecrementFromCart(int id)
        {
            cartService.DecrementFromCart(id);
            return RedirectToAction("Details");
        }
        public IActionResult RemoveFromCart(int id)
        {
            cartService.DecrementFromCart(id);
            return RedirectToAction("Details");
        }
        public IActionResult RemoveAll()
        {
            cartService.RemoveAll();
            return RedirectToAction("Details");
        }
        public IActionResult AddToCart(int id)
        {
            cartService.AddToCart(id);
            return RedirectToAction("Details");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderViewModel model)
        {
            if (!ModelState.IsValid) return View(nameof(Details), new DetailsViewModel
            {
                CartViewModel = cartService.TransFromCart(),
                OrderViewModel = model
            });
            var order = orderService.CreateOrder(model, cartService.TransFromCart(), User.Identity.Name);
            cartService.RemoveAll();
            return RedirectToAction("OrderConfirmed", new { id=order.Id});
        }
        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}