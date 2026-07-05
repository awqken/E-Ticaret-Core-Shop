using CoreShop.MODEL.Entities;

namespace CoreShop.Models
{
    public class ProfileVM
    {
        public required User User { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
