using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _Configuration { get; }
        private IProductData _ProductData { get; }

        public HomeController(IConfiguration configuration, IProductData productData)
        {
            _Configuration = configuration;
            _ProductData = productData;
        }

        public IActionResult Index()
        {
            var productData = _ProductData.GetProducts()
                .Take(9)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                });

            return View(productData);
        }

        public IActionResult SecondAction() => Content(_Configuration["Greetings"]);
        public IActionResult Blog() => View();
        public IActionResult BlogSingle() => View();
        public IActionResult Cart() => View();
        public IActionResult Checkout() => View();
        public IActionResult ContactUs() => View();
        public IActionResult Login() => View();
        public IActionResult ProductDetails() => View();
    }
}
