using vp_server.Models;
using Microsoft.EntityFrameworkCore;
namespace vp_server.Utils
{
    public class Recorder
    {
        //Получить список имеющихся продуктов
        public List<Product> getProducts()
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                var products = db.Products.Include(p=>p.Category).Include(p=>p.NicotineType).Include(p=>p.Strength).ToList();
                return products;
            }
        }
        //Получить список имеющихся категорий
        public List<Category> getCategories()
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                var categories = db.Categories.ToList();
                return categories;
            }
        }
        //Получить список производителей
       
    }
}
