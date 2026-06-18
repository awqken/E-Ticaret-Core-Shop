using CoreShop.MODEL.Entites;

namespace CoreShop.Areas.Models
{
    public class AdminOrderDetailVM
    {
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}