using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Controllers;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class ErrorsControllerTests
    {
        [TestMethod]
        public void ErrorsReturnsView()
        {
            var controller = new ErrorsController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}