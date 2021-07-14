using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(HttpClient client) : base(client, APIAddress.EMPLOYEES) { }

        public IEnumerable<Employee> GetList()
        {
            return Get<IEnumerable<Employee>>(Address);
        }

        public Employee Get(int id)
        {
            return Get<Employee>($"{Address}/{id}");
        }

        public int Add(Employee employee)
        {
            var response = Post(Address, employee);

            if (!response.IsSuccessStatusCode)
                return -1;

            var id = GetList().Select(e => e.Id).Max();

            return id;
        }

        public void Update(Employee employee)
        {
            Put(Address, employee);
        }

        public bool Delete(int id)
        {
            return Delete($"{Address}/{id}").IsSuccessStatusCode;
        }
    }
}