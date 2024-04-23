using vp_server.Models;
using vp_server.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace vp_server.Utils
{
    public class Recorder
    {
        static VapeshopContext dba = new VapeshopContext();
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
        public List<Manufacturer> GetManufacturers()
        {
            using (VapeshopContext db = new VapeshopContext())            
                return db.Manufacturers.ToList(); 
        }
        //Получить список количества никотина
        public List<NicotineType> GetNicotineType()
        {
            using (VapeshopContext db = new VapeshopContext())
                return db.NicotineTypes.ToList();
        }
        //Получить список крепкости
        public List<Strenght> GetStrenghts()
        {
            using (VapeshopContext db = new VapeshopContext())
                return db.Strenghts.ToList();
        }
        //Передать во ViewBag категории только с двумя параметрами

        public List<CategoriesDTO> GetTrueCategories()
        {
            //Воткнуть тут выборку определенных категорий, которые можно приписать к продукции

            var categories = from c in dba.Categories.Where(c=>c.ParentCategoryId != null)
                             select new CategoriesDTO()
                             {
                                 Id = c.Id,
                                 Title = c.CategoryName
                             };
            return categories.ToList();
        }
        public List<CategoriesDTO> getAllCategories()
        {
            var categories = from c in dba.Categories
                             select new CategoriesDTO()
                             {
                                 Id = c.Id,
                                 Title = c.CategoryName
                             };
            return categories.ToList();
        }

    }
}
