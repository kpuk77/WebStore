using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _Configuration { get; }

        public HomeController(IConfiguration configuration) => _Configuration = configuration;

        public IActionResult Index() => View();
        public IActionResult SecondAction() => Content(_Configuration["Greetings"]);
        public IActionResult Blog() => View();
        public IActionResult BlogSingle() => View();
        public IActionResult Cart() => View();
        public IActionResult Checkout() => View();
        public IActionResult ContactUs() => View();
        public IActionResult Login() => View();
        public IActionResult ProductDetails() => View();
        public IActionResult Shop() => View();
    }
}
