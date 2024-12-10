using Pronia_Tekrar_1.Model.BaseEntity;

namespace Pronia_Tekrar_1.Model
{
    public class TagProduct : BaseClass
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
