using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Pronia_Tekrar_1.Controllers
{
    public class ShopController : Controller
    {
        AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {            
            return View();
        }
        public IActionResult Filter(string search)
        {
            return ViewComponent("Shop", search);
        }
    }
}
