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

            var categories = from c in dba.Categories
                             select new CategoriesDTO()
                             {
                                 Id = c.Id,
                                 Title = c.CategoryName
                             };
            return categories.ToList();
        }
        //Передать Views только с определенными параметрами для определенного продукта
        public List<ProductViews> GetProductViews(int id)
        {
            var Views = from v in dba.Views.Where(v => v.ProductId == id)
                        select new ProductViews()
                        {
                            Id = v.Id,
                            date = v.Date,
                            time = v.Time
                        };
            return Views.ToList();
        }
        //Передать определенные аттрибуты связных таблиц транзакции для определенного продукта
        public List<ProductTransaction> GetProductTransactions(int id)
        {           
            var transactions = dba.TransactionsAndProducts.Where(t=>t.ProductId==id).Join(dba.Transactions,
                tp => tp.TransactionId,
                t => t.Id,
                (tp, t) => new ProductTransaction()
                {
                    Id = tp.Id,                   
                    Quantitly = tp.Quantitly,
                    date = t.Date
                });
            return transactions.ToList();
            
        }
    }
}
