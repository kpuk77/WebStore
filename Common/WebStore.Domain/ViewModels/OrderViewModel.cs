using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels
{
    public class OrderViewModel
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required] 
        [DataType(DataType.PhoneNumber), MaxLength(125)]
        public string Phone { get; set; }

        [Required, MaxLength(255)]
        public string Address { get; set; }

    }
}
