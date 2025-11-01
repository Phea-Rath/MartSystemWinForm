using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class ProductInventory
    {
        private int productId;
        private string productName;
        private int quantity;
        private DateTime expiryDate;
        public ProductInventory(int productId, string productName, int quantity, DateTime expiryDate)
        {
            this.productId = productId;
            this.productName = productName;
            this.quantity = quantity;
            this.expiryDate = expiryDate;
        }

        public int ProductId { get => productId; set => productId = value; }
        public string ProductName { get => productName; set => productName = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public DateTime ExpiryDate { get => expiryDate; set => expiryDate = value; }


    }
}
