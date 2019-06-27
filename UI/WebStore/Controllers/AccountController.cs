using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Register() => View();
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var new_user = new User
            {
                UserName = model.UserName,
                
            };
            var creation_result = await userManager.CreateAsync(new_user, model.Password);
            if(creation_result.Succeeded)
            {
                await signInManager.SignInAsync(new_user, false);
                return RedirectToAction("Index", "Main");
            }
            foreach (var error in creation_result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

        public IActionResult Login() => View();
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var login_result = await signInManager.PasswordSignInAsync(
                model.UserName, model.Password, model.Remember, false);
            if (login_result.Succeeded)
            {
                if (Url.IsLocalUrl(model.ReturnUrl)) return Redirect(model.ReturnUrl);
                return RedirectToAction("Index", "Main");
            }
            ModelState.AddModelError("", "Имя пользователя или пароль неверны!");
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Main");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}