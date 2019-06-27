using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IOrderService orderService;

        public ProfileController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Orders()
        {
            var orders = orderService.GetUserOrders(User.Identity.Name);
            return View(orders.Select(o => new UserOrderViewModel
            {
                Id = o.Id,
                Name = o.Name,
                Adress = o.Adress,
                Phone = o.Phone,
                TotalSum = o.OrderItems.Sum(or => or.Quantity * or.Price)
            }));
        }
    }
}