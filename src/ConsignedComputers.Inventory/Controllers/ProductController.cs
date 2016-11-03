using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ConsignedComputers.Inventory.Controllers
{
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiVersion("1.0")]
  public class ProductController : ControllerBase
  {

    public ProductController(Models.InventoryContext ctx)
    {
      this.DbContext = ctx;
    }

    public Models.InventoryContext DbContext { get; }

    /// <summary>
    /// Get records of all products in inventory
    /// </summary>
    /// <returns>All products of type <see cref="Models.ProductInventory"/></returns>
    [HttpGet]
    public IEnumerable<Models.ProductInventory> Get()
    {
      return DbContext.Products;
    }

    /// <summary>
    /// Get a specific product by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Product with matching id</returns>
    [HttpGet("{id}")]
    public Models.ProductInventory Get(int id)
    {
      return DbContext.Products.FirstOrDefault(p => p.ID == id);
    }

    /// <summary>
    /// Add a new Product to inventory
    /// </summary>
    /// <param name="newProduct"></param>
    /// <response code="201">The newly created Product</response>
    /// <response code="400">Invalid input</response>
    [HttpPost]
    [ProducesResponseType(typeof(Models.ProductInventory), 201)]
    [ProducesResponseType(typeof(ModelStateDictionary), 400)]
    public IActionResult Post([FromBody]Models.ProductInventory newProduct)
    {

      if (ModelState.IsValid)
      {
        newProduct.LastUpdate = DateTime.UtcNow;
        DbContext.Add(newProduct);
        DbContext.SaveChanges();

        return CreatedAtAction("Get", new {id=newProduct.ID }, newProduct);

      }

      return BadRequest(ModelState);

    }

    /// <summary>
    /// Modify the in-stock count of the product
    /// </summary>
    /// <param name="id">Identifier of the product to modify</param>
    /// <param name="count">Count adjustment to make to the in-stock count</param>
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]int count)
    {
      var thisProduct = DbContext.Products.FirstOrDefault(p => p.ID == id);
      if (thisProduct == null) return base.BadRequest();

      thisProduct.LastUpdate = DateTime.UtcNow;
      thisProduct.InStock += count;

      DbContext.SaveChanges();
      return new OkObjectResult(thisProduct);


    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {

      DbContext.Products.Remove(DbContext.Products.First(p => p.ID == id));
      DbContext.SaveChanges();

      return base.Ok();

    }
  }
}
