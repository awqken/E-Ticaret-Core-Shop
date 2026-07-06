using CoreShop.MODEL.Entities;

namespace CoreShop.Models
{
    public class ProfileOrderVM
    {
        public required Order Order { get; init; }
        public List<OrderDetail> Details { get; init; } = new List<OrderDetail>();
    }
}
