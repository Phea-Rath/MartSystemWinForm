using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class Purchase
    {
        public int PurchaseID { get; set; }
        public int SupplierID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal subTotal { get; set; }
        public int Tax { get; set; }
        public int Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public int CreateBy { get; set; }
        public string Description { get; set; }
        public decimal Payment { get; set; }
        public decimal Balance { get; set; }
        public decimal Fee { get; set; }
        public Purchase(int purchaseID, int supplierID, DateTime purchaseDate, decimal subTotal, int tax, int discount, decimal totalAmount, int createBy, string description, decimal payment, decimal balance, decimal fee)
        {
            PurchaseID = purchaseID;
            SupplierID = supplierID;
            PurchaseDate = purchaseDate;
            this.subTotal = subTotal;
            Tax = tax;
            Discount = discount;
            TotalAmount = totalAmount;
            CreateBy = createBy;
            Description = description;
            Payment = payment;
            Balance = balance;
            Fee = fee;
        }
    }
}
