using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class Order:BaseEntity
    {
        public int OrderId { get; set; }
        public decimal SubTotalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }


        // Navigation
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

}
