using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using vp_server.Utils;
using vp_server.Models;
using vp_server.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.IO;
using OfficeOpenXml;

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
                Models.Transaction transaction = db.Transactions.Where(t => t.Id == IdTransaction).FirstOrDefault();
                if (transaction!=null)
                {
                    transaction.IsViewed = true;
                    db.SaveChanges();
                }
                var products = from t in db.TransactionsAndProducts.Where(tp => tp.TransactionId == IdTransaction).Include(tp => tp.Product).Include(tp => tp.Transaction)
                               select new TransactionProductDTO()
                               {
                                   Id = t.Product.Id,
                                   Name = t.Product.Title,
                                   Cost = t.Product.Cost,
                                   Quality = t.Quantitly
                               };
                var status = db.Transactions.Where(tp => tp.Id == IdTransaction).Include(t => t.TransactionStatus).FirstOrDefault();
                             
                ProductsInTransactionWithStatus PTS = new ProductsInTransactionWithStatus
                {
                    Id = IdTransaction,
                    tpDTO = products.ToList(),
                    Status = status.TransactionStatus.Title
                };
                return View(PTS);
            }
        }

        #region HTTP
        [HttpPost]
        public IActionResult excelToDatabase(IFormFile excelFile)//Получение Excel файла, его обработка и запись данных в List
        {
            if (excelFile != null)
            {
                var stream = excelFile.OpenReadStream();
                
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int colCount = worksheet.Dimension.End.Column;
                    int rowCount = worksheet.Dimension.End.Row;
                    ValidateExcel validate = new ValidateExcel();
                    if (validate.columnValidate(worksheet))
                    {//Необходимо записать данные в List 


                        return Content($"столбцы: {colCount}, строки: {rowCount}");
                    }
                    else
                    {
                        return Content($"столбцы не соответствуют принимаемой форме");
                    }


                }
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> SetAccept(int idTransaction)
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                Models.Transaction? trns = db.Transactions.Where(t => t.Id == idTransaction).Include(t => t.TransactionStatus).FirstOrDefault();
                if (trns != null)
                {
                    trns.TransactionStatusId = 2;
                    db.SaveChanges();
                    return Json(trns.TransactionStatus.Title);
                }
                return BadRequest();
            }         
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument(DateOnly dateStart, DateOnly dateEnd)
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                ExcelDataModel excelData = new ExcelDataModel
                {
                    totalSales = db.Transactions.Where(ts => (ts.Date >= dateStart) && (ts.Date <= dateEnd) && (ts.IsViewed == true) && (ts.TransactionStatusId == 2)).Count(),              
                    totalSum = (float)db.Transactions.Where(ts => (ts.Date >= dateStart) && (ts.Date <= dateEnd) && (ts.IsViewed == true) && (ts.TransactionStatusId == 2)).Sum(ts => ts.Sum),
                    productRest = db.ProductCounts.Sum(pc => pc.Count),
                    productReceipt = db.ReplenishmentProducts.Where(rp=>(rp.Date >= dateStart) && (rp.Date <= dateEnd)).Sum(rp=>rp.Quantity),
                    //Поступившая за выбранный промежуток продукция
                    receiptProducts = from p in db.ReplenishmentProducts.Include(rp=>rp.Product).Include(rp=>rp.Product.ProductCounts).Where(rp=>(rp.Date >= dateStart) && (rp.Date <= dateEnd))
                                   select new receiptProduct()
                                   {
                                       productName = p.Product.Title,
                                       cost = (float)p.Product.Cost,
                                       quantity = p.Quantity,
                                       dateReceipt = p.Date
                                   },
                    //Оставшаяся продукция
                    restProducts = from pr in db.ProductCounts.Include(p => p.Product).Where(pc => pc.Count > 0)
                                   select new restProduct()
                                   {
                                       productName = pr.Product.Title,
                                       remains = pr.Count,
                                       cost = (float)pr.Product.Cost
                                   }
                };
 
                
                var Excel = new ExcelGenerator()
                    .Generate(excelData);

                ExcelDocument? excelSave = db.ExcelDocuments.Where(x => x.Id == 1).FirstOrDefault();
                if (excelSave != null)
                {
                    excelSave.DocExcel = Excel;
                    await db.SaveChangesAsync();
                }
                //var exePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\"));                                                             
                return Ok(1);

                //return File(Excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Отчет {dateStart}-{dateEnd}.xlsx");
            }
        }
        #endregion
        public async Task<IActionResult> Download(int id)
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                byte[]? Excel = db.ExcelDocuments.Where(i => i.Id == id).Select(i => i.DocExcel).FirstOrDefault();
                if (Excel != null)
                {
                    return File(Excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчет.xlsx");
                }
                return BadRequest();
            }
        }
    }
}

