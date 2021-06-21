using System.Collections.Generic;
using System.Linq;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Mapping
{
    public static class EmployeeMapping
    {
        public static EmployeeViewModel ToViewModel(this Employee employee)
        {
            return employee is null
                ? null
                : new EmployeeViewModel
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    LastName = employee.LastName,
                    MiddleName = employee.MiddleName,
                    Age = employee.Age,
                    EmploymentDate = employee.EmploymentDate,
                    MIN = employee.MIN
                };
        }

        public static IEnumerable<EmployeeViewModel> ToViewModels(this IEnumerable<Employee> employees)
        {
            return employees.Select(e => e.ToViewModel());
        }

        public static Employee ToModel(this EmployeeViewModel model)
        {
            return model is null
                ? null
                : new Employee
                {
                    Id = model.Id,
                    Name = model.Name,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    Age = model.Age,
                    EmploymentDate = model.EmploymentDate,
                    MIN = model.MIN
                };
        }

        public static IEnumerable<Employee> ToModels(this IEnumerable<EmployeeViewModel> models) => models.Select(m => m.ToModel());
    }
}
