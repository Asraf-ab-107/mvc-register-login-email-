using System.ComponentModel.DataAnnotations;

namespace Pronia_Tekrar_1.ViewModels.Account
{
    public class ResetPasswordVm
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password),Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
        public string? userId { get; set; }
        public string? token { get; set; }
    }
}
