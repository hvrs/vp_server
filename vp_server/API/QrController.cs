using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using vp_server.Models;
using ZXing.QrCode;
using ZXing;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Drawing.Imaging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vp_server.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrController : ControllerBase
    {
        // GET api/<QrController>/5 
        [HttpGet("{sum}")]
        public async Task <ActionResult<ImageDto>> Get(string sum)
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                PaymentDetail paymentDetail = await db.PaymentDetails.FirstOrDefaultAsync();
                if (paymentDetail != null)
                {
                    string paymenMessage = $"ST00012|Name={paymentDetail.FirmName}" +
                    $"|PersonalAcc={paymentDetail.PersonalRs}|BankName={paymentDetail.BankName}" +
                    $"|BIC={paymentDetail.Bik}|CorrespAcc={paymentDetail.BankKs}" +
                    $"|PayeeINN={paymentDetail.BankInn}|KPP={paymentDetail.BankKpp}" +
                    $"|Sum={sum}|Purpose=Тестовая проверка QR|Contract=1111";

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
                    ImageDto imageDto = new ImageDto
                    {
                        image = (byte[])converter.ConvertTo(QRcode, typeof(byte[]))
                    };
                    return Ok(imageDto);
                    

                }
                else
                    return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult<int>> Put([FromBody] idProductsInBasketAndSum basketAndSum)//Добавление транзакции в базу данныхы
        {
            using(VapeshopContext db = new VapeshopContext())
            {
                Transaction transaction = new Transaction
                {
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Time = TimeOnly.FromDateTime(DateTime.Now),
                    Sum = basketAndSum.Sum,
                    IsViewed = false,
                    TransactionStatusId = 1
                };
                db.Transactions.Add(transaction);
                await db.SaveChangesAsync();
                foreach (var i in basketAndSum.productQ)
                {
                    TransactionsAndProduct tP = new TransactionsAndProduct
                    {
                        TransactionId = transaction.Id,
                        ProductId = i.ProductID,
                        Quantitly = i.Quantity
                    };
                    db.TransactionsAndProducts.Add(tP);
                    await db.SaveChangesAsync();
                }
                return Ok(transaction.Id);
            }
        }
    }

    public class ImageDto
    {
        public byte[]? image { get; set; }
    }
}
