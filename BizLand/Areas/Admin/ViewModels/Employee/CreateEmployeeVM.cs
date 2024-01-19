using BizLand.Models;
using System.ComponentModel.DataAnnotations;

namespace BizLand.Areas.Admin.ViewModels
{
    public class CreateEmployeeVM
    {
        [Required(ErrorMessage = "Is required")]
        [MinLength(1, ErrorMessage = "Min Lenght 1")]
        [MaxLength(50, ErrorMessage = "Max Lenght 50")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Is required")]
        [MinLength(1, ErrorMessage = "Min Lenght 1")]
        [MaxLength(150, ErrorMessage = "Max Lenght 150")]
        public string Surname { get; set; }

        public string? TwitLink { get; set; }
        public string? FaceLink { get; set; }
        public string? InstaLink { get; set; }
        public string? LinkedLink { get; set; }

        [Required(ErrorMessage = "Is required")]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Is required")]
        public int PositionId { get; set; }

        public ICollection<Position>? Positions { get; set; }

    }
}
