using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsignedComputers.Core
{
  public class Product
  {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [MaxLength(100), Required]
    public string Name { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    [Range(0, 10000), Required]
    public decimal PriceUSD { get; set; }

  }
}
