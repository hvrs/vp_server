using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using vp_server.Utils;
using vp_server.Models;
using vp_server.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.IO;
using OfficeOpenXml;
using System.Text.Json;

using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.Drawing;
using ZXing.QrCode.Internal;


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
        public async Task<IActionResult> excelToDatabase(IFormFile excelFile)//Получение Excel файла, его обработка и запись данных в List
        {
            
            if (excelFile != null)
            {
                var stream = excelFile.OpenReadStream();
                
                using (ExcelPackage package = new ExcelPackage(stream))
                {                    
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];                   
                    ExcelGenerate validate = new ExcelGenerate();
                    if (validate.columnValidate(worksheet))
                    {
                        IEnumerable<ProductExcelDTO> excelCollection = worksheet.FromSheetToModel<ProductExcelDTO>();                       
                        using (VapeshopContext db = new VapeshopContext())
                        {                          
                            foreach (var i in excelCollection)
                            {
                                
                                    if (i.Category is not null && i.Title is not null && i.Cost != 0 && db.Categories.Any(c => c.CategoryName.ToLower() == i.Category.ToLower()))//Проверка на то, что категория товара, переданного из Excel есть в БД
                                    {
                                        Product product = new Product();//Проверка на null!!!
                                        if (db.NicotineTypes.Any(nt => nt.Title.ToLower() == i.Nicotine.ToLower()))
                                        {
                                            product.NicotineTypeId = db.NicotineTypes.Where(nt => nt.Title.ToLower() == i.Nicotine.ToLower()).FirstOrDefault().Id;
                                        }
                                        else if (i.Nicotine != null)
                                        {
                                            NicotineType nicotineType = new NicotineType
                                            {
                                                Title = i.Nicotine
                                            };
                                            db.NicotineTypes.Add(nicotineType);
                                            await db.SaveChangesAsync();
                                            product.NicotineTypeId = nicotineType.Id;
                                        }
                                        if (db.Manufacturers.Any(m => m.Title.ToLower() == i.Manufacturer.ToLower()))
                                        {
                                            product.ManufacturerId = db.Manufacturers.Where(m => m.Title.ToLower() == i.Manufacturer.ToLower()).FirstOrDefault().Id;
                                        }
                                        else if (i.Manufacturer != null)
                                        {
                                            Manufacturer manufacturer = new Manufacturer
                                            {
                                                Title = i.Manufacturer
                                            };
                                            db.Manufacturers.Add(manufacturer);
                                            await db.SaveChangesAsync();
                                            product.ManufacturerId = manufacturer.Id;
                                        }
                                        if (db.Strenghts.Any(s => s.Title.ToLower() == i.Strength.ToLower()))
                                        {
                                            product.StrengthId = db.Strenghts.Where(s => s.Title.ToLower() == i.Strength.ToLower()).FirstOrDefault().Id;
                                        }
                                        else if (i.Strength != null)
                                        {
                                            Strenght strenght = new Strenght
                                            {
                                                Title = i.Strength
                                            };
                                            db.Strenghts.Add(strenght);
                                            await db.SaveChangesAsync();
                                            product.StrengthId = strenght.Id;
                                        }
                                        //Чтото возможно не так.Нужно придумать решение, если невозможные для null поля будут все же равны null
                                        product.CategoryId = db.Categories.Where(c => c.CategoryName.ToLower() == i.Category.ToLower()).FirstOrDefault().Id;
                                        if (product.ManufacturerId != null)
                                        {
                                            product.Title = i.Title;
                                            product.Cost = i.Cost;
                                            product.Material = i.Material;
                                            product.Taste = i.Taste;
                                            product.Image = System.IO.File.ReadAllBytes(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Utils\\stub.jpg")));

                                            db.Products.Add(product);
                                            await db.SaveChangesAsync();

                                            ProductCount PC = new ProductCount();
                                            PC.ProductId = product.Id;
                                            if (i.Count >= 0)
                                                PC.Count = i.Count;
                                            else
                                                PC.Count = 0;
                                            db.ProductCounts.Add(PC);
                                            await db.SaveChangesAsync();
                                        }
                                    }                               
                            }
                        }
                        return RedirectToAction("Index", "Home");                  
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
                    await db.SaveChangesAsync();
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
 
                
                var Excel = new ExcelGenerate()
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
                byte[]? Excel = await db.ExcelDocuments.Where(i => i.Id == id).Select(i => i.DocExcel).FirstOrDefaultAsync();
                if (Excel != null)
                {
                    return File(Excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Отчет.xlsx");
                }
                return BadRequest();
            }
        }
        
        public IActionResult getExcelForm()
        {
            byte[] excel = System.IO.File.ReadAllBytes(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Utils\\Форма.xlsx")));
            return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Форма заполения.xlsx");
        }
        
        public IActionResult QRpayment(string sum)//Поместить в HttpGet запрос
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                PaymentDetail paymentDetail = db.PaymentDetails.FirstOrDefault();
                if (paymentDetail != null)
                {
                    string paymenMessage = $"ST00012|Name={paymentDetail.FirmName}" +
                    $"|PersonalAcc={paymentDetail.PersonalRs}|BankName={paymentDetail.BankName}" +
                    $"|BIC={paymentDetail.Bik}|CorrespAcc={paymentDetail.BankKs}" +
                    $"|PayeeINN={paymentDetail.BankInn}|KPP={paymentDetail.BankKpp}" +
                    $"|Sum={sum}.00|Purpose=Тестовая проверка QR|Contract=1111";

                    QrCodeEncodingOptions options = new()
                    {
                        DisableECI = true,
                        CharacterSet = "utf-8",
                        Width = 400,
                        Height = 400
                    };

                    var writer = new ZXing.Windows.Compatibility.BarcodeWriter();
                    writer.Format = BarcodeFormat.QR_CODE;
                    writer.Options = options;
                    var QRcode = writer.Write(paymenMessage);

                    ImageConverter converter = new ImageConverter();
                    return View((byte[])converter.ConvertTo(QRcode, typeof(byte[])));

                }
                else
                    return RedirectToAction("ExcelDocCreate");
            }
                           
        }
        
    }
}

