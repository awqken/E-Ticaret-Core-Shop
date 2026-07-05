using CoreShop.MODEL.Entities;

namespace CoreShop.Models
{
    public class ProductDetailVM
    {
        public required Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; } = new List<Product>();
    }
}
