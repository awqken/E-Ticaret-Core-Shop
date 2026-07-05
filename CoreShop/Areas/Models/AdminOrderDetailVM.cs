using CoreShop.MODEL.Entities;

namespace CoreShop.Areas.Models
{
    public class AdminOrderDetailVM
    {
        public required Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
