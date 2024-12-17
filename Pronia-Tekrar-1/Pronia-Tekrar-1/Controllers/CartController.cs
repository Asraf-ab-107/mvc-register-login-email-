using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Versioning;
using Pronia_Tekrar_1.ViewModels.Basket;

namespace Pronia_Tekrar_1.Controllers
{
    public class CartController : Controller
    {
        AppDbContext _context;

        public CartController(AppDbContext context)
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
                cookies.ForEach(c =>
                {
                    var product = _context.products.Include(p => p.productImages).FirstOrDefault(p => p.Id == c.Id);
                    if (product == null)
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
                            ImgUrl = product.productImages.FirstOrDefault(p => p.PrimaryImg)?.ImgUrl,
                            Count = c.Count
                        });
                    }
                });
                if (deleteItem.Count > 0)
                {
                    deleteItem.ForEach(d =>
                    {
                        cookies.Remove(d);
                    });
                    Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookies));
                }
            }
            return View(cart);
        }
        [HttpPost]
        public async Task<IActionResult> AddBasket([FromBody] int Id)
        {
            var product = await _context.products.FirstOrDefaultAsync(p => p.Id == Id);
            if (product == null) return NotFound();

            CookieItemVm vm = new CookieItemVm()
            {
                Id = Id,
                Count = 1
            };

            List<CookieItemVm> cookieList;
            var basket = Request.Cookies["basket"];

            if (basket != null)
            {
                cookieList = JsonConvert.DeserializeObject<List<CookieItemVm>>(basket);
                var exsistproduct = cookieList.FirstOrDefault(i => i.Id == Id);

                if (exsistproduct != null)
                {
                    exsistproduct.Count += 1;
                }
                else
                {
                    cookieList.Add(new CookieItemVm()
                    {
                        Id = Id,
                        Count = 1,
                    });
                }
            }
            else
            {
                cookieList = new List<CookieItemVm>();
                cookieList.Add(new CookieItemVm()
                {
                    Id = Id,
                    Count = 1,
                });
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookieList));

            return Ok();
        }
        public IActionResult GetBasket()
        {
            return Content(Request.Cookies["basket"]);
        }

        public IActionResult Refresh()
        {
            return ViewComponent("Basket");
        }
       
        public IActionResult GetBasketCount()
        {

            var jsonCookie = Request.Cookies["basket"];

            List<CookieItemVm> cookie = String.IsNullOrEmpty(jsonCookie) ?
                new List<CookieItemVm>()
                : JsonConvert.DeserializeObject<List<CookieItemVm>>(jsonCookie);
            int count=cookie.Count==0?0
            : cookie.Sum(x => x.Count);

            return Ok(count);
        }
    }
}
