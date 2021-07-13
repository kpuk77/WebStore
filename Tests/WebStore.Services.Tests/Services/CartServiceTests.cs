using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

using Assert = Xunit.Assert;

namespace WebStore.Services.Tests.Services
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _Cart;

        [TestInitialize]
        public void Initialize() =>
            _Cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new() {ProductId = 1, Quantity = 2},
                    new() {ProductId = 2, Quantity = 1},
                }
            };

        [TestMethod]    //  Done
        public void CartReturnsCorrectItemsCount()
        {
            var cart = _Cart;

            var expectedItemsCount = _Cart.Items.Sum(i => i.Quantity);
            var actualItemsCount = cart.ItemsCount;

            Assert.Equal(expectedItemsCount, actualItemsCount);
        }

        [TestMethod]    //  Done
        public void CartViewModelReturnsCorrectItemsCount()
        {
            const int FIRST_PRODUCT_ID = 1;
            const string FIRST_PRODUCT_NAME = "Product 1";
            const int FIRST_PRODUCT_COUNT = 1;
            const decimal FIRST_PRODUCT_PRICE = 1.2m;

            const int SECOND_PRODUCT_ID = 1;
            const string SECOND_PRODUCT_NAME = "Product 2";
            const int SECOND_PRODUCT_COUNT = 2;
            const decimal SECOND_PRODUCT_PRICE = 3.4m;

            const int EXPECTED_COUNT = FIRST_PRODUCT_COUNT + SECOND_PRODUCT_COUNT;

            var cartViewModel = new CartViewModel
            {
                Items = new[]
                {
                    (new ProductViewModel {Id = FIRST_PRODUCT_ID, Name = FIRST_PRODUCT_NAME,
                            Price = FIRST_PRODUCT_PRICE}, FIRST_PRODUCT_COUNT),
                    (new ProductViewModel {Id = SECOND_PRODUCT_ID, Name = SECOND_PRODUCT_NAME,
                            Price = SECOND_PRODUCT_PRICE}, SECOND_PRODUCT_COUNT)
                }
            };

            var actualCount = cartViewModel.ItemsCount;

            Assert.Equal(EXPECTED_COUNT, actualCount);
        }

        [TestMethod]    //  Done
        public void CartViewModelReturnsCorrectTotalPrice()
        {
            const int FIRST_PRODUCT_ID = 1;
            const string FIRST_PRODUCT_NAME = "Product 1";
            const int FIRST_PRODUCT_COUNT = 1;
            const decimal FIRST_PRODUCT_PRICE = 1.2m;

            const int SECOND_PRODUCT_ID = 1;
            const string SECOND_PRODUCT_NAME = "Product 2";
            const int SECOND_PRODUCT_COUNT = 2;
            const decimal SECOND_PRODUCT_PRICE = 3.4m;

            const decimal EXPECTED_TOTAL_PRICE =
                FIRST_PRODUCT_COUNT * FIRST_PRODUCT_PRICE + SECOND_PRODUCT_COUNT * SECOND_PRODUCT_PRICE;

            var cartViewModel = new CartViewModel
            {
                Items = new[]
                {
                    (new ProductViewModel {Id = FIRST_PRODUCT_ID, Name = FIRST_PRODUCT_NAME,
                        Price = FIRST_PRODUCT_PRICE}, FIRST_PRODUCT_COUNT),
                    (new ProductViewModel {Id = SECOND_PRODUCT_ID, Name = SECOND_PRODUCT_NAME,
                        Price = SECOND_PRODUCT_PRICE}, SECOND_PRODUCT_COUNT)
                }
            };

            var actualTotalPrice = cartViewModel.Items.Sum(p => p.quantity * p.product.Price);

            Assert.Equal(EXPECTED_TOTAL_PRICE, actualTotalPrice);
        }
    }
}