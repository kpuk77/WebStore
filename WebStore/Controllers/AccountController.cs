using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Registration => View();

        public IActionResult Login => View();

        public IActionResult Logout => View();

        public IActionResult AccessDenied => View();
    }
}
