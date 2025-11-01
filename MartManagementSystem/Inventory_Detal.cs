using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class Inventory_Detal
    {
        public int InventoryDetailID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
