using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;

        public CartController(ICartService cartService) => _CartService = cartService;

        public IActionResult Index() => View(new CartOrderViewModel { Cart = _CartService.GetViewModel() });

        public IActionResult Add(int id)
        {
            _CartService.Add(id);

            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Decrement(int id)
        {
            _CartService.Decrement(id);

            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Remove(int id)
        {
            _CartService.Remove(id);

            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Clear()
        {
            _CartService.Clear();

            return RedirectToAction("Index", "Cart");
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(OrderViewModel model, [FromServices] IOrderService orderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), new CartOrderViewModel
                {
                    Cart = _CartService.GetViewModel(),
                    Order = model
                });

            var order = await orderService.CreateOrder(
                User.Identity!.Name, _CartService.GetViewModel(), model);

            _CartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new { order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
