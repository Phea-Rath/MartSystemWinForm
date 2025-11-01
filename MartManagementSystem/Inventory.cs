using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class Inventory:Inventory_Detal
    {
        public int InventoryID { get; set; }
        public string Description { get; set; }
        public int CreateBy { get; set; }
        
        public Inventory(int inventoryID, string description, int createBy, int productID, int quantity, DateTime expireDate)
        {
            InventoryID = inventoryID;
            Description = description;
            CreateBy = createBy;
            ProductID = productID;
            Quantity = quantity;
            ExpireDate = expireDate;
        }
    }
}
