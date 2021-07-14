using System.Linq;

using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services
{
    public class CartService : ICartService
    {
        private readonly ICartStore _CartStore;
        private readonly IProductData _ProductData;
        
        public CartService(ICartStore cartStore, IProductData productData)
        {
            _CartStore = cartStore;
            _ProductData = productData;
        }

        public void Add(int id)
        {
            var cart = _CartStore.Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                cart.Items.Add(new CartItem { ProductId = id });
            else
                item.Quantity++;

            _CartStore.Cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = _CartStore.Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity <= 0)
                cart.Items.Remove(item);

            _CartStore.Cart = cart;
        }

        public void Remove(int id)
        {
            var cart = _CartStore.Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                return;

            cart.Items.Remove(item);

            _CartStore.Cart = cart;
        }

        public void Clear()
        {
            var cart = _CartStore.Cart;

            cart.Items.Clear();

            _CartStore.Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                Ids = _CartStore.Cart.Items.Select(i => i.ProductId).ToArray()
            });

            var productsDictionary = products.ToViewModels().ToDictionary(i => i.Id);

            return new CartViewModel
            {
                Items = _CartStore.Cart.Items
                    .Where(i => productsDictionary.ContainsKey(i.ProductId))
                    .Select(i => (productsDictionary[i.ProductId], i.Quantity))
            };
        }
    }
}
