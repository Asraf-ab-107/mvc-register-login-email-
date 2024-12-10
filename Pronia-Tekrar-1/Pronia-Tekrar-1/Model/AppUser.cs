using Microsoft.AspNetCore.Identity;

namespace Pronia_Tekrar_1.Model
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
