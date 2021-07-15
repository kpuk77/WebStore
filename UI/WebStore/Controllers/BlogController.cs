using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Single() => View();
    }
}
