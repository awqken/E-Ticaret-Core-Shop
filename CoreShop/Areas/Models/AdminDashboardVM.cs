using CoreShop.MODEL.Entities;

namespace CoreShop.Areas.Models
{
    public class AdminDashboardVM
    {
        public int ProductCount { get; set; }
        public int CategoryCount { get; set; }
        public int OrderCount { get; set; }
        public int UserCount { get; set; }

        public List<Product> LastProducts { get; set; } = new List<Product>();
        public List<Product> LowStockProducts { get; set; } = new List<Product>();
        public int LowStockCount { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}
