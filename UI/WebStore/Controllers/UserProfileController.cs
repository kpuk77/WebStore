using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        public IActionResult Index() => View();

        public async Task<IActionResult> Orders([FromServices] IOrderService orderService)
        {
            var orders = await orderService.GetOrders(User.Identity!.Name);

            return View(orders.Select(o => new UserOrderViewModel()
            {
                Id = o.Id,
                Name = o.Name,
                Phone = o.Phone,
                Address = o.Address,
                TotalPrice = o.Items.Sum(i => i.TotalItemsPrice)
            }));
        }
    }
}
