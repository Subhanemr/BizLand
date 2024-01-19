using BizLand.Areas.Admin.ViewModels;
using BizLand.DAL;
using BizLand.Models;
using BizLand.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLand.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page <= 0) return BadRequest();
            double count = await _context.Positions.CountAsync();
            ICollection<Service> items = await _context.Services.Skip((page - 1) * 4).Take(4).ToListAsync();

            PaginationVM<Service> vM = new PaginationVM<Service>
            {
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / 4),
                Items = items
            };
            return View(vM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVM create)
        {
            if (!ModelState.IsValid)
            {
                return View(create);
            }
            if (await _context.Services.AnyAsync(x => x.Title.Trim().ToLower() == create.Title.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(create);
            }
            Service item = new Service
            {
                Title = create.Title,
                SubTitle = create.SubTitle,
                Icon = create.Icon
            };
            await _context.Services.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Service item = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            UpdateServiceVM update = new UpdateServiceVM
            {
                Title = item.Title,
                SubTitle = item.SubTitle,
                Icon = item.Icon
            };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateServiceVM update)
        {
            if (!ModelState.IsValid) return View(update);
            Service item = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            if (await _context.Services.AnyAsync(x => x.Title.Trim().ToLower() == update.Title.Trim().ToLower()&& x.Id != id))
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(update);
            }
            item.Title = update.Title;
            item.SubTitle = update.SubTitle;
            item.Icon = update.Icon;


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Service item = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            _context.Services.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
