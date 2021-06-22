using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите название")]
        public string Name { get; set; }

        [DisplayName("Стоимость")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите стоимость")]
        public decimal Price { get; set; }

        [DisplayName("Картинка"), DataType(DataType.ImageUrl)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите путь до картинки")]
        public string ImageUrl { get; set; }

        [DisplayName("Бренд")]
        public string Brand { get; set; }

        [DisplayName("Секция")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите секцию")]
        public string Section { get; set; }
    }
}
