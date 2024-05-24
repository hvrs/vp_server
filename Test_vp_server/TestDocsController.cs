using vp_server.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using vp_server.Models.ViewModels;
using vp_server.Models;
using Microsoft.EntityFrameworkCore;

namespace Test_vp_server
{
    [TestClass]
    public class TestDocsController
    {
        VapeshopContext db = new VapeshopContext();
        [TestMethod]
        public void TestDataModelAllTransaction()
        {
            var controller = new DocsController();
            var result = controller.AllTransactions() as ViewResult;
            Assert.IsNotNull(result);
            var modelFromController = (List<Transaction>)result.ViewData.Model;
            Assert.IsNotNull(modelFromController);

            List<Transaction> transactions = db.Transactions.ToList();
            Assert.AreEqual(transactions.Count(), modelFromController.Count());
            Assert.AreEqual(transactions.First().Id, modelFromController.First().Id);
        }
        [TestMethod]
        public void TestDataModelInfoTransaction()
        {
            int idTransaction = db.Transactions.FirstOrDefault().Id; //Получение из БД ID транзакции
            var controller = new DocsController();
            var result = controller.InfoTransaction(idTransaction) as ViewResult;
            Assert.IsNotNull(result);
            var modelFromController = (ProductsInTransactionWithStatus)result.ViewData.Model;
            Assert.IsNotNull(modelFromController);

            Transaction transaction = db.Transactions.Where(t => t.Id == idTransaction).FirstOrDefault();
            var products = from t in db.TransactionsAndProducts.Where(tp => tp.TransactionId == idTransaction).Include(tp => tp.Product).Include(tp => tp.Transaction)
                           select new TransactionProductDTO()
                           {
                               Id = t.Product.Id,
                               Name = t.Product.Title,
                               Cost = t.Product.Cost,
                               Quality = t.Quantitly
                           };
            var status = db.Transactions.Where(tp => tp.Id == idTransaction).Include(t => t.TransactionStatus).FirstOrDefault();

            ProductsInTransactionWithStatus PTS = new ProductsInTransactionWithStatus
            {
                Id = idTransaction,
                tpDTO = products.ToList(),
                Status = status.TransactionStatus.Title
            };
            Assert.AreEqual(PTS.Id, modelFromController.Id);
            Assert.AreEqual(PTS.tpDTO.Count(), modelFromController.tpDTO.Count());   
        }
    }
}
