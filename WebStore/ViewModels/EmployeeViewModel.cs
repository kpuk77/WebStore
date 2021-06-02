using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [DisplayName("Имя")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [DisplayName("Фамилия")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите фамилию")]
        public string LastName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [DisplayName("Возраст")]
        [Range(18, 65, ErrorMessage = "Возраст должен быть от 18 до 65 лет")]
        public int Age { get; set; }

        [DisplayName("Номер медицинской страховки")]
        public int MIN { get; set; }

        [DisplayName("Дата трудоустройства")]
        public DateTime EmploymentDate { get; set; } = DateTime.Now;
    }
}
