using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class ErrorsController : Controller
    {
        #region Колхозный велосипед
        
        public IActionResult Index(int id)
        {
            //if (id == 404)
            //    return Error404();

            //else
            //    return Content("Ошибко");
            return View("Error404");
        }

        private IActionResult Error404() => View();

        #endregion
    }
}