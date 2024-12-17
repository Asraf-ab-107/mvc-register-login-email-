using Microsoft.AspNetCore.Mvc;

namespace Pronia_Tekrar_1.ViewComponents
{
    public class ShopViewComponent: ViewComponent
    {
        AppDbContext _context;

        public ShopViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? searchTerm)
        {
            List<Product> products;
            if (searchTerm != null)
            {
                products = await _context.products.Include(p=>p.productImages)
                .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToListAsync();
            }
            else
            {
                products = await _context.products.Include(p=>p.productImages).ToListAsync();
            }
            return View(products);
        }

    }
}
