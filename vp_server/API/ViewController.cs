using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using vp_server.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vp_server.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewController : ControllerBase
    {

        // POST api/<ViewController>
        [HttpPost]
        public async void Post([FromBody] ViewDTO viewD)
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                View view = new View
                {
                    Date = viewD.Date,
                    Time = viewD.Time,
                    ProductId = viewD.ProductId
                };
                db.Views.Add(view);
                await db.SaveChangesAsync();
            }
        }    
    }
}
