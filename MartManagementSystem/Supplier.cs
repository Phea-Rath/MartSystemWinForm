using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class Supplier
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public int CreateBy { get; set; }

        public Supplier(int supplierID, string supplierName, string tel, string address, string email, int createBy)
        {
            SupplierID = supplierID;
            SupplierName = supplierName;
            Tel = tel;
            Address = address;
            Email = email;
            CreateBy = createBy;
        }

    }
}
