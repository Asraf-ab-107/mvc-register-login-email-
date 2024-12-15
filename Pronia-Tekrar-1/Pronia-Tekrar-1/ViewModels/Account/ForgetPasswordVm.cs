using System.ComponentModel.DataAnnotations;

namespace Pronia_Tekrar_1.ViewModels.Account
{
    public class ForgetPasswordVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
