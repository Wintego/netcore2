﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels.Cart;
using WebStore.Domain.ViewModels.Order;
using WebStore.Interfaces.Services;

namespace WebStore.Services
{
    public class SqlOrdersService : IOrderService
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _UserManager;

        public SqlOrdersService(WebStoreContext db, UserManager<User> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public IEnumerable<OrderDTO> GetUserOrders(string UserName) =>
            _db.Orders
                .Include(order => order.User)
                .Include(order => order.OrderItems)
                .Where(order => order.User.UserName == UserName)
               .Select(o => new OrderDTO
               {
                   Address = o.Adress,
                   Phone = o.Phone,
                   Date = o.Date,
                   OrderItem = o.OrderItems.Select(item => new OrderItemDTO
                   {
                       Id = item.Id,
                       Price = item.Price,
                       Quantity = item.Quantity
                   }).ToArray()
               })
                .ToArray();

        public OrderDTO GetOrderById(int id)
        {
            var order = _db.Orders
               .Include(o => o.OrderItems)
               .FirstOrDefault(o => o.Id == id);
            return order is null ? null : new OrderDTO
            {
                Address = order.Adress,
                Phone = order.Phone,
                Date = order.Date,
                OrderItem = order.OrderItems.Select(i => new OrderItemDTO
                {
                    Id = i.Id,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToArray()
            };
        }

        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName)
        {
            var user = _UserManager.FindByNameAsync(UserName).Result;

            using (var transaction = _db.Database.BeginTransaction())
            {
                var order = new Order
                {
                    Name = OrderModel.OrderViewModel.Name,
                    Adress = OrderModel.OrderViewModel.Adress,
                    Phone = OrderModel.OrderViewModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };

                _db.Orders.Add(order);

                foreach (var item in OrderModel.OrderItems)
                {
                    var product_model = _db.Products.FirstOrDefault(p => p.Id == item.Id);
                    if (product_model is null)
                        throw new InvalidOperationException($"Товар с id {item.Id} не найден в базе данных");

                    var product = _db.Products.FirstOrDefault(p => p.Id == product_model.Id);
                    if (product is null)
                        throw new InvalidOperationException($"Товар с идентификатором {product_model.Id} в базе данных не найден");

                    var order_item = new OrderItem
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        Product = product
                    };

                    _db.OrderItems.Add(order_item);
                }

                _db.SaveChanges();
                transaction.Commit();

                return new OrderDTO
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Adress,
                    Phone = order.Phone,
                    Date = order.Date,
                    OrderItem = order.OrderItems.Select(i => new OrderItemDTO
                    {
                        Id = i.Id,
                        Price = i.Price,
                        Quantity = i.Quantity
                    }).ToArray()
                };
            }
        }
    }
}
