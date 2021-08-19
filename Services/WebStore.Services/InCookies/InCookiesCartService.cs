using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using System.Linq;

using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _Context;
        private readonly IProductData _ProductData;
        private readonly string _CartName;

        public InCookiesCartService(IHttpContextAccessor context, IProductData productData)
        {
            _Context = context;
            _ProductData = productData;

            var user = context.HttpContext!.User;
            var userName = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"WebStore.Cart{userName}";
        }

        private Cart Cart
        {
            get
            {
                var context = _Context.HttpContext;
                var cookies = context!.Response.Cookies;

                var cartCookie = context.Request.Cookies[_CartName];
                if (cartCookie is null)
                {
                    var cart = new Cart();

                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart));

                    return cart;
                }

                ReplaceCookies(cookies, cartCookie);

                return JsonConvert.DeserializeObject<Cart>(cartCookie);
            }
            set => ReplaceCookies(_Context.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCookies(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_CartName);
            cookies.Append(_CartName, cookie);
        }

        public void Add(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                cart.Items.Add(new CartItem { ProductId = id });
            else
                item.Quantity++;

            Cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity <= 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void Remove(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                return;

            cart.Items.Remove(item);

            Cart = cart;
        }

        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(i => i.ProductId).ToArray()
            });

            var productsDictionary = products.Products.ToViewModels().ToDictionary(i => i.Id);

            return new CartViewModel
            {
                Items = Cart.Items
                    .Where(i => productsDictionary.ContainsKey(i.ProductId))
                    .Select(i => (productsDictionary[i.ProductId], i.Quantity))
            };
        }
    }
}
