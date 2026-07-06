using CoreShop.CORE.Entity;
using System.ComponentModel.DataAnnotations;

namespace CoreShop.MODEL.Entities
{
    public class Category : CoreEntity
    {
        [Required]
        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
