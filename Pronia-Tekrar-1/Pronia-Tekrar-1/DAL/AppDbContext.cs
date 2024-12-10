using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pronia_Tekrar_1.Model;

namespace Pronia_Tekrar_1.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<TagProduct> tagsProduct { get; set; }
        public DbSet<Slider> slider { get; set; }
        public DbSet<ProductImages> productImages { get; set; }
    }
}
