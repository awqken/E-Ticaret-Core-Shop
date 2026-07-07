using CoreShop.CORE.Entity;
using System.ComponentModel.DataAnnotations;

namespace CoreShop.MODEL.Entities
{
    public class Product : CoreEntity
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public decimal ProductPrice { get; set; }

        /// <summary>Previous list price; when set and higher than ProductPrice the product is on sale.</summary>
        public decimal? OldPrice { get; set; }

        [Required]
        public int ProductStock { get; set; }

        public string? ProductImage { get; set; }

        [Required]
        public string ProductBrand { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
