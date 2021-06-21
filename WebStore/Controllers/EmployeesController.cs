using Microsoft.AspNetCore.Mvc;

using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _Employees;

        public EmployeesController(IEmployeesData employees) =>
            _Employees = employees;

        public IActionResult Index() => View(_Employees.GetList());

        public IActionResult Details(int id)
        {
            var employee = _Employees.Get(id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        public IActionResult Create() => View("Edit", new EmployeeViewModel());

        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());

            var employee = _Employees.Get((int)id);
            
            if (employee is null)
                return NotFound();

            var viewModel = EmployeeToViewModel(employee);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel employeeModel)
        {
            if (!ModelState.IsValid)
                return View(employeeModel);

            var employee = ViewModelToEmployee(employeeModel);

            if (employeeModel.Id == 0)
                _Employees.Add(employee);
            else
                _Employees.Update(employee);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            var employee = _Employees.Get(id);

            if (employee is null)
                return NotFound();

            var employeeModel = EmployeeToViewModel(employee);

            return View(employeeModel);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _Employees.Delete(id);

            return RedirectToAction("Index");
        }

        private Employee ViewModelToEmployee(EmployeeViewModel viewModel)
        {
            var employee = new Employee
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                LastName = viewModel.LastName,
                MiddleName = viewModel.MiddleName,
                Age = viewModel.Age,
                MIN = viewModel.MIN,
                EmploymentDate = viewModel.EmploymentDate
            };

            return employee;
        }

        private EmployeeViewModel EmployeeToViewModel(Employee employee)
        {
            var viewModel = new EmployeeViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Age = employee.Age,
                MIN = employee.MIN,
                EmploymentDate = employee.EmploymentDate
            };

            return viewModel;
        }
    }
}

