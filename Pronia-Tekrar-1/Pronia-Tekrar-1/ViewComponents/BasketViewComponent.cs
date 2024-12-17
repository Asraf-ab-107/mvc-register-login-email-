using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia_Tekrar_1.ViewModels.Basket;

namespace Pronia_Tekrar_1.ViewComponents
{
    public class BasketViewComponent: ViewComponent
    {
        AppDbContext _context;

        public BasketViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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
                    HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(cookies));
                }
            }
            return View(cart);
        }
    }
}
