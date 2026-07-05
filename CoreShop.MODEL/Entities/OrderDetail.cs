using CoreShop.CORE.Entity;

namespace CoreShop.MODEL.Entities
{
    public class OrderDetail : CoreEntity
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // Product snapshot taken at checkout time
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }

        // Navigation
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
