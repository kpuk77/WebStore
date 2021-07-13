using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class UserProfileControllerTests
    {
        [TestMethod]    //  Done
        public void IndexReturnsView()
        {
            var controller = new UserProfileController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]    //  Done
        public async Task OrdersReturnsViewWithViewModel()
        {
            const int EXPECTED_ID = 1;
            const string EXPECTED_NAME = "Test name";
            const string EXPECTED_USER_NAME = "Test uset";
            const string EXPECTED_PHONE = "Test phone";
            const string EXPECTED_ADDRESS = "Test address";

            var order = new Order
            {
                Id = EXPECTED_ID,
                Name = EXPECTED_NAME,
                Phone = EXPECTED_PHONE,
                Address = EXPECTED_ADDRESS,
                Items = new List<OrderItem> { new() }
            };

            var orderService = new Mock<IOrderService>();
            orderService.Setup(opt => opt.GetOrders(It.IsAny<string>())).ReturnsAsync(new List<Order> { order });

            var controller = new UserProfileController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                            {new Claim(ClaimTypes.Name, EXPECTED_USER_NAME)}))
                    }
                }
            };

            var result = await controller.Orders(orderService.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<UserOrderViewModel>>(viewResult.Model);
            Assert.Collection(model,
                o =>
                {
                    Assert.Equal(EXPECTED_NAME, o.Name);
                    Assert.Equal(EXPECTED_PHONE, o.Phone);
                    Assert.Equal(EXPECTED_ADDRESS, o.Address);
                });
        }
    }
}
