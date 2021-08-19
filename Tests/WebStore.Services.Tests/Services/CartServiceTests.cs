using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System.Collections.Generic;
using System.Linq;

using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

using Assert = Xunit.Assert;

namespace WebStore.Services.Tests.Services
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _Cart;

        private Mock<ICartStore> _CartStoreMock;
        private Mock<IProductData> _ProductDataMock;

        private ICartService _CartService;

        [TestInitialize]
        public void Initialize()
        {
            _Cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new() {ProductId = 1, Quantity = 2},
                    new() {ProductId = 2, Quantity = 1},
                }
            };

            _CartStoreMock = new Mock<ICartStore>();
            _CartStoreMock.Setup(opt => opt.Cart).Returns(_Cart);

            _ProductDataMock = new Mock<IProductData>();
            _ProductDataMock.Setup(opt => opt.GetProducts(It.IsAny<ProductFilter>())).Returns(() => new ProductsPage(new[]
            {
                new Product {Id = 1, Name = "Product 1", ImageUrl = "imageUrl 1", Price = 1, Section = new Section{Id = 1, Name = "Section 1", Order = 1}},
                new Product {Id = 2, Name = "Product 2", ImageUrl = "imageUrl 2", Price = 2, Section = new Section{Id = 2, Name = "Section 2", Order = 2}}
            }, 2));
            _CartService = new CartService(_CartStoreMock.Object, _ProductDataMock.Object);
        }

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

        [TestMethod]    //  Done
        public void CartServiceAddWork()
        {
            const int EXPECTED_ID = 3;
            const int EXPECTED_ITEMS_COUNT = 1;

            _Cart.Items.Clear();

            _CartService.Add(EXPECTED_ID);

            Assert.Equal(EXPECTED_ITEMS_COUNT, _Cart.ItemsCount);
            Assert.Single(_Cart.Items);
            Assert.Collection(_Cart.Items, i => Assert.Equal(EXPECTED_ID, i.ProductId));
        }

        [TestMethod]    //  Done
        public void CartServiceDecrementWorkWithoutItems()
        {
            const int ITEM_ID = 1;

            _Cart.Items.Clear();

            _CartService.Decrement(ITEM_ID);

            Assert.Empty(_Cart.Items);
        }

        [TestMethod]    //  Done
        public void CartServiceDecrementWork()
        {
            const int ITEM_ID = 1;
            const int EXPECTED_ITEMS_COUNT = 2;

            _CartService.Decrement(ITEM_ID);

            var actualCount = _Cart.ItemsCount;

            Assert.Equal(EXPECTED_ITEMS_COUNT, actualCount);
        }

        [TestMethod]    //  Done
        public void CartServiceDecrementRemoveWork()
        {
            const int REMOVED_ITEM_ID = 2;
            const int ITEM_ID = 1;
            const int ITEMS_COUNT = 2;

            _CartService.Decrement(REMOVED_ITEM_ID);

            Assert.Single(_Cart.Items);
            Assert.Collection(_Cart.Items, i => Assert.Equal(ITEM_ID, i.ProductId));
            Assert.Equal(ITEMS_COUNT, _Cart.ItemsCount);
        }

        [TestMethod]    //  Done
        public void CartServiceRemoveNullWork()
        {
            _Cart.Items.Clear();

            _CartService.Remove(It.IsAny<int>());

            Assert.Empty(_Cart.Items);
        }

        [TestMethod]    //  Done
        public void CartServiceRemoveWork()
        {
            const int EXPECTED_REMOVED_ITEMS_ID = 1;
            const int EXPECTED_ITEMS_COUNT = 1;

            _CartService.Remove(EXPECTED_REMOVED_ITEMS_ID);

            Assert.DoesNotContain(EXPECTED_REMOVED_ITEMS_ID, _Cart.Items.Select(i => i.ProductId));
            Assert.Equal(EXPECTED_ITEMS_COUNT, _Cart.ItemsCount);
        }

        [TestMethod]    //  Done
        public void CartServiceClearWork()
        {
            _CartService.Clear();

            Assert.Empty(_Cart.Items);
        }

        [TestMethod]    //  Done
        public void CartServiceGetViewModelWork()
        {
            const int EXPECTED_FIRST_ITEM_ID = 1;
            const int EXPECTED_SECOND_ITEM_ID = 2;
            const int EXPECTED_ITEMS_COUNT = 3;

            var cartViewModel = _CartService.GetViewModel();

            Assert.IsType<CartViewModel>(cartViewModel);
            Assert.Collection(cartViewModel.Items,
                i => Assert.Equal(EXPECTED_FIRST_ITEM_ID, i.product.Id),
                i => Assert.Equal(EXPECTED_SECOND_ITEM_ID, i.product.Id)
            );
            Assert.Equal(EXPECTED_ITEMS_COUNT, cartViewModel.ItemsCount);
        }
    }
}