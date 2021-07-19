using Microsoft.AspNetCore.Mvc;

using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _CartService;

        public CartViewComponent(ICartService cartService) => _CartService = cartService;

        public IViewComponentResult Invoke()
        {
            var itemsCount = _CartService.GetViewModel().ItemsCount;

            ViewBag.Count = itemsCount > 0 ? $"({itemsCount})" : null;

            return View();
        }
    }
}
