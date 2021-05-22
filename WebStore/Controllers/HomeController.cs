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
    }
}
