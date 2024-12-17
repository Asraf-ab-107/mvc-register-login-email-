using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Pronia_Tekrar_1.Areas.Manage.ViewModels.Product;
using Pronia_Tekrar_1.DAL;
using Pronia_Tekrar_1.Helpers;
using Pronia_Tekrar_1.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Pronia_Tekrar_1.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class ProductController : Controller
    {
        AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext Db,IWebHostEnvironment env)
        {
            _context = Db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var product = await _context.products.
                Include(c=>c.Category).
                Include(p=>p.TagProducts).
                ThenInclude(t=>t.Tag).
                Include(p=>p.productImages).
                ToListAsync();

            return View(product);
        }

        public IActionResult Create()
        {
            ViewBag.Categories =_context.categories.ToList();
            ViewBag.Tags = _context.tags.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVm vm)
        {
            ViewBag.Categories =_context.categories.ToList();
            ViewBag.Tags = _context.tags.ToList();

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
                SKU = vm.SKU,
                productImages = new List<ProductImages>()
            };

            if (vm.TagIds == null)
            {
                foreach (var tagId in vm.TagIds)
                {
                    if (!(await _context.tags.AnyAsync(t => t.Id == tagId)))
                    {
                        ModelState.AddModelError("TagIds", $"{tagId} -li tag yoxdu");
                        return View();
                    }
                    TagProduct tagProduct = new TagProduct()
                    {
                        TagId = tagId,
                        Product = product,
                    };
                    _context.tagsProduct.Add(tagProduct);

                }
            }



            if (!vm.MainPhoto.ContentType.Contains("image"))
            {
                ModelState.AddModelError("MainPhoto", "Duzgun file formati daxil edin");
                return View();
            }
            if (vm.MainPhoto.Length > 20907152)
            {
                ModelState.AddModelError("MainPhoto", "fayl maksimum 2 mb ola biler");
                return View();
            }

            product.productImages.Add(new()
            {
                PrimaryImg = true,
                ImgUrl = vm.MainPhoto.Upload(_env.WebRootPath,"Upload")
            });

            List<string> Errors = new List<string>();

            foreach (var item in vm.Images)
            {
                if (!item.ContentType.Contains("image"))
                {
                    Errors.Add($"{item.Name} file formati duzgun deyil");
                    continue;
                }
                if (item.Length > 20907152)
                {
                    Errors.Add($"{item.Name} fayl maksimum 2 mb ola biler");
                    continue;
                }

                product.productImages.Add(new()
                {
                    PrimaryImg = true,
                    ImgUrl = item.Upload(_env.WebRootPath, "Upload")
                });
            }

            TempData["Errors"] = Errors;

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
                Include(p=>p.productImages).
                FirstOrDefaultAsync(t => t.Id == id);

            ViewBag.Categories = _context.categories.ToList();
            ViewBag.Tags = _context.tags.ToList();

            UpdateProductVm updateProductVm = new UpdateProductVm()
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                SKU = product.SKU,
                Id = product.Id,
                TagIds = new List<int>(),
                ProductImages = new List<ProductImagesVm>()
            };

            

            foreach (var item in product.TagProducts)
            {
                updateProductVm.TagIds.Add(item.TagId);
            }
            foreach (var item in product.productImages)
            {
                updateProductVm.ProductImages.Add(new()
                {
                    ImgUrl = item.ImgUrl,
                    PrimaryImg = item.PrimaryImg
                });
            }

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
            Product oldproduct = _context.products.Include(p=>p.TagProducts).Include(p=>p.productImages).FirstOrDefault(p => p.Id == vm.Id);
            if (oldproduct != null) return View("Error");
            if (vm.CategoryId == null)
            {
                if (!await _context.categories.AnyAsync(c => c.Id == vm.CategoryId))
                {
                    ModelState.AddModelError("CategoryId", $"{vm.CategoryId}-li category id mövcud deyildir");
                    return View();
                }
            }



            if (vm.TagIds.Count > 0)
            {
                foreach (var item in vm.TagIds)
                {
                    await _context.tagsProduct.AddAsync(new TagProduct()
                    {
                        ProductId = oldproduct.Id,
                        TagId = item
                    });
                    
                }
            }

            if (vm.MainPhoto != null)
            {
                if (!vm.MainPhoto.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("MainPhoto", "file formati duzgun deyil");
                    return View(vm);
                }
                if (vm.MainPhoto.Length > 20907152)
                {
                    ModelState.AddModelError("MainPhoto", "fayl maksimum 2 mb ola biler");
                    return View(vm);
                }
                FileExtension.DeleteFile(_env.WebRootPath, "Upload/Product", oldproduct.productImages.FirstOrDefault(p => p.PrimaryImg)?.ImgUrl);
                _context.productImages.Remove(oldproduct.productImages.FirstOrDefault(p => p.PrimaryImg));
                oldproduct.productImages.Add(new()
                {
                    PrimaryImg = true,
                    ImgUrl = vm.MainPhoto.Upload(_env.WebRootPath, "Upload")
                });
            }

            if (vm.ProductImages != null)
            {
                foreach (var item in vm.Images)
                {
                    if (!item.ContentType.Contains("image"))
                    {
                        continue;
                    }
                    if (item.Length > 20907152)
                    {
                        continue;
                    }
                    oldproduct.productImages.Add(new()
                    {
                        PrimaryImg = true,
                        ImgUrl = item.Upload(_env.WebRootPath, "Upload/Product")
                    });
                }
            }

            if (vm.Images != null)
            {
                var removeImg = new List<ProductImages>();
                foreach(var item in oldproduct.productImages.Where(x=>!x.PrimaryImg))
                {
                    if (!vm.ImagesUrl.Any(x => x == item.ImgUrl))
                    {

                        FileExtension.DeleteFile(_env.WebRootPath, "Upload/Product",item.ImgUrl);
                        _context.productImages.Remove(item);
                    }
                }
            }
            else
            {
                foreach(var item in oldproduct.productImages.Where(x=>x.PrimaryImg))
                {
                    FileExtension.DeleteFile(_env.WebRootPath,"Upload/Product",item.ImgUrl);
                    _context.productImages.Remove(item);
                }
            }


            oldproduct.Name = vm.Name;
            oldproduct.Price = vm.Price;
            oldproduct.Description = vm.Description;
            oldproduct.SKU = vm.SKU;
            oldproduct.CategoryId = vm.CategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
