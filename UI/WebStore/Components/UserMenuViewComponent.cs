using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Components
{
    [ViewComponent(Name = "UserMenu")]
    public class UserMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated) return View("UserMenuView");
            return View();
        }
    }
}
