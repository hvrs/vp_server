using Microsoft.AspNetCore.Mvc;
using vp_server.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vp_server.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewController : ControllerBase
    {

        // POST api/<ViewController>/5
        [HttpPost("{id}")]
        public async void Post(int id, DateOnly date, TimeOnly time)
        {
            using(VapeshopContext db = new VapeshopContext())
            {
                View view = new View
                {
                    Date = date,
                    Time = time,
                    ProductId = id
                };
                db.Views.Add(view);
                await db.SaveChangesAsync();
            }
        }    
    }
}
