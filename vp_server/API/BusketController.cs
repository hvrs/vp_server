using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vp_server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vp_server.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusketController : ControllerBase
    {

        // POST api/<BusketController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BusketController>/5
        [HttpPut]
        public async void Put([FromBody] ProductToB product)
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                if (db.ProductBaskets.Any())
                {
                    if (await db.ProductBaskets.AnyAsync(pb => pb.ProductId == product.ProductId))
                    {
                        ProductBasket pb = await db.ProductBaskets.Where(pb => pb.ProductId == product.ProductId).FirstOrDefaultAsync();
                        if (pb != null)
                            pb.Quantity++;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        ProductBasket productBasket = new ProductBasket
                        {
                            ProductId = product.ProductId,
                            Quantity = 1
                        };
                        db.ProductBaskets.Add(productBasket);
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    ProductBasket productBasket = new ProductBasket
                    {
                        ProductId = product.ProductId,
                        Quantity = 1
                    };
                    db.ProductBaskets.Add(productBasket);
                    await db.SaveChangesAsync();
                }
            }

        }

        // DELETE api/<BusketController>
        [HttpDelete]
        public async void Delete()//https://stackoverflow.com/questions/15220411/entity-framework-delete-all-rows-in-table
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ProductBascket]");
            }
        }

    }
}
