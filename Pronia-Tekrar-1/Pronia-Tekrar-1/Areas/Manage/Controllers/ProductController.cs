using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Pronia_Tekrar_1.Areas.Manage.ViewModels.Product;
using Pronia_Tekrar_1.DAL;
using Pronia_Tekrar_1.Model;

namespace Pronia_Tekrar_1.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        AppDbContext _context;

        public ProductController(AppDbContext Db)
        {
            _context = Db;
        }

        public async Task<IActionResult> Index()
        {
            var product = await _context.products.
                Include(c=>c.Category).
                Include(p=>p.TagProducts).
                ThenInclude(t=>t.Tag).
                ToListAsync();

            return View(product);
        }

        public IActionResult Create()
        {
            ViewBag.Categories =_context.categories.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVm vm)
        {
            ViewBag.Categories =_context.categories.ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (vm.CategoryId == null)
            {
                if (! await _context.categories.AnyAsync(c=>c.Id == vm.CategoryId))
                {
                    ModelState.AddModelError("CategoryId", $"{vm.CategoryId}-li category id mövcud deyildir");
                    return View();
                }
            }

            Product product = new Product()
            {
                Name = vm.Name,
                CategoryId = vm.CategoryId,
                Price = vm.Price,
                Description = vm.Description,
                SKU = vm.SKU
            };
            await _context.products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || (!_context.products.Any(p => p.Id == id))) return View("Error");

            var product = await _context.products.
                Include(c => c.Category).
                Include(p => p.TagProducts).
                ThenInclude(t => t.Tag).
                FirstOrDefaultAsync(t => t.Id == id);

            UpdateProductVm updateProductVm = new UpdateProductVm()
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                SKU = product.SKU,
                Id = product.Id
            };
            return View(updateProductVm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVm vm)
        {
            ViewBag.Categories = _context.categories.ToList();
            if (vm.Id == null || (!_context.products.Any(p => p.Id == vm.Id))) return View("Error");

            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Product oldproduct = _context.products.FirstOrDefault(p => p.Id == vm.Id);
            
            if (oldproduct != null) return View("Error");

            if (vm.CategoryId == null)
            {
                if (!await _context.categories.AnyAsync(c => c.Id == vm.CategoryId))
                {
                    ModelState.AddModelError("CategoryId", $"{vm.CategoryId}-li category id mövcud deyildir");
                    return View();
                }
            }

            oldproduct.Name = vm.Name;
            oldproduct.Price = vm.Price;
            oldproduct.Description = vm.Description;
            oldproduct.SKU = vm.SKU;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
