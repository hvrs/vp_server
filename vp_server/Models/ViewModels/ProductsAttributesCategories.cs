using Microsoft.AspNetCore.Mvc.Rendering;

namespace vp_server.Models.ViewModels
{
    public class ProductsAttributesCategories
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public SelectList Manufacturers { get; set; }
        public int? categroyNow { get; set; }
        public int? manufacturerNow { get; set; }
        public string? productNameNow { get; set; }
        public bool isProduct {  get; set; }
    }
}
