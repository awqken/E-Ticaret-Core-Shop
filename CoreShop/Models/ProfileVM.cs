using CoreShop.MODEL.Entites;
using Microsoft.AspNetCore.Mvc;

namespace CoreShop.Models
{

        public class ProfileVM
        {
            public User User { get; set; }
            public List<Order> Orders { get; set; }
            public List<OrderDetail> OrderDetails { get; set; }
        }
    
}
