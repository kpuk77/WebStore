using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Index() => View("Error404");
    }
}