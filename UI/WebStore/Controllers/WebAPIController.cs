using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Controllers
{
    [Authorize]
    public class WebAPIController : Controller
    {
        private readonly IValuesService _ValuesService;

        public WebAPIController(IValuesService valuesService) => _ValuesService = valuesService;

        public IActionResult Index() => View(_ValuesService.GetAll());
    }
}
