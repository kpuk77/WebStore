using System;

namespace WebStore.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public int Age { get; set; }

        public int MIN { get; set; }    //  Medical Insurance Number

        public DateTime EmploymentDate { get; set; }
    }
}