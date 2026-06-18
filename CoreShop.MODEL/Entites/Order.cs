using CoreShop.CORE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreShop.MODEL.Entites
{
    public class Order : CoreEntity
    {
        public int UserId { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending"; 

        public DateTime OrderDate { get; set; } = DateTime.Now;

       
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string FullAddress { get; set; }
        public string CardName { get; set; }
        public string CardLast4 { get; set; }

        
        public User User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
