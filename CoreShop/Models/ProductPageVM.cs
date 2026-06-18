using CoreShop.MODEL.Entites;

namespace CoreShop.Models
{
    public class ProductPageVM
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        public string Search { get; set; }

        public List<string> Brands { get; set; }

        public List<int> SelectedCategoryIds { get; set; } = new List<int>();
        public List<string> SelectedBrands { get; set; } = new List<string>();
        public string Sort { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
