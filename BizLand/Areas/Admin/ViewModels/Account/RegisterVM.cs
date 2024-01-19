using System.ComponentModel.DataAnnotations;

namespace BizLand.Areas.Admin.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Is required")]
        [MinLength(1, ErrorMessage = "Min Lenght 1")]
        [MaxLength(50, ErrorMessage = "Max Lenght 50")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Is required")]
        [MinLength(1, ErrorMessage = "Min Lenght 1")]
        [MaxLength(50, ErrorMessage = "Max Lenght 50")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Is required")]
        [MinLength(1, ErrorMessage = "Min Lenght 1")]
        [MaxLength(50, ErrorMessage = "Max Lenght 50")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Is required")]
        [MinLength(1, ErrorMessage = "Min Lenght 1")]
        [MaxLength(255, ErrorMessage = "Max Lenght 255")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Is required")]
        [MinLength(1, ErrorMessage = "Min Lenght 1")]
        [MaxLength(50, ErrorMessage = "Max Lenght 50")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Is required")]
        [MinLength(1, ErrorMessage = "Min Lenght 1")]
        [MaxLength(50, ErrorMessage = "Max Lenght 50")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Compare with password")]
        public string ConfirmPassword { get; set; }
    }
}
