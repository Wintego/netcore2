using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels.Account;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        public IActionResult Register() => View();
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            using (logger.BeginScope("Регистрация нового пользователя {0}", model.UserName))
            {
                var new_user = new User
                {
                    UserName = model.UserName,
                };
                var creation_result = await userManager.CreateAsync(new_user, model.Password);
                if (creation_result.Succeeded)
                {
                    await signInManager.SignInAsync(new_user, false);
                    return RedirectToAction("Index", "Main");
                }
                foreach (var error in creation_result.Errors)
                    ModelState.AddModelError("", error.Description);
                logger.LogWarning($"Ошибка при регистрации пользователя {model.UserName}: " +
                                  $"{string.Join(",", creation_result.Errors.Select(e=>e.Description))}");
            }
            
            
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
                logger.LogInformation($"Пользователь {model.UserName} вошел в систему");
                if (Url.IsLocalUrl(model.ReturnUrl)) return Redirect(model.ReturnUrl);
                return RedirectToAction("Index", "Main");
            }
            ModelState.AddModelError("", "Имя пользователя или пароль неверны!");
            logger.LogWarning($"Ошибка входа пользователя {model.UserName} в систему");

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            var user_name = User.Identity.Name;
            await signInManager.SignOutAsync();
            logger.LogInformation($"Пользователь {user_name} вышел из системы");

            return RedirectToAction("Index", "Main");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}