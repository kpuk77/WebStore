﻿using Microsoft.AspNetCore.Mvc;

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

            var viewModel = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Age = employee.Age,
                MIN = employee.MIN,
                EmploymentDate = employee.EmploymentDate
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel employeeModel)
        {
            var employee = new Employee
            {
                Id = employeeModel.Id,
                Name = employeeModel.Name,
                LastName = employeeModel.LastName,
                MiddleName = employeeModel.MiddleName,
                Age = employeeModel.Age,
                MIN = employeeModel.MIN,
                EmploymentDate = employeeModel.EmploymentDate
            };

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

            var employeeModel = new EmployeeViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Age = employee.Age,
                MIN = employee.MIN,
                EmploymentDate = employee.EmploymentDate
            };

            return View(employeeModel);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _Employees.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
