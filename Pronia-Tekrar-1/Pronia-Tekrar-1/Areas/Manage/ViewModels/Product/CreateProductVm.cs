using Microsoft.AspNetCore.Mvc;
using Pronia_Tekrar_1.Model;

namespace Pronia_Tekrar_1.Areas.Manage.ViewModels.Product
{
    [Area("Manage")]
    public class CreateProductVm
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public List<int>? TagIds { get; set; }
        public List<int>? TagProducts { get; set; }
        public IFormFile MainPhoto { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
