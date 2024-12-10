using System.ComponentModel.DataAnnotations;

namespace Pronia_Tekrar_1.ViewModels.Account
{
    public class RegisterVm
    {
        [Required,MinLength(3)]
        public string Name { get; set; }
        [Required, MinLength(5)]
        public string UserName {  get; set; }
        [Required, MinLength(3)]
        public string Surname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
