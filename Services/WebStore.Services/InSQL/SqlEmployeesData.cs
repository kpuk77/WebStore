using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.InSQL
{
    public class SqlEmployeesData : IEmployeesData
    {
        private readonly ILogger<SqlEmployeesData> _Logger;
        private readonly WebStoreDB _Db;

        public SqlEmployeesData(WebStoreDB db, ILogger<SqlEmployeesData> logger)
        {
            _Logger = logger;
            _Db = db;
        }

        public IEnumerable<Employee> GetList() => _Db.Employees;

        public Employee Get(int id) => _Db.Employees.SingleOrDefault(e => e.Id == id);

        public int Add(Employee employee)
        {
            if (employee is null)
            {
                _Logger.LogError($"---> Ошибка добавления сотрудника");
                throw new ArgumentNullException(nameof(employee));
            }

            _Logger.LogInformation("---> Добавление сотрудника");

            _Db.Add(employee);
            _Db.SaveChanges();

            _Logger.LogInformation($"---> Сотрудник {employee.Id}: {employee.Name} добавлен");
            return employee.Id;
        }

        public void Update(Employee employee)
        {
            if (employee is null)
            {
                _Logger.LogError("---> Ошибка обновления данных сотрудника");
                throw new ArgumentNullException(nameof(employee));
            }

            _Db.Employees.Update(employee);
            _Db.SaveChanges();

            _Logger.LogInformation($"---> Данные сотрудника {employee.Id}: {employee.Name} успешно изменены");
        }

        public bool Delete(int id)
        {
            _Logger.LogInformation($"---> Попытка удаления сотрудника с id: {id}");
            var employee = Get(id);

            if (employee is null)
            {
                _Logger.LogError($"---> Ошибка удаления сотрудника {id}");
                return false;
            }


            _Db.Remove(employee);
            _Db.SaveChanges();

            _Logger.LogInformation($"---> Сотрудник {employee.Id}: {employee.Name} удален");

            return true;
        }
    }
}
