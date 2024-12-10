using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia_Tekrar_1.DAL;
using Pronia_Tekrar_1.Model;
using System.Data.Common;

namespace Pronia_Tekrar_1.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.categories.Include(p => p.Products).ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _context.categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public IActionResult Update(int? id)
        {
            if(id==null) return BadRequest();
            var category = _context.categories.FirstOrDefault(c => c.Id == id);
            if (category == null)return NotFound();
            return View(category);
        }

        [HttpPost]
        public IActionResult Update(Category NewCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(NewCategory);
            }
            var oldCategory = _context.categories.FirstOrDefault(c=>c.Id == NewCategory.Id);
            if (oldCategory == null)return NotFound();

            oldCategory.Name = NewCategory.Name;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = _context.categories.FirstOrDefault(p => p.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _context.categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
