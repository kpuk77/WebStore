using System;
using System.Collections.Generic;
using System.Linq;

using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.InSQL
{
    public class SqlEmployeesData : IEmployeesData
    {
        private readonly WebStoreDB _Db;

        public SqlEmployeesData(WebStoreDB db) => _Db = db;

        public IEnumerable<Employee> GetList() => _Db.Employees;

        public Employee Get(int id) => _Db.Employees.SingleOrDefault(e => e.Id == id);

        public int Add(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            _Db.Add(employee);
            _Db.SaveChanges();

            return employee.Id;
        }

        public void Update(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            _Db.Employees.Update(employee);
            _Db.SaveChanges();
        }

        public bool Delete(int id)
        {
            var employee = Get(id);

            if (employee is null)
                return false;

            _Db.Remove(employee);
            _Db.SaveChanges();

            return true;
        }
    }
}
