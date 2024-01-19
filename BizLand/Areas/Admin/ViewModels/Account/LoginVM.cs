using System.ComponentModel.DataAnnotations;

namespace BizLand.Areas.Admin.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Is required")]
        [MinLength(1, ErrorMessage = "Min Lenght 1")]
        [MaxLength(50, ErrorMessage = "Max Lenght 50")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Is required")]
        [MinLength(1, ErrorMessage = "Min Lenght 1")]
        [MaxLength(150, ErrorMessage = "Max Lenght 150")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
