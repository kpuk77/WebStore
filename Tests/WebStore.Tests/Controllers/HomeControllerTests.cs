using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using WebStore.Controllers;
using WebStore.Domain;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void IndexReturnsView()
        {
            var productData = new Mock<IProductData>();
            productData.Setup(s => s.GetProducts(It.IsAny<ProductFilter>()))
                .Returns(new ProductsPage(Enumerable.Empty<Product>(), It.IsAny<int>()));

            var controller = new HomeController();
            var result = controller.Index(productData.Object);

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void CheckoutReturnsView()
        {
            var controller = new HomeController();
            var result = controller.Checkout();
            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ContactUsReturnsView()
        {
            var controller = new HomeController();
            var result = controller.ContactUs();
            Assert.IsType<ViewResult>(result);
        }
    }
}
