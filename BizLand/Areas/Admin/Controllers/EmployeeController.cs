using BizLand.Areas.Admin.ViewModels;
using BizLand.DAL;
using BizLand.Models;
using BizLand.Utilities.Extentions;
using BizLand.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BizLand.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page <= 0) return BadRequest();
            double count = await _context.Employees.CountAsync();
            ICollection<Employee> items = await _context.Employees.Skip((page - 1) * 4).Take(4).Include(x => x.Position).ToListAsync();

            PaginationVM<Employee> vM = new PaginationVM<Employee>
            {
                CurrentPage = page,
                TotalPage = Math.Ceiling(count / 4),
                Items = items
            };
            return View(vM);
        }
        public async Task<IActionResult> Create()
        {
            CreateEmployeeVM create = new CreateEmployeeVM
            {
                Positions = await _context.Positions.ToListAsync()
            };
            return View(create);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM create)
        {
            if (!ModelState.IsValid)
            {
                create.Positions = await _context.Positions.ToListAsync();
                return View(create);
            }
            if (await _context.Employees.AnyAsync(x => x.Name.Trim().ToLower() == create.Name.Trim().ToLower()))
            {
                create.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Name", "Is exists");
                return View(create);
            }
            if (!create.Photo.IsValid())
            {
                create.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Photo", "Is not valid");
                return View(create);
            }
            if (!create.Photo.LimitSize())
            {
                create.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Photo", "Limit size 10MB");
                return View(create);
            }
            Employee item = new Employee
            {
                Name = create.Name,
                Surname = create.Surname,
                FaceLink = create.FaceLink,
                TwitLink = create.TwitLink,
                InstaLink = create.InstaLink,
                LinkedLink = create.LinkedLink,
                Img = await create.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img"),
                PositionId = create.PositionId
            };
            await _context.Employees.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Employee item = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            UpdateEmployeeVM update = new UpdateEmployeeVM
            {
                Name = item.Name,
                Surname = item.Surname,
                FaceLink = item.FaceLink,
                TwitLink = item.TwitLink,
                InstaLink = item.InstaLink,
                LinkedLink = item.LinkedLink,
                Img = item.Img,
                PositionId = item.PositionId,
                Positions = await _context.Positions.ToListAsync()
            };
            return View(update);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateEmployeeVM update)
        {
            if (!ModelState.IsValid)
            {
                update.Positions = await _context.Positions.ToListAsync();
                return View(update);
            }
            Employee item = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            if (await _context.Employees.AnyAsync(x => x.Name.Trim().ToLower() == update.Name.Trim().ToLower() && x.Id != id))
            {
                update.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Name", "Is exists");
                return View(update);
            }
            if (update.Photo != null)
            {
                if (!update.Photo.IsValid())
                {
                    update.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Photo", "Is not valid");
                    return View(update);
                }
                if (!update.Photo.LimitSize())
                {
                    update.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Photo", "Limit size 10MB");
                    return View(update);
                }
                item.Img.DeleteFileAsync(_env.WebRootPath, "assets", "img");
                item.Img = await update.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            }
            item.Name = update.Name;
            item.Surname = update.Surname;
            item.FaceLink = update.FaceLink;
            item.TwitLink = update.TwitLink;
            item.LinkedLink = update.LinkedLink;
            item.InstaLink = update.InstaLink;
            item.PositionId = update.PositionId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Employee item = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return NotFound();
            item.Img.DeleteFileAsync(_env.WebRootPath, "assets", "img");
            _context.Employees.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
