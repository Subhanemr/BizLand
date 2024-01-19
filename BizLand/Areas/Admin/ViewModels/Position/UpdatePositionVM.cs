using System.ComponentModel.DataAnnotations;

namespace BizLand.Areas.Admin.ViewModels
{
    public class UpdatePositionVM
    {
        [Required(ErrorMessage = "Is required")]
        [MinLength(1, ErrorMessage = "Min Lenght 1")]
        [MaxLength(50, ErrorMessage = "Max Lenght 50")]
        public string Name { get; set; }
    }
}
