using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
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
        public IActionResult jqTest()
        {
            return View();
        }
        
        public IActionResult AboutProduct(int id)
        {

            using (VapeshopContext db = new VapeshopContext())
            {
                var cotn = db.ProductCounts.Where(x => x.ProductId == id).Select(s => new
                {
                   _count = s.Count,
                }).FirstOrDefault();
                

                ProductViewsTransactions PVT = new ProductViewsTransactions
                {
                    
                    Product = db.Products.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.Strength).Include(p => p.NicotineType).Where(p => p.Id == id).FirstOrDefault(),
                    Count = cotn._count.Value

                    /*Views = recorder.GetProductViews(id),
                    productTransactions = recorder.GetProductTransactions(id)*/
                };
                ViewBag.manufacturers = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(recorder.GetManufacturers(), "Id", "Title", PVT.Product.ManufacturerId.ToString());
                ViewBag.categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(recorder.GetTrueCategories(), "Id", "Title", PVT.Product.CategoryId.ToString());
                ViewBag.nicotine = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(recorder.GetNicotineType(), "Id", "Title", PVT.Product.NicotineTypeId.ToString());
                ViewBag.strenght = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(recorder.GetStrenghts(), "Id", "Title", PVT.Product.StrengthId.ToString());
                return View(PVT);
            }                
        }
        public IActionResult Delete(int id)
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                Product? product = db.Products.Where(p => p.Id == id).FirstOrDefault();
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        #region HTTPS
        [HttpPost]
        public async Task<IActionResult> _Quantity(int idProduct, int quantity)//Либо же отправлять модель полностью и возвращать также
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                ProductCount? productCount = db.ProductCounts.Where(pc => pc.ProductId == idProduct).FirstOrDefault();
                if (productCount != null)
                {
                    productCount.Count += quantity;
                }
                await db.SaveChangesAsync();

                ReplenishmentProduct RP = new ReplenishmentProduct
                {
                    ProductId = idProduct,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Quantity = quantity
                };
                db.ReplenishmentProducts.Add(RP);
                await db.SaveChangesAsync();

                return Json(productCount.Count);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductViewsTransactions PVT, IFormFile? PhotoFile)
        {
            /*if (ModelState.IsValid)
            {*/
                using (VapeshopContext db = new VapeshopContext())
                {
                    Product? product = db.Products.Where(p => p.Id == PVT.Product.Id).FirstOrDefault();
                    if (product != null)
                    {
                        product.Title = PVT.Product.Title;
                        product.Cost = PVT.Product.Cost;
                        product.Material = PVT.Product.Material;
                        product.Taste = PVT.Product.Taste;
                        product.CategoryId = PVT.Product.CategoryId;
                        product.NicotineTypeId = PVT.Product.NicotineTypeId;
                        product.StrengthId = PVT.Product.StrengthId;
                        product.ManufacturerId = PVT.Product.ManufacturerId;
                        if (PhotoFile !=null)
                        {
                            byte[] imageData = null;
                            using (var binaryReader = new BinaryReader(PhotoFile.OpenReadStream()))
                                imageData = binaryReader.ReadBytes((int)PhotoFile.Length);
                            product.Image = imageData;         
                        }
                        await db.SaveChangesAsync();
                    }
                    return RedirectToAction("Index");
                }
            /*}*/
            /*string errorMessages = "";
            foreach (var item in ModelState)
            {
                if (item.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        errorMessages = $"{errorMessages}{error.ErrorMessage}\n";
                    }
                }
            }
            return Content(errorMessages);*/
        }


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