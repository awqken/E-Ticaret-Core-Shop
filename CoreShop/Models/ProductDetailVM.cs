using CoreShop.MODEL.Entites;

namespace CoreShop.Models
{
    public class ProductDetailVM
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}