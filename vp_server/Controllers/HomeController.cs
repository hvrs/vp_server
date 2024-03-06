using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vp_server.Models;
using vp_server.Models.ViewModels;
using vp_server.Utils;

namespace vp_server.Controllers
{
    public class HomeController : Controller
    {
        Recorder recorder = new Recorder();
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
                        if (ctg.Count != 0)
                        {
                            foreach (var ct in ctg)
                            {
                                var tempProduct = db.Products.Include(p => p.Manufacturer).Include(p => p.Category).Include(p => p.NicotineType).Include(p => p.Strength).Where(p => p.CategoryId == ct.Id).Where(p => p.Title.Contains(name)).FirstOrDefault();
                                if(tempProduct != null)
                                    productList.Add(tempProduct);
                            }
                        }                       
                    }                        
                    else if (manufacturer != null && manufacturer != 0)
                        productList = products.Where(p => p.ManufacturerId == manufacturer).Where(p => p.Title.Contains(name)).ToList();
                    else
                        productList = products.Where(p => p.Title.Contains(name)).ToList();
               }
                else
                {
                    if (category != null)
                    {
                        var ctg = db.Categories.Where(c => c.ParentCategoryId == category).ToList();
                        productList = products.Where(p => p.CategoryId == category).ToList();
                        if (ctg.Count != 0)
                        {
                            foreach (var ct in ctg)
                            {
                                var tempProduct = db.Products.Include(p => p.Manufacturer).Include(p => p.Category).Include(p => p.NicotineType).Include(p => p.Strength).Where(p => p.CategoryId == ct.Id).FirstOrDefault();
                                if (tempProduct != null)
                                    productList.Add(tempProduct);
                            }
                        }
                    }
                    else
                    {
                        productList = products.ToList();
                    }
                    if (manufacturer != null && manufacturer !=0)
                        productList = products.Where(p => p.ManufacturerId == manufacturer).ToList();

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

        public IActionResult AddManufacturer()
        {
            return View();
        }       

        public IActionResult AddProduct()
        {
            ViewBag.manufacturers = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(recorder.GetManufacturers(), "Id", "Title");
            ViewBag.categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(recorder.GetTrueCategories(), "Id", "Title");
            ViewBag.nicotine = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(recorder.GetNicotineType(), "Id", "Title");
            ViewBag.strenght = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(recorder.GetStrenghts(), "Id", "Title");
            return View();
        }
        
        public IActionResult AboutProduct(int id)
        {
            using (VapeshopContext db = new VapeshopContext())
            {              
                ProductViewsTransactions PVT = new ProductViewsTransactions
                {
                    Product = db.Products.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.Strength).Include(p => p.NicotineType).Where(p => p.Id == id).FirstOrDefault(),
                    Views = recorder.GetProductViews(id),
                    productTransactions = recorder.GetProductTransactions(id)
                };
                return View(PVT);
            }                
        }

        #region HTTPS
        [HttpPost]
        public async Task<IActionResult> CreateManufacturer(Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                using (VapeshopContext db = new VapeshopContext())
                {
                    db.Manufacturers.Add(manufacturer);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return Content("Данные не прошли проверку");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product, IFormFile PhotoFile, int? quantity)
        {
            byte[] imageData = null;
            //Считать переданный файл в массив байтов
            if (PhotoFile != null)
            {
                using (var binaryReader = new BinaryReader(PhotoFile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)PhotoFile.Length);
                }
                ValidatewPhoto validatew = new ValidatewPhoto();
                if (validatew.IsImage(imageData))
                    product.Image = imageData;
            }
            else
            {
                var exePath = Directory.GetCurrentDirectory();
                imageData = System.IO.File.ReadAllBytes(Path.GetFullPath(Path.Combine(exePath, "Utils\\stub.jpg")));
                product.Image = imageData;
            }
            using (VapeshopContext db = new VapeshopContext())
            {
                ProductCount PC = new ProductCount();
                db.Products.Add(product);
                await db.SaveChangesAsync();

                PC.ProductId = product.Id;
                if (quantity >= 0)
                    PC.Count = quantity;
                else
                    PC.Count = 0;
                db.ProductCounts.Add(PC);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

        }
        #endregion
    }
}
//Scaffold-DbContext "Data Source=(local);Initial Catalog=vapeshop;Integrated Security=True;Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer