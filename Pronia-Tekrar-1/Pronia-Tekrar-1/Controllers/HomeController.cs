using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia_Tekrar_1.DAL;
using Pronia_Tekrar_1.Model;
using Pronia_Tekrar_1.ViewModels;

namespace Pronia_Tekrar_1.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Response.Cookies.Append("Cookie-1", "salam");

            List<Slider> slidersList = _context.slider.ToList();
            List<Category> categoriesList = _context.categories.ToList();

            List<Product> productsList = _context.products.Include(p=>p.productImages).ToList();

            List<Tag> tagsList = _context.tags.ToList();
            List<TagProduct> tagProductsList = _context.tagsProduct.ToList();

            HomeVm vm = new HomeVm()
            {
                Sliders = slidersList,
                Categories = categoriesList,
                Products = productsList,
            };
            return View(vm);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vm = await _context.products.
                Include(c=>c.Category).
                Include(pi=>pi.productImages).
                Include(t=>t.TagProducts).ThenInclude(t=>t.Tag).
                FirstOrDefaultAsync(p=>p.Id == id);

            ViewBag.ReProducts = await _context.products.Include(p=>p.productImages).Where(x=>x.CategoryId==vm.Id&&x.Id!=vm.Id).ToListAsync();
            return View(vm);
        }
    }
}
