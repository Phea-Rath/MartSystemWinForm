using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class ProductInventory:Product
    {
        private int quantity;
        private decimal subTotal;
        private DateTime expireDate;
        
        public ProductInventory()
        {
        }
        public static List<ProductInventory> products { get; set; } = new List<ProductInventory>();
        public DateTime ExpireDate { get => expireDate; set => expireDate = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public decimal SubTotal { get => subTotal; set => subTotal = value; }
    }
}
