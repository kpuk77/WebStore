using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using WebStore.Models;
using WebStore.Services;
using WebStore.Services.Interfaces;

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

        public IActionResult Edit(int id)
        {
            var employee = _Employees.Get(id);

            if (employee is not null)
                return View(employee);
            
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            _Employees.Update(employee);

            return RedirectToAction("Index");
        }
    }
}
