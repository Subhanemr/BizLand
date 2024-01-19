using BizLand.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLand.ViewComponents
{
    public class HeadeerViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HeadeerViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(x => x.Key, y => y.Value);
            return View(settings);
        }
    }
}
