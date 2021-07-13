using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System.Threading.Tasks;

using WebStore.Controllers;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;

using Assert = Xunit.Assert;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]    //  Done
        public async Task RegisterModelStateInvalidReturnsWithViewModel()
        {
            const string EXPECTED_NAME = "Test name";

            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager();
            var logger = new Mock<ILogger<AccountController>>();

            var registerUserViewModel = new RegisterUserViewModel
            {
                UserName = EXPECTED_NAME
            };

            var controller = new AccountController(userManager.Object, signInManager.Object, logger.Object);
            controller.ModelState.AddModelError("error", "InvalidModel");

            var result = await controller.Register(registerUserViewModel);
            var viewResult = Assert.IsType<ViewResult>(result);
            var modelResult = Assert.IsAssignableFrom<RegisterUserViewModel>(viewResult.Model);

            Assert.Equal(EXPECTED_NAME, modelResult.UserName);
        }

        [TestMethod]    //  Done
        public async Task RegisterReturnsUnsuccessfulCreatedRedirect()
        {
            const string EXPECTED_NAME = "Test name";
            const string EXPECTED_PASSWORD = "Password";
            const string EXPECTED_ACTION = "Index";
            const string EXPECTED_CONTROLLER = "Home";

            var userManager = CreateMockUserManager();
            userManager.Setup(opt => opt.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));

            var signInManager = CreateMockSignInManager();
            var logger = new Mock<ILogger<AccountController>>();

            var registerViewModel = new RegisterUserViewModel
            {
                UserName = EXPECTED_NAME,
                Password = EXPECTED_PASSWORD
            };

            var controller = new AccountController(userManager.Object, signInManager.Object, logger.Object);

            var result = await controller.Register(registerViewModel);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal(EXPECTED_ACTION, viewResult.ActionName);
            Assert.Equal(EXPECTED_CONTROLLER, viewResult.ControllerName);
        }

        [TestMethod]    //  Done
        public async Task LoginFailureReturnsViewWithModel()
        {
            const string EXPECTED_NAME = "Test name";
            const string EXPECTED_PASSWORD = "Password";

            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager();
            signInManager.Setup(opt => opt.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(SignInResult.Failed));
            var logger = new Mock<ILogger<AccountController>>();

            var loginViewModel = new LoginViewModel
            {
                UserName = EXPECTED_NAME,
                Password = EXPECTED_PASSWORD
            };

            var controller = new AccountController(userManager.Object, signInManager.Object, logger.Object);
            var result = await controller.Login(loginViewModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            var modelResult = Assert.IsAssignableFrom<LoginViewModel>(viewResult.Model);

            Assert.Equal(EXPECTED_NAME, modelResult.UserName);
            Assert.Equal(EXPECTED_PASSWORD, modelResult.Password);
        }

        [TestMethod]    //  Done
        public async Task LoginSucceedReturnRedirect()
        {
            const string EXPECTED_NAME = "Test name";
            const string EXPECTED_PASSWORD = "Password";
            const string EXPECTED_RETURN_URL = "/";
            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager();
            signInManager
                .Setup(opt =>
                    opt.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(SignInResult.Success));

            var logger = new Mock<ILogger<AccountController>>();

            var loginViewModel = new LoginViewModel
            {
                UserName = EXPECTED_NAME,
                Password = EXPECTED_PASSWORD,
            };

            var controller = new AccountController(userManager.Object, signInManager.Object, logger.Object);

            var result = await controller.Login(loginViewModel);
            var viewResult = Assert.IsType<LocalRedirectResult>(result);

            Assert.Equal(EXPECTED_RETURN_URL, viewResult.Url);
        }

        [TestMethod]    //  Done
        public async Task LogoutReturnsRedirect()
        {
            const string EXPECTED_ACTION = "Index";
            const string EXPECTED_CONTROLLER = "Home";
            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager();
            var logger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(userManager.Object, signInManager.Object, logger.Object);

            var result = await controller.Logout();
            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal(EXPECTED_ACTION, viewResult.ActionName);
            Assert.Equal(EXPECTED_CONTROLLER, viewResult.ControllerName);
        }

        [TestMethod]
        public void AccessDeniedReturnsView()
        {
            var userManager = CreateMockUserManager();
            var signInManager = CreateMockSignInManager();
            var logger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(userManager.Object, signInManager.Object, logger.Object);

            var result = controller.AccessDenied();
            Assert.IsType<ViewResult>(result);
        }

        private Mock<UserManager<User>> CreateMockUserManager() =>
            new(Mock.Of<IUserStore<User>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

        private Mock<SignInManager<User>> CreateMockSignInManager() =>
            new (CreateMockUserManager().Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                null,
                null,
                null,
                null);
    }
}