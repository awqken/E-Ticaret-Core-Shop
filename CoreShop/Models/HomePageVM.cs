using CoreShop.MODEL.Entities;

namespace CoreShop.Models
{
    public class HomePageVM
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
