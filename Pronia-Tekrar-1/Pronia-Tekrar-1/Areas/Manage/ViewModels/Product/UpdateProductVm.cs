namespace Pronia_Tekrar_1.Areas.Manage.ViewModels.Product
{
    public class UpdateProductVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public List<int>? TagProducts { get; set; }
    }
}