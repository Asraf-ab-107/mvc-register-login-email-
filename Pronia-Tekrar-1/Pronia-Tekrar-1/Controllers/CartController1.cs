using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Versioning;
using Pronia_Tekrar_1.ViewModels.Basket;

namespace Pronia_Tekrar_1.Controllers
{
    public class CartController1 : Controller
    {
        AppDbContext _context;

        public CartController1(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var json = Request.Cookies["basket"];
            List<CookieItemVm> cookies = new List<CookieItemVm>();
            if (json != null)
            {
                cookies = JsonConvert.DeserializeObject<List<CookieItemVm>>(json);
            }
            List<CartVm> cart = new List<CartVm>();
            List<CookieItemVm> deleteItem = new List<CookieItemVm>();
            if (cookies.Count > 0)
            {
                cookies.ForEach( c =>
                {
                    var product = _context.products.Include(p => p.productImages).FirstOrDefault(p => p.Id == c.Id);
                    if (product != null)
                    {
                        deleteItem.Add(c);
                    }
                    else
                    {
                        cart.Add(new CartVm()
                        {
                            Id = c.Id,
                            Name = product.Name,
                            Price = product.Price,
                            ImgUrl = product.productImages.FirstOrDefault(p=>p.PrimaryImg)?.ImgUrl,
                            Count = c.Count
                        });
                    }
                });
                if (deleteItem.Count>0)
                {
                    deleteItem.ForEach(d =>
                    {
                        cookies.Remove(d);
                    });
                }
                Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookies));
            }

            return View();
        }

        public async Task<IActionResult> AddBasket(int itemId)
        {
            var product = await _context.products.FirstOrDefaultAsync(p=>p.Id == itemId);
            if(product == null) return NotFound();

            List<CookieItemVm> cookieList ;

            var basket = Request.Cookies["basket"];

            if (basket != null)
            {
                cookieList = JsonConvert.DeserializeObject<List<CookieItemVm>>(basket);
                var exsistproduct = cookieList.FirstOrDefault(i=>i.Id == itemId);

                if (exsistproduct != null)
                {
                    exsistproduct.Count += 1;
                }
                else
                {
                    cookieList.Add(new CookieItemVm()
                    {
                        Id = itemId,
                        Count = 1,
                    });
                }
            }
            else
            {
                cookieList = new List<CookieItemVm>();
                cookieList.Add(new CookieItemVm()
                {
                    Id = itemId,
                    Count = 1,
                });
            }
            Response.Cookies.Append("basket",JsonConvert.SerializeObject(cookieList));

            CookieItemVm vm = new CookieItemVm()
            {
                Id = itemId,
                Count = 1,
            };

            var json = JsonConvert.DeserializeObject(vm);
            Response.Cookies.Append("basket",json);

            return RedirectToAction("Index","Home");
        }

        public ActionResult GetBasket()
        {
            var json = JsonConvert.DeserializeObject<CookieItemVm>(Request.Cookies["basket"]);
            return View(json);

        }
    }
}
