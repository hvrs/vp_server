using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using vp_server.Utils;
using vp_server.Models;

namespace vp_server.Controllers
{
    public class DocsController : Controller
    {
        public IActionResult Index()
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
        #region HTTP
        /*[HttpPost]
        public async Task<IActionResult> CreateDocument()
        {
            
        }*/
        #endregion
    }
}
