using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Pronia_Tekrar_1.DAL;
using Pronia_Tekrar_1.Helpers;
using Pronia_Tekrar_1.Model;

namespace Pronia_Tekrar_1.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class SliderController : Controller
    {
        AppDbContext _context;
        private readonly IWebHostEnvironment env;

        public SliderController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliderList = await _context.slider.ToListAsync();
            return View(sliderList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!slider.File.ContentType.Contains("image"))
            {
                ModelState.AddModelError("File","Sevh sekil formati daxil etdiniz");
                return View();
            }

            if (slider.File.Length > 20907152)
            {
                ModelState.AddModelError("File", "Maksimum 2 mb sekil qebul olunur");
                return View();
            }



            slider.ImgUrl = slider.File.Upload(env.WebRootPath, "Upload/Slider");
            _context.slider.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Update(int? id)
        {
            if (id == null) return BadRequest();
            var slider = _context.slider.FirstOrDefault(x => x.Id == id);
            if (slider == null) return NotFound();
            return View(slider);

        }

        [HttpPost]
        public IActionResult Update(Slider slider)
        {
            var oldslider = _context.slider.FirstOrDefault(s=>s.Id == slider.Id);
            if (oldslider == null) return NotFound();

            if (slider.File != null)
            {
                if (slider.File.ContentType == "image")
                {
                    ModelState.AddModelError("File", "Yanlis format");
                    return View(oldslider);
                }

                if (slider.File.Length > 20907152)
                {
                    ModelState.AddModelError("File", "Maksimum 2 mb sekil qebul olunur");
                }

                if (!string.IsNullOrEmpty(oldslider.ImgUrl))
                {
                    string oldFilePath = Path.Combine(env.WebRootPath ,oldslider.ImgUrl);
                    if(System.IO.File.Exists(oldFilePath))
                    { 
                        System.IO.File.Delete(oldFilePath);
                    }

                }

                oldslider.ImgUrl = slider.File.Upload(env.WebRootPath,"Upload/Slider");

            }

            oldslider.Title = slider.Title;
            oldslider.SubTitle = slider.SubTitle;
            oldslider.Descriptin = slider.Descriptin;

            if (!ModelState.IsValid)
            {
                return View(oldslider);
            }



            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //public IActionResult Update(Slider NewSlider)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(NewSlider);
        //    }
        //    var oldslider = _context.slider.FirstOrDefault(s => s.Id == NewSlider.Id);
        //    if (oldslider == null) return NotFound();

        //    oldslider.Id = NewSlider.Id;

        //    return RedirectToAction("Index");
        //}


        public IActionResult Delete(int? id)
        {
            var slider = _context.slider.FirstOrDefault(s => s.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            FileExtension.DeleteFile(env.WebRootPath,"Upload/Slider",slider.ImgUrl);

            _context.slider.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
