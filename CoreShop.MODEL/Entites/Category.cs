using CoreShop.CORE.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreShop.MODEL.Entites
{
    public class Category : CoreEntity
    {
        [Required]
        public string CategoryName { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
