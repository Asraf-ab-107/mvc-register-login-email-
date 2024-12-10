using Pronia_Tekrar_1.Model.BaseEntity;

namespace Pronia_Tekrar_1.Model
{
    public class Product : BaseClass
    {
        public string Name { get; set; }
        public int Price {  get; set; } 
        public string Description { get; set; }
        public string SKU { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<TagProduct>? TagProducts { get; set; }
        public List<ProductImages>? productImages { get; set; }
    }
}
