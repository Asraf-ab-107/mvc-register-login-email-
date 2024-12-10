using Pronia_Tekrar_1.Model.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace Pronia_Tekrar_1.Model
{
    public class Category: BaseClass
    {
        [Required,
            StringLength(15,ErrorMessage ="maksimum uzunluq 15 simvol ola biler"),
            MinLength(3, ErrorMessage = "minumum uzunluq 3 simvol ola biler")]
        public string Name { get; set; }
        public List<Product>? Products { get; set; }
    
    }
}
