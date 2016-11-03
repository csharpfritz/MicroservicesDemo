using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsignedComputers.Inventory.Models
{
  public class InventoryContext : DbContext
  {

    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

    public DbSet<ProductInventory> Products { get; set; }

  }


  public static class InventoryContextExtensions
  {

    public static void EnsureSeedData(this InventoryContext ctx)
    {

      if (!ctx.Products.Any())
      {
        ctx.Products.AddRange(
        
          new ProductInventory { ID=1, Name="HDD Enclosure", Description="Metal enclosure to house your 3.5inch IDE hard drive", InStock=5, LastUpdate=DateTime.Today.AddDays(-2), PriceUSD=3.99M},
          new ProductInventory { ID=2, Name="3.5\" Floppy Drive", Description="IDE drive for reading your 3.5 inch floppy disks", InStock=2, LastUpdate=DateTime.Today.AddDays(-8), PriceUSD=5.99M},
          new ProductInventory { ID=3, Name="8MB AGP Video Card", Description="AGP mounted video card with 8MB", InStock=3, LastUpdate=DateTime.Today.AddDays(-21)}
          
        );

        ctx.SaveChanges();

      }

    }

  }

}
