using Pronia_Tekrar_1.Model.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia_Tekrar_1.Model
{
    public class Slider:BaseClass
    {
        [Required,StringLength(20,ErrorMessage ="Maksimum uzunluq 20 sivol ola biler")]
        public string Title { get; set; }
        [StringLength(20, ErrorMessage = "Maksimum uzunluq 20 sivol ola biler")]
        public string SubTitle { get; set; }
        public string Descriptin { get; set; }
        public string? ButtonText { get; set; }
        [StringLength(100)]
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
