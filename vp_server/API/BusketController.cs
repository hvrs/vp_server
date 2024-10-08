﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using vp_server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace vp_server.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusketController : ControllerBase
    {
        private VapeshopContext db = new VapeshopContext();

        // POST api/<BusketController>
        [HttpPost]
        public async void Post([FromBody] PurchaseStatus status)//Отмена покупки пользователем с последующей очисткой корзины
        {
            Transaction transaction = await db.Transactions.Where(t=>t.Id == status.transactionID).FirstOrDefaultAsync();
            if (transaction != null)
            {
                if (!status.isCompleted)
                {
                    transaction.TransactionStatusId = 3;//Отменена
                    await db.SaveChangesAsync();
                    Delete();
                }
                else
                {
                    transaction.TransactionStatusId = 2;//Оплачена
                    await db.SaveChangesAsync();
                    Delete();
                }
                
            }
           
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOProductAndQuantity>>> Get()
        {
            try
            {
                if (await db.ProductBaskets.AnyAsync())
                {
                    List<DTOProductAndQuantity> productDtoQuant = new List<DTOProductAndQuantity>();
                    List<ProductBasket> PB = await db.ProductBaskets.ToListAsync();
                    foreach (var item in PB)
                    {
                        DTOProductAndQuantity productInBucket = new DTOProductAndQuantity();
                        var prod = from pc in db.Products.Where(p => p.Id == item.ProductId)
                                             select new ProductDTO
                                            {
                                                Id = pc.Id,
                                                NameProduct = pc.Title,
                                                Photo = pc.Image,
                                                Category = pc.Category.CategoryName,
                                                Manufacturer = pc.Manufacturer.Title,
                                                Nicotine = pc.NicotineType.Title,
                                                Strength = pc.Strength.Title,
                                                Cost = pc.Cost,                                               
                                            };
                        productInBucket.product = await prod.FirstOrDefaultAsync();
                        productInBucket.quantityInBusket = item.Quantity;
                        productInBucket.quantityInWarehouse = await db.ProductCounts.Where(pc=>pc.ProductId == item.ProductId).Select(pc=>pc.Count).FirstOrDefaultAsync();
                        productDtoQuant.Add(productInBucket);
                    }
                    return Ok(productDtoQuant);
                }
                else
                {
                    List<DTOProductAndQuantity> productDtoQuant = new List<DTOProductAndQuantity>();
                    return Ok(productDtoQuant);
                }
            }
            catch
            {
                return BadRequest();
            }
        }


        // PUT api/<BusketController>/5
        [HttpPut]
        public async void Put([FromBody] ProductToB product)
        {
                if (db.ProductBaskets.Any())
                {
                    if (product.isPlus)
                    {
                        if (await db.ProductBaskets.AnyAsync(pb => pb.ProductId == product.ProductId))
                        {
                            ProductBasket pb = await db.ProductBaskets.Where(pb => pb.ProductId == product.ProductId).FirstOrDefaultAsync();
                            if (pb != null && pb.Quantity < db.ProductCounts.FirstOrDefault(pc => pc.ProductId == product.ProductId).Count)
                                pb.Quantity++; await db.SaveChangesAsync();
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
                    else if (!product.isPlus)
                    {
                        ProductBasket pb = await db.ProductBaskets.Where(pb => pb.ProductId == product.ProductId).FirstOrDefaultAsync();
                        if (pb != null)
                            pb.Quantity--;
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

        // DELETE api/<BusketController>
        [HttpDelete]
        public async void Delete()//https://stackoverflow.com/questions/15220411/entity-framework-delete-all-rows-in-table
        {
              //await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ProductBasket]");
            VapeshopContext deleteTableFor = new VapeshopContext();
            await deleteTableFor.Database.ExecuteSqlRawAsync($"SET FOREIGN_KEY_CHECKS = 0; TRUNCATE TABLE Productbasket");
            await deleteTableFor.Database.ExecuteSqlRawAsync($"SET FOREIGN_KEY_CHECKS = 1;");
        }

        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            db.ProductBaskets.Remove(await db.ProductBaskets.Where(pb => pb.ProductId == id).FirstOrDefaultAsync());
            await db.SaveChangesAsync();
        }           

    }
    public class PurchaseStatus
    {
        public int transactionID { get; set; }
        public bool isCompleted { get; set; }
    }
    
}
