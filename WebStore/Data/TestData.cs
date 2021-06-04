using System;
using System.Collections.Generic;
using System.Linq;

using WebStore.Domain.Entities;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        private static readonly Random __Rand = new();

        public static List<Employee> Employees { get; } = new
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

        public static List<Section> Sections { get; } = new 
            (
                Enumerable.Range(1, 10).Select(s => new Section
                {
                    Id = s,
                    Name = $"Секция-{s}",
                    Order = __Rand.Next(1, 30),
                    ParentId = s > 3 ? (__Rand.Next(2) == 0 ? null : __Rand.Next(1, 4)) : null
                })
            );

        public static List<Brand> Brands { get; } = new
            (
                Enumerable.Range(1, 10).Select(b => new Brand
                {
                    Id = b,
                    Name = $"Брэнд-{b}",
                    Order = __Rand.Next(30)
                })
            );
    }
}