using Pronia_Tekrar_1.Model.BaseEntity;

namespace Pronia_Tekrar_1.Model
{
    public class ProductImages:BaseClass
    {
        public string ImgUrl { get; set; }
        public bool PrimaryImg { get; set; }
        public int ProductId {  get; set; }
        public Product Product { get; set; }
        public string WebRootPath { get; }

    }
}
