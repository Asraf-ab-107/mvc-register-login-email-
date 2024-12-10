using Pronia_Tekrar_1.Model.BaseEntity;

namespace Pronia_Tekrar_1.Model
{
    public class Tag : BaseClass
    {
        public string Name { get; set; }
        public List<TagProduct> TagProducts { get; set; }
    }
}

