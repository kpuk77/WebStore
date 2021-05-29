using WebStore.Models;
using WebStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private static readonly Random __Rand = new();

        private readonly List<Employee> _Employees = new
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

        private int _CurrentMaxId;

        public InMemoryEmployeesData() => _CurrentMaxId = _Employees.Max(i => i.Id);

        public IEnumerable<Employee> GetList() => _Employees;

        public Employee Get(int id) => _Employees.SingleOrDefault(i => i.Id == id);

        public int Add(Employee employee)
        {
            if (employee is null)
                throw new NullReferenceException(nameof(employee));
            if (_Employees.Contains(employee))
                return employee.Id;

            employee.Id = ++_CurrentMaxId;
            _Employees.Add(employee);

            return employee.Id;
        }

        public void Update(Employee employee)
        {
            if (employee is null) throw new NullReferenceException(nameof(employee));
            if (_Employees.Contains(employee))
                return;

            var sourceItem = Get(employee.Id);
            if (sourceItem is null)
                return;

            sourceItem.Name = employee.Name;
            sourceItem.LastName = employee.LastName;
            sourceItem.MiddleName = employee.MiddleName;
            sourceItem.Age = employee.Age;
            sourceItem.MIN = employee.MIN;
            sourceItem.EmploymentDate = employee.EmploymentDate;
        }

        public bool Delete(int id)
        {
            var employee = Get(id);

            if (employee is null)
                return false;

            return _Employees.Remove(employee);
        }
    }
}
