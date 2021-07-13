using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.InCookies
{
    public class InCookiesCartStore : ICartStore
    {
        private readonly IHttpContextAccessor _Context;
        private readonly string _CartName;

        public InCookiesCartStore(IHttpContextAccessor context)
        {
            _Context = context;

            var user = context.HttpContext!.User;
            var userName = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"WebStore.Cart{userName}";
        }

        public Cart Cart
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
    }
}
