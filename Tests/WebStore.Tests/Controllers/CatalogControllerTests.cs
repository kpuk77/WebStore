using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using WebStore.Controllers;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]    //  Done
        public void IndexReturnsViewWithCatalogViewModel()
        {
            var productData = new Mock<IProductData>();
            var productFilter = new Mock<ProductFilter>();

            var controller = new CatalogController(productData.Object);

            var result = controller.Index(It.IsAny<int>(), It.IsAny<int>());
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<CatalogViewModel>(viewResult.Model);
        }

        [TestMethod]    //  Done
        public void DetailsNullResultReturnsNotFound()
        {
            var productData = new Mock<IProductData>();
            var controller = new CatalogController(productData.Object);

            var result = controller.Details(It.IsAny<int>());
            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]    //  Done
        public void DetailsReturnsView()
        {
            const int EXPECTED_ID = 1;
            const string EXPECTED_NAME = "Test name";
            const string EXPECTED_IMAGE_URL = "Test image url";
            const decimal EXPECTED_PRICE = 9;

            var productData = new Mock<IProductData>();
            var section = new Mock<Section>();
            productData.Setup(opt => opt.GetProduct(It.IsAny<int>())).Returns(new Product
            {
                Id = EXPECTED_ID,
                Name = EXPECTED_NAME,
                ImageUrl = EXPECTED_IMAGE_URL,
                Price = EXPECTED_PRICE,
                Section = section.Object
            });

            var controller = new CatalogController(productData.Object);

            var result = controller.Details(It.IsAny<int>());
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.Model);
            Assert.Equal(EXPECTED_ID, model.Id);
            Assert.Equal(EXPECTED_NAME, model.Name);
            Assert.Equal(EXPECTED_IMAGE_URL, model.ImageUrl);
            Assert.Equal(EXPECTED_PRICE, model.Price);
            Assert.Equal(section.Object.Name, model.Section);
        }
    }
}