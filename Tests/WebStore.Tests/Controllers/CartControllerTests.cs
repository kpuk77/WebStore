using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void IndexReturnsView()
        {
            var cartService = new Mock<ICartService>();

            var controller = new CartController(cartService.Object);

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void AddReturnsRedirect()
        {
            var cartService = new Mock<ICartService>();
            var controller = new CartController(cartService.Object);

            var result = controller.Add(It.IsAny<int>());
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal(nameof(controller.Index), redirectResult.ActionName);
        }

        [TestMethod]
        public void DecrementReturnsRedirect()
        {
            var cartService = new Mock<ICartService>();
            var controller = new CartController(cartService.Object);

            var result = controller.Decrement(It.IsAny<int>());
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal(nameof(controller.Index), redirectResult.ActionName);
        }

        [TestMethod]
        public void RemoveReturnsRedirect()
        {
            var cartService = new Mock<ICartService>();
            var controller = new CartController(cartService.Object);

            var result = controller.Decrement(It.IsAny<int>());
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal(nameof(controller.Index), redirectResult.ActionName);
        }

        [TestMethod]
        public void ClearReturnsRedirect()
        {
            var cartService = new Mock<ICartService>();
            var controller = new CartController(cartService.Object);

            var result = controller.Clear();
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal(nameof(controller.Index), redirectResult.ActionName);
        }

        [TestMethod]
        public async Task CheckoutModelStateInvalidReturnsViewWithModel()
        {
            const string EXPECTED_NAME = "Test order";

            var cartServiceMock = new Mock<ICartService>();
            var orderService = new Mock<IOrderService>();
            var controller = new CartController(cartServiceMock.Object);

            controller.ModelState.AddModelError("error", "InvalidModel");

            var orderViewModel = new OrderViewModel
            {
                Name = EXPECTED_NAME
            };

            var result = await controller.CheckOut(orderViewModel, orderService.Object);

            var viewResult = Assert.IsType<ViewResult>(result);

            var modelResult = Assert.IsAssignableFrom<CartOrderViewModel>(viewResult.Model);

            Assert.Equal(EXPECTED_NAME, modelResult.Order.Name);
        }

        [TestMethod]
        public async Task CheckoutModelStateCallServiceAndReturnsRedirect()
        {
            const int EXPECTED_ORDER_ID = 1;
            const string EXPECTED_ORDER_NAME = "Test name";
            const string EXPECTED_ORDER_ADDRESS = "Test address";
            const string EXPECTED_ORDER_PHONE = "+12345678910";
            const string EXPECTED_USER = "Test user";

            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock.Setup(s => s.GetViewModel()).Returns(new CartViewModel
            {
                Items = new[] { (new ProductViewModel { Name = "Test product" }, 1) }
            });

            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock.Setup(s =>
                    s.CreateOrder(It.IsAny<string>(), It.IsAny<CartViewModel>(), It.IsAny<OrderViewModel>()))
                .ReturnsAsync(new Order
                {
                    Id = EXPECTED_ORDER_ID,
                    Name = EXPECTED_ORDER_NAME,
                    Address = EXPECTED_ORDER_ADDRESS,
                    Phone = EXPECTED_ORDER_PHONE,
                    TimeOrder = DateTime.Now,
                    Items = Array.Empty<OrderItem>()
                });

            var controller = new CartController(cartServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(
                            new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, EXPECTED_USER) }))
                    }
                }
            };

            var orderViewModel = new OrderViewModel
            {
                Name = EXPECTED_ORDER_NAME,
                Address = EXPECTED_ORDER_ADDRESS,
                Phone = EXPECTED_ORDER_PHONE
            };

            var result = await controller.CheckOut(orderViewModel, orderServiceMock.Object);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(CartController.OrderConfirmed), redirectResult.ActionName);
            Assert.Null(redirectResult.ControllerName);

            Assert.Equal(EXPECTED_ORDER_ID, redirectResult.RouteValues["id"]);
        }
    }
}
