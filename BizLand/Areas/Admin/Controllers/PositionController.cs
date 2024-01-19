using BizLand.Areas.Admin.ViewModels;
using BizLand.DAL;
using BizLand.Models;
using BizLand.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLand.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page <= 0) return BadRequest();
            double count = await _context.Positions.CountAsync();
            ICollection<Position> items = await _context.Positions.Skip((page - 1) * 4).Take(4).Include(x => x.Employees).ToListAsync();

            PaginationVM<Position> vM = new PaginationVM<Position>
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
        public async Task<IActionResult> Create(CreatePositionVM create)
        {
            if (!ModelState.IsValid)
            {
                return View(create);
            }
            if (await _context.Positions.AnyAsync(x => x.Name.Trim().ToLower() == create.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(create);
            }
            Position item = new Position
            {
                Name = create.Name
            };
            await _context.Positions.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Position item = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            UpdatePositionVM update = new UpdatePositionVM
            {
                Name = item.Name
            };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdatePositionVM update)
        {
            if (!ModelState.IsValid)
            {
                return View(update);
            }
            Position item = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            if (await _context.Positions.AnyAsync(x => x.Name.Trim().ToLower() == update.Name.Trim().ToLower()))
            {
                ModelState.AddModelError("Name", "Is exists");
                return View(update);
            }
            item.Name = update.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Position item = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            _context.Positions.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
