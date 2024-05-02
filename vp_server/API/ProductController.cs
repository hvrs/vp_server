using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vp_server.Models;


namespace vp_server.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public VapeshopContext db = new VapeshopContext();
        // GET: api/<ProductController>
        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> Get()
        {
            try
            {
              var products = from pc in db.ProductCounts.Where(pc => pc.Count > 0).Include(pc => pc.Product).Include(pc => pc.Product.NicotineType).Include(pc => pc.Product.Category).Include(pc => pc.Product.Manufacturer).Include(pc => pc.Product.Strength)
                             select new ProductDTO
                             {
                                 Id = pc.ProductId,
                                 NameProduct = pc.Product.Title,
                                 Photo = pc.Product.Image,
                                 Category = pc.Product.Category.CategoryName,
                                 Manufacturer = pc.Product.Manufacturer.Title,
                                 Nicotine = pc.Product.NicotineType.Title,
                                 Strength = pc.Product.Strength.Title,
                                 Cost = pc.Product.Cost
                             };
                return Ok(products);
            }
            catch
            {
                return BadRequest();
            }
        }      
    }
}
