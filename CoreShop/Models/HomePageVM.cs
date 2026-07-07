using CoreShop.MODEL.Entities;

namespace CoreShop.Models
{
    public class HomePageVM
    {
        /// <summary>Showcase products for the landing page (in-stock first).</summary>
        public List<Product> FeaturedProducts { get; set; } = new();

        /// <summary>Top products by total quantity sold, computed from order details.</summary>
        public List<Product> BestSellers { get; set; } = new();

        /// <summary>Most recently added products.</summary>
        public List<Product> NewArrivals { get; set; } = new();

        public List<Category> Categories { get; set; } = new();

        /// <summary>Hero showcase product (a premium PC case when available).</summary>
        public Product? HeroProduct { get; set; }
    }
}
