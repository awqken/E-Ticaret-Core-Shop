using CoreShop.CORE.Entity;

namespace CoreShop.MODEL.Entities
{
    public class Order : CoreEntity
    {
        public int UserId { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime OrderDate { get; set; } = DateTime.Now;

        // Delivery snapshot taken at checkout time
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string FullAddress { get; set; } = string.Empty;

        // Payment snapshot: only the card holder name and last 4 digits are stored
        public string CardName { get; set; } = string.Empty;
        public string CardLast4 { get; set; } = string.Empty;

        // Navigation
        public User? User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
