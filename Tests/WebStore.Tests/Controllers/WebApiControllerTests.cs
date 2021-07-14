using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using WebStore.Controllers;
using WebStore.Interfaces.TestAPI;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebApiControllerTests
    {
        [TestMethod]
        public void IndexReturnsView()
        {
            var valueService = new Mock<IValuesService>();
            var controller = new WebAPIController(valueService.Object);

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}