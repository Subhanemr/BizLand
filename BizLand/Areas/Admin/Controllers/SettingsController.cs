using BizLand.Areas.Admin.ViewModels;
using BizLand.DAL;
using BizLand.Models;
using BizLand.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLand.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page <= 0) return BadRequest();
            double count = await _context.Settings.CountAsync();
            ICollection<Settings> items = await _context.Settings.Skip((page - 1) * 4).Take(4).ToListAsync();

            PaginationVM<Settings> vM = new PaginationVM<Settings>
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
        public async Task<IActionResult> Create(CreateSettingsVM create)
        {
            if (!ModelState.IsValid)
            {
                return View(create);
            }
            if (await _context.Settings.AnyAsync(x => x.Key.Trim().ToLower() == create.Key.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(create);
            }
            Settings item = new Settings
            {
                Key = create.Key,
                Value = create.Value
            };
            await _context.Settings.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Settings item = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            UpdateSettingsVM update = new UpdateSettingsVM
            {
                Key = item.Key,
                Value = item.Value
            };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSettingsVM update)
        {
            if (!ModelState.IsValid) return View(update);
            Settings item = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            if (await _context.Settings.AnyAsync(x => x.Key.Trim().ToLower() == update.Key.Trim().ToLower() && x.Id != id))
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(update);
            }
            item.Key = update.Key;
            item.Value = update.Value;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Settings item = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            _context.Settings.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
