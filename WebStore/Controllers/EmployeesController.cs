using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;

using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private static readonly Random __Rand = new Random();

        private static readonly List<Employee> __Employees = new
        (
            Enumerable.Range(1, 100).Select(i => new Employee
            {
                Id = i,
                Age = __Rand.Next(18, 55),
                Name = $"Имя-{i}",
                LastName = $"Фамилия-{i}",
                MiddleName = $"Отчество-{i}",
                MIN = __Rand.Next(10000, 90000),
                EmploymentDate = new DateTime(
                    __Rand.Next(1999, 2020),
                    __Rand.Next(1, 13),
                    __Rand.Next(1, 20),
                    __Rand.Next(10, 19),
                    __Rand.Next(60), 0)
            })
        );

        public IActionResult Index() => View(__Employees);

        public IActionResult Details(int? id)
        {
            var employee = __Employees.FirstOrDefault(i => i.Id == id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var employee = __Employees.FirstOrDefault(i => i.Id == id);

                if (employee != null)
                    return View(employee);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            __Employees[employee.Id] = employee;

            return RedirectToAction("Index");
        }
    }
}
