using Microsoft.AspNetCore.Mvc;
using WebUI.Data;

namespace WebUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        public SliderController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var sliders = _context.Sliders.ToList();
            return View(sliders);
        }
    }
}
