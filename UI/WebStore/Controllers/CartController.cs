using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.ViewModels.Cart;
using WebStore.Domain.ViewModels.Order;
using WebStore.Interfaces.Services;

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
            var create_model = new CreateOrderModel
            {
                OrderViewModel = model,
                OrderItems = cartService.TransFromCart().Items.Select(i => new OrderItemDTO
                {
                    Id = i.Key.Id,
                    Price = i.Key.Price,
                    Quantity = i.Value
                }).ToList()
            };
            var order = orderService.CreateOrder(create_model, User.Identity.Name);
            cartService.RemoveAll();
            return RedirectToAction("OrderConfirmed", new { id=order.Id});
        }
        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
        #region AjaxApi
        public IActionResult AddToCartApi(int id)
        {
            cartService.AddToCart(id);
            return Json(new { id, message = $"Товар {id} добавлен в корзину" } );
        }
        #endregion
    }
}