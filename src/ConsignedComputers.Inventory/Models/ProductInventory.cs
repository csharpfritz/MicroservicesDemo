using ConsignedComputers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsignedComputers.Inventory.Models
{

  public class ProductInventory : Product
  { 

    public int InStock { get; set; }

    public DateTime LastUpdate { get; set; }

  }

}
