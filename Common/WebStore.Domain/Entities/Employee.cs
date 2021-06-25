using System;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    public class Employee : NamedEntity
    {
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public int Age { get; set; }

        public int MIN { get; set; }    //  Medical Insurance Number

        public DateTime EmploymentDate { get; set; }
    }
}