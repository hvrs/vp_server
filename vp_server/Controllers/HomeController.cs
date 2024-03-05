using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vp_server.Models;
using vp_server.Utils;

namespace vp_server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string? name, int? category, int? manufacturer)
        {
            using (VapeshopContext db = new VapeshopContext())
            {
               IQueryable<Product> products = db.Products.Include(p=>p.Manufacturer).Include(p=>p.Category).Include(p=>p.NicotineType).Include(p=>p.Strength);
               List<Product> productList = new List<Product>();
               if (name != null)
               {
                    if (category != null)
                    {
                        var ctg = db.Categories.Where(c => c.ParentCategoryId == category).ToList();
                        productList = products.Where(p => p.CategoryId == category).Where(p => p.Title.Contains(name)).ToList();
                        //products = products.Where(p => p.CategoryId == category).Where(p => p.Title.Contains(name));
                        if (ctg.Count != 0)
                        {
                            foreach (var ct in ctg)
                            {
                                productList.Add(db.Products.Include(p => p.Manufacturer).Include(p => p.Category).Include(p => p.NicotineType).Include(p => p.Strength).Where(p => p.CategoryId == ct.Id).Where(p => p.Title.Contains(name)).FirstOrDefault());
                                /*products.Concat(db.Products.Include(p => p.Manufacturer).Include(p => p.Category).Include(p => p.NicotineType).Include(p => p.Strength).Where(p => p.CategoryId == ct.Id).Where(p => p.Title.Contains(name)));*/
                            }
                        }                       
                    }                        
                    else if (manufacturer != null && manufacturer != 0)
                        products = products.Where(p => p.ManufacturerId == manufacturer).Where(p => p.Title.Contains(name));
                    else
                        products = products.Where(p => p.Title.Contains(name));
               }
                else
                {
                    if (category != null)
                    {
                        var ctg = db.Categories.Where(c => c.ParentCategoryId == category).ToList();
                        productList = products.Where(p => p.CategoryId == category).ToList();
                        //products = products.Where(p => p.CategoryId == category);
                        if (ctg.Count != 0)
                        {
                            foreach (var ct in ctg)
                            {
                                var tempProduct = db.Products.Include(p => p.Manufacturer).Include(p => p.Category).Include(p => p.NicotineType).Include(p => p.Strength).Where(p => p.CategoryId == ct.Id).FirstOrDefault();
                                if (tempProduct != null)
                                    productList.Add(tempProduct);
                                /* products.Concat(db.Products).Include(p => p.Manufacturer).Include(p => p.Category).Include(p => p.NicotineType).Include(p => p.Strength).Where(p => p.CategoryId == ct.Id);*/
                            }
                        }
                    }
                    else
                    {
                        productList = products.ToList();
                    }
                    if (manufacturer != null && manufacturer !=0)
                        products = products.Where(p => p.ManufacturerId == manufacturer);

                }
               List<Manufacturer> manufacturers = db.Manufacturers.ToList();
               manufacturers.Insert(0, new Manufacturer { Title = "Все", Id = 0 });
               ProductsAttributesCategories PAC = new ProductsAttributesCategories
               {
                    Products = productList,
                    Categories = db.Categories.ToList(),
                    Manufacturers = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(manufacturers, "Id", "Title")
               };
               return View(PAC);
            }
        }

        public IActionResult Menu()
        {
            return PartialView();
        }
    }
}
//Scaffold-DbContext "Data Source=(local);Initial Catalog=vapeshop;Integrated Security=True;Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer