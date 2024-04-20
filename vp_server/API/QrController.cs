using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using vp_server.Models;
using ZXing.QrCode;
using ZXing;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vp_server.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrController : ControllerBase
    {
        // GET api/<QrController>/5
        [HttpGet("{sum}")]
        public async Task <ActionResult<byte[]>> Get(string sum)
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
                    return Ok((byte[])converter.ConvertTo(QRcode, typeof(byte[])));

                }
                else
                    return BadRequest();
            }
        }       
    }
}
