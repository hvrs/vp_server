using Microsoft.AspNetCore.Mvc.Rendering;

namespace vp_server.Models
{
    public class ProductsAttributesCategories
    {
        public IEnumerable<Product> Products { get; set;}
        public IEnumerable<Category> Categories { get; set;}
        public SelectList Manufacturers { get; set;}
    }
}
