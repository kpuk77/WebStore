using Microsoft.AspNetCore.Mvc;

using System.Linq;

using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices]IProductData productData)
        {
            var products = productData.GetProducts()
                .Take(9)
                .Select(p => p.ToViewModel());

            return View(products);
        }

        public IActionResult Blog() => View();
        public IActionResult BlogSingle() => View();
        public IActionResult Checkout() => View();
        public IActionResult ContactUs() => View();
    }
}
