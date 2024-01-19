

using BizLand.DAL;
using BizLand.Models;
using BizLand.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLand.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Employee> employees = await _context.Employees.Include(x => x.Position).ToListAsync();
            ICollection<Service> services = await _context.Services.ToListAsync();

            HomeVM vM = new HomeVM
            {
                Employees = employees,
                Services = services
            };

            return View(vM);
        }
    }
}
