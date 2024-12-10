using System.ComponentModel.DataAnnotations;

namespace Pronia_Tekrar_1.ViewModels
{
    public class LoginVm
    {
        [Required]
        public string EmailOrUsername { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Remember { get; set; }

    }
}
