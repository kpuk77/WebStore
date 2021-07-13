using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using System.Collections.Generic;

using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class EmployeesControllerTests
    {
        [TestMethod]    //  Done
        public void IndexReturnsViewWithLists()
        {
            const int EXPECTED_EMPLOYEE_ID = 1;
            const string EXPECTED_EMPLOYEE_NAME = "Test employee name";

            var employee = new Employee
            {
                Id = EXPECTED_EMPLOYEE_ID,
                Name = EXPECTED_EMPLOYEE_NAME
            };
            var employees = new Mock<IEmployeesData>();
            employees.Setup(opt => opt.GetList()).Returns(new List<Employee> { employee });
            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);

            var result = controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Employee>>(viewResult.Model);

            Assert.Contains(employee, model);
        }

        [TestMethod]    //  Done
        public void DetailsReturnsNotFound()
        {
            var employees = new Mock<IEmployeesData>();
            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);
            var result = controller.Details(It.IsAny<int>());
            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]    //  Done
        public void DetailsReturnsViewWithEmployee()
        {
            const int EXPECTED_EMPLOYEE_ID = 1;
            const string EXPECTED_EMPLOYEE_NAME = "Test employee name";

            var employee = new Employee { Id = EXPECTED_EMPLOYEE_ID, Name = EXPECTED_EMPLOYEE_NAME };

            var employees = new Mock<IEmployeesData>();
            employees.Setup(opt => opt.Get(It.IsAny<int>())).Returns(employee);

            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);

            var result = controller.Details(It.IsAny<int>());
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Employee>(viewResult.Model);

            Assert.Equal(employee, model);
        }

        [TestMethod]    //  Done
        public void CreateFailureReturnsViewWithViewModel()
        {
            var employees = new Mock<IEmployeesData>();
            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);
            var result = controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EmployeeViewModel>(viewResult.Model);
        }

        [TestMethod]    //  Done
        public void EditIdFailureReturnsViewWithViewModel()
        {
            var employees = new Mock<IEmployeesData>();
            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);
            var result = controller.Edit(id: null);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<EmployeeViewModel>(viewResult.Model);
        }

        [TestMethod]    //  Done
        public void EditIdReturnsNotFound()
        {
            var employees = new Mock<IEmployeesData>();
            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);
            var result = controller.Edit(It.IsAny<int>());

            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]    //  Done
        public void EditIdReturnsViewWithViewModel()
        {
            const int EXPECTED_ID = 1;
            const string EXPECTED_NAME = "Test employee name";

            var employee = new Employee
            {
                Id = EXPECTED_ID,
                Name = EXPECTED_NAME
            };

            var employees = new Mock<IEmployeesData>();
            employees.Setup(opt => opt.Get(It.IsAny<int>())).Returns(employee);

            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);
            var result = controller.Edit(It.IsAny<int>());

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeViewModel>(viewResult.Model);
            Assert.Equal(EXPECTED_ID, model.Id);
            Assert.Equal(EXPECTED_NAME, model.Name);
        }

        [TestMethod]    //  Done
        public void EditModelStateInvalidReturnsViewWith()
        {
            var employees = new Mock<IEmployeesData>();
            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);
            controller.ModelState.AddModelError("error", "InvalidModel");

            var result = controller.Edit(It.IsAny<EmployeeViewModel>());

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]    //  Done
        public void EditAddEmployeeReturnsRedirect()
        {
            const int EXPECTED_ID = 0;
            const string EXPECTED_NAME = "Test name";
            const string EXPECTED_ACTION = "Index";

            var employees = new Mock<IEmployeesData>();
            var logger = new Mock<ILogger<EmployeesController>>();

            var employeeViewModel = new EmployeeViewModel { Id = EXPECTED_ID, Name = EXPECTED_NAME };

            var controller = new EmployeesController(employees.Object, logger.Object);
            var result = controller.Edit(employeeViewModel);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(EXPECTED_ACTION, viewResult.ActionName);
        }

        [TestMethod]    //  Done
        public void EditUpdateEmployeeReturnsRedirect()
        {
            const int EXPECTED_ID = 1;
            const string EXPECTED_NAME = "Test name";
            const string EXPECTED_ACTION = "Index";

            var employees = new Mock<IEmployeesData>();
            var logger = new Mock<ILogger<EmployeesController>>();

            var employeeViewModel = new EmployeeViewModel { Id = EXPECTED_ID, Name = EXPECTED_NAME };
            
            var controller = new EmployeesController(employees.Object, logger.Object);
            var result = controller.Edit(employeeViewModel);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(EXPECTED_ACTION, viewResult.ActionName);
        }

        [TestMethod]    //  Done
        public void DeleteReturnsBadRequest()
        {
            var employees = new Mock<IEmployeesData>();
            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);
            var result = controller.Delete(It.IsAny<int>());

            Assert.IsType<BadRequestResult>(result);
        }

        [TestMethod]    //  Done
        public void DeleteReturnsNotFound()
        {
            const int ID = 1;
            var employees = new Mock<IEmployeesData>();
            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);
            var result = controller.Delete(ID);

            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]    //  Done
        public void DeleteReturnsViewWithModel()
        {
            const int EXPECTED_ID = 1;
            const string EXPECTED_NAME = "Test name";

            var employees = new Mock<IEmployeesData>();
            employees.Setup(opt => opt.Get(It.IsAny<int>())).Returns(new Employee
            {
                Id = EXPECTED_ID,
                Name = EXPECTED_NAME
            });

            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);
            var result = controller.Delete(EXPECTED_ID);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeViewModel>(viewResult.Model);
            Assert.Equal(EXPECTED_ID, model.Id);
            Assert.Equal(EXPECTED_NAME, model.Name);
        }

        [TestMethod]    //  Done
        public void DeleteConfirmedReturnsRedirect()
        {
            const string EXPECTED_ACTION = "Index";

            var employees = new Mock<IEmployeesData>();
            employees.Setup(opt => opt.Get(It.IsAny<int>()))
                .Returns(new Employee {Id = It.IsAny<int>(), Name = It.IsAny<string>()});
            var logger = new Mock<ILogger<EmployeesController>>();

            var controller = new EmployeesController(employees.Object, logger.Object);
            var result = controller.DeleteConfirmed(It.IsAny<int>());

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(EXPECTED_ACTION, viewResult.ActionName);
        }
    }
}
