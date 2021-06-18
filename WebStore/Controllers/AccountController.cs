using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;

using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly ILogger<AccountController> _Logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            _Logger = logger;
        }
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User { UserName = model.UserName };

            var result = await _UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _Logger.LogInformation("---> Регистрация нового пользователя {0} с id: {1}", user.UserName, user.Id);
                await _UserManager.AddToRoleAsync(user, Role.Users);
                await _SignInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.TryAddModelError("", error.Description);

            _Logger.LogError(string.Join("\n", result.Errors.Select(e => "---> " + e.Description)));

            return View(model);
        }

        public IActionResult Login(string returnUrl) => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            #region Попытка lockout - не вышло  //TODO: разобраться

            //if (!ModelState.IsValid) return View(model);

            //if (_UserManager.FindByNameAsync(model.UserName).Result is { } user)
            //{
            //    if (_UserManager.IsLockedOutAsync(user).Result)
            //    {
            //        _Logger.LogWarning("---> Неа");
            //        ModelState.AddModelError("", "Заблочен!");
            //        return View(model);
            //    }

            //    var result =
            //        await _SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);

            //    if (result.Succeeded)
            //        return LocalRedirect(model.ReturnUrl ?? "/");
            //}

            //ModelState.AddModelError("", "Ошибка в логине или пароле");

            //_Logger.LogWarning("---> Ошибка входа пользователя {0}", model.UserName);
            //return View(model);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //if (await _UserManager.FindByNameAsync(model.UserName) is { } user)
            //{
            //    await _UserManager.AccessFailedAsync(user);

            //    if (await _UserManager.IsLockedOutAsync(user))
            //    {
            //        _Logger.LogWarning("---> Неа!");
            //        ViewBag.ErrorMessage = "What?";
            //        ModelState.AddModelError("","Блок!");
            //        return View(model);
            //    }
            //}

            #endregion

            if (!ModelState.IsValid) return View(model);

            var result =
                    await _SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);

            if (result.Succeeded)
                return LocalRedirect(model.ReturnUrl ?? "/");

            ModelState.AddModelError("", "Ошибка в логине или пароле");

            _Logger.LogWarning("---> Ошибка входа пользователя {0}", model.UserName);

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}