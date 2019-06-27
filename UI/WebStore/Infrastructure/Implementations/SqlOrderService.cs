using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Implementations
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreContext db;
        private readonly UserManager<User> userManager;

        public SqlOrderService(WebStoreContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public Order CreateOrder(OrderViewModel orderViewModel, CartViewModel cartViewModel, string UserName)
        {
            var user = userManager.FindByNameAsync(UserName).Result;
            using (var transaction = db.Database.BeginTransaction())
            {
                var order = new Order
                {
                    Name = orderViewModel.Name,
                    Adress = orderViewModel.Adress,
                    Phone = orderViewModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };
                db.Orders.Add(order);
                foreach (var (product_view_model, quantity) in cartViewModel.Items)
                {
                    var product = db.Products.FirstOrDefault(p => p.Id == product_view_model.Id);
                    if (product is null) throw new InvalidOperationException($"Товар #{product_view_model.Id} не найден");
                    var order_item = new OrderItem
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = quantity,
                        Product = product
                    };
                    db.OrderItems.Add(order_item);
                }
                db.SaveChanges();
                transaction.Commit();
                return order;
            }
        }

        public Order GetOrderById(int id)
        {
            return db.Orders.Include(order => order.OrderItems).FirstOrDefault(order => order.Id == id);
        }

        public IEnumerable<Order> GetUserOrders(string UserName)
        {
            return db.Orders
                .Include(order => order.User)
                .Include(order => order.OrderItems)
                .Where(order => order.User.UserName == UserName)
                .ToArray();
        }
    }
}
