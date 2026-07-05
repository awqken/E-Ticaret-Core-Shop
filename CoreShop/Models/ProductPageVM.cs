using CoreShop.MODEL.Entities;

namespace CoreShop.Models
{
    public class ProductPageVM
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<string> Brands { get; set; } = new List<string>();

        public string? Search { get; set; }
        public string? Sort { get; set; }

        public List<int> SelectedCategoryIds { get; set; } = new List<int>();
        public List<string> SelectedBrands { get; set; } = new List<string>();

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
