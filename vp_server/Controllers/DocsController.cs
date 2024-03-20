using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using vp_server.Utils;
using vp_server.Models;
using vp_server.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace vp_server.Controllers
{
    public class DocsController : Controller
    {
        public IActionResult ExcelDocCreate()
        {
            return View();
        }
        
        public IActionResult AllTransactions()
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                List<Models.Transaction> trs = db.Transactions.ToList();
                return View(trs);
            }  
        }
        public IActionResult InfoTransaction(int IdTransaction)
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                //List<TransactionsAndProduct> TAP = db.TransactionsAndProducts.Where(t=>t.TransactionId==IdTransaction).Include(t=>t.Product).ToList();
                var products = from t in db.TransactionsAndProducts.Where(tp => tp.TransactionId == IdTransaction).Include(tp => tp.Product)
                               select new TransactionProductDTO()
                               {
                                   Id = t.Product.Id,
                                   Name = t.Product.Title,
                                   Cost = t.Product.Cost,
                                   Quality = t.Quantitly
                               };

                return View(products.ToList());
            }            
        }
        #region HTTP
        /*[HttpPost]
        public async Task<IActionResult> CreateDocument()
        {
            
        }*/
        #endregion
    }
}
