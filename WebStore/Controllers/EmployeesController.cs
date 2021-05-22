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
            if (id is null || id > __Employees.Max(i => i.Id))
                return Content("Сотрудник не найден");

            var employee = __Employees.Where(i => i.Id == id);

            return View(employee);
        }
    }
}
