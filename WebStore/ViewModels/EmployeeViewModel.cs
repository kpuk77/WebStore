using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        private const string _REG_EX = @"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)";

        [HiddenInput]
        public int Id { get; set; }

        [DisplayName("Имя")]
        [RegularExpression(_REG_EX, ErrorMessage = "Имя, должно быть написано с использованием кириллицы или латиницы.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [DisplayName("Фамилия")]
        [RegularExpression(_REG_EX, ErrorMessage = "Фамилия, должна быть написана с использованием кириллицы или латиницы.")]
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
