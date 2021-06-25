using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    [Authorize(Roles = Role.Administrators)]    //  Ибо нефиг простым смертным смотреть на список сотрудников :)
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _Employees;
        private readonly ILogger<EmployeesController> _Logger;

        public EmployeesController(IEmployeesData employees, ILogger<EmployeesController> logger)
        {
            _Employees = employees;
            _Logger = logger;
        }

        public IActionResult Index() => View(_Employees.GetList());

        public IActionResult Details(int id)
        {
            var employee = _Employees.Get(id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        public IActionResult Create()
        {
            _Logger.LogInformation($"---> Добавление нового сотрудника");

            return View("Edit", new EmployeeViewModel());
        }

        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());

            var employee = _Employees.Get((int)id);
            
            if (employee is null)
                return NotFound();

            _Logger.LogInformation($"---> Редактирование сотрудника {employee.Name} с id: {employee.Id}");

            return View(employee.ToViewModel());
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var employee = model.ToModel();

            if (model.Id == 0)
                _Employees.Add(employee);
            else
                _Employees.Update(employee);

            _Logger.LogInformation($"---> Сохранение изменений: {employee.Name} id: {employee.Id}");

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            var employee = _Employees.Get(id);

            if (employee is null)
                return NotFound();

            _Logger.LogInformation($"---> Запрос на удаление сотрудника {employee.Name} с id: {employee.Id}");

            return View(employee.ToViewModel());
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _Employees.Get(id);

            _Logger.LogInformation($"---> Удаление сотрудника {employee.Name} с id: {employee.Id}");

            _Employees.Delete(id);

            return RedirectToAction("Index");
        }
    }
}