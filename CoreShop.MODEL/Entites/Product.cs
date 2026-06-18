using CoreShop.CORE.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CoreShop.MODEL.Entites
{
    public class Product : CoreEntity
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [Required]
        public int ProductStock { get; set; }

        public string? ProductImage { get; set; }

        [Required]
        public string ProductBrand { get; set; }

        public int CategoryId { get; set; } 
        public Category Category { get; set; }
    }
}
