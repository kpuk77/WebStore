using WebStore.Models;
using WebStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;


namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private int _CurrentMaxId;

        public InMemoryEmployeesData() => _CurrentMaxId = TestData.Employees.Max(i => i.Id);

        public IEnumerable<Employee> GetList() => TestData.Employees;

        public Employee Get(int id) => TestData.Employees.SingleOrDefault(i => i.Id == id);

        public int Add(Employee employee)
        {
            if (employee is null)
                throw new NullReferenceException(nameof(employee));
            if (TestData.Employees.Contains(employee))
                return employee.Id;

            employee.Id = ++_CurrentMaxId;
            TestData.Employees.Add(employee);

            return employee.Id;
        }

        public void Update(Employee employee)
        {
            if (employee is null) throw new NullReferenceException(nameof(employee));
            if (TestData.Employees.Contains(employee))
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

            return TestData.Employees.Remove(employee);
        }
    }
}