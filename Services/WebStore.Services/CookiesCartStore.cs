using Microsoft.AspNetCore.Http;
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
        public Cart Cart { get; set; }
    }
}
