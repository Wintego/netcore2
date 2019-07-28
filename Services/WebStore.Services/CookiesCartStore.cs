using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Services
{
    public class CookiesCartStore : ICartStore
    {
        private readonly string CartName;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CookiesCartStore(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;

            var user = httpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? user.Identity.Name : null;
            CartName = $"cart{user_name}";
        }
        public Cart Cart
        {
            get
            {
                var http_context = httpContextAccessor.HttpContext;
                var cookie = http_context.Request.Cookies[CartName];

                Cart cart = null;
                if (cookie is null)
                {
                    cart = new Cart();
                    http_context.Response.Cookies.Append(
                        CartName,
                        JsonConvert.SerializeObject(cart));
                }
                else
                {
                    cart = JsonConvert.DeserializeObject<Cart>(cookie);
                    http_context.Response.Cookies.Delete(CartName);
                    http_context.Response.Cookies.Append(CartName, cookie, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1)
                    });
                }

                return cart;
            }
            set
            {
                var http_context = httpContextAccessor.HttpContext;

                var json = JsonConvert.SerializeObject(value);
                http_context.Response.Cookies.Delete(CartName);
                http_context.Response.Cookies.Append(CartName, json, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1)
                });
            }
        }
    }
}
