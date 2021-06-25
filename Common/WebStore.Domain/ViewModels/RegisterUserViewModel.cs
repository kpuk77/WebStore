using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [DisplayName("Логин")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
    }
}
