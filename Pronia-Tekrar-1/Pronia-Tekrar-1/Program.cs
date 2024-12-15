
using Pronia_Tekrar_1.Helpers.Email;
using Pronia_Tekrar_1.NewFolder.Abstractions.EmailServices;
using Pronia_Tekrar_1.Services;
using System.Configuration;

namespace Pronia_Tekrar_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddTransient<IMailServices,MailService>();
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                //opt.SignIn.RequireConfirmedPhoneNumber = true;
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 5;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


            builder.Services.AddDbContext<AppDbContext>(opt => 
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("Mssql"));
            }                               
            );

            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name:"areas",
                pattern:"{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name:"Default",
                pattern:"{controller=home}/{action=index}/{id?}"
            );

            app.UseStaticFiles();
            app.Run();
        }
    }
}
