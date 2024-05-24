using vp_server.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using vp_server.Models.ViewModels;
using vp_server.Models;
using Microsoft.EntityFrameworkCore;

namespace Test_vp_server
{
    [TestClass]
    public class TestHomeController
    {
        VapeshopContext db = new VapeshopContext();
        [TestMethod]
        public void TestDataModelFromIndex()//“естирование модели,передаваемой в представление
        {
            var controller = new HomeController();
            var result = controller.Index(null,null,null) as ViewResult;
            Assert.IsNotNull(result);
            var modelFromController = (ProductsAttributesCategories)result.ViewData.Model;
            Assert.IsNotNull(modelFromController);

            
            List<Manufacturer> manufacturers = db.Manufacturers.ToList();
            manufacturers.Insert(0, new Manufacturer { Title = "¬се", Id = 0 });
            ProductsAttributesCategories PAC = new ProductsAttributesCategories
            {
                Products = db.Products.Include(p => p.Manufacturer).Include(p => p.Category).Include(p => p.NicotineType).Include(p => p.Strength),
                Categories = db.Categories.ToList(),
                Manufacturers = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(manufacturers, "Id", "Title"),
                categroyNow = null,
                productNameNow = null,
                manufacturerNow = null
            };
            Assert.AreEqual(PAC.Products.Count(), modelFromController.Products.Count());
            Assert.AreEqual(PAC.Manufacturers.Count(), modelFromController.Manufacturers.Count());
        }

        [TestMethod]
        public void TestDataModelFromAboutProduct()//“естирование модели, возвращаемой представлением, получившим ID записи продукции
        {
            int productID = 18; //ID тестируемой записи продукта
            var controller = new HomeController();
            var result = controller.AboutProduct(productID) as ViewResult;
            Assert.IsNotNull(result);
            var productModelFromController = (ProductViewsTransactions)result.ViewData.Model;
            Assert.IsNotNull(productModelFromController);

            var cotn = db.ProductCounts.Where(x => x.ProductId == productID).Select(s => new
            {
                _count = s.Count,
            }).FirstOrDefault();
            ProductViewsTransactions PVT = new ProductViewsTransactions
            {

                Product = db.Products.Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.Strength).Include(p => p.NicotineType).Where(p => p.Id == productID).FirstOrDefault(),
                Count = cotn._count.Value
            };
            Assert.AreEqual(PVT.Product.Title, productModelFromController.Product.Title);
            Assert.AreEqual(PVT.Count, productModelFromController.Count);
        }
        [TestMethod]
        public void TestDataModelFromAddManufacturer()//ѕроверка возращаемых платежных данных их представлени€
        {
            var controller = new HomeController();
            var result = controller.AddManufacturer() as ViewResult;
            Assert.IsNotNull(result);
            var paymentDetailsFromController = (PaymentManufacturer)result.ViewData.Model;
            Assert.IsNotNull(paymentDetailsFromController);
            PaymentManufacturer PM = new PaymentManufacturer
            {
                PaymentDetail = db.PaymentDetails.FirstOrDefault()
            };
            Assert.IsNotNull(PM);
            Assert.AreEqual(PM.PaymentDetail.FirmName, paymentDetailsFromController.PaymentDetail.FirmName);
            Assert.AreEqual(PM.PaymentDetail.PersonalRs, paymentDetailsFromController.PaymentDetail.PersonalRs);
            Assert.AreEqual(PM.PaymentDetail.BankInn, paymentDetailsFromController.PaymentDetail.BankInn);
        }
    }
}