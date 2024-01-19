using BizLand.Models;

namespace BizLand.ViewModels
{
    public class HomeVM
    {
        public ICollection<Service> Services { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
