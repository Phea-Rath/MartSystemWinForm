using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class Product
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public int SizeId { get; set; }
        public int CreatedBy { get; set; }
        public double ProductPrice { get; set; }
        public double ProductDiscount { get; set; }
        public double ProductCost { get; set; }
        public double ProductTax { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        public Product(int productId, string productCode, string productName, int brandId, int sizeId, int createdBy, double productPrice, double productDiscount, double productCost, double productTax, string description, int categoryId)
        {
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            BrandId = brandId;
            SizeId = sizeId;
            CreatedBy = createdBy;
            ProductPrice = productPrice;
            ProductDiscount = productDiscount;
            ProductCost = productCost;
            ProductTax = productTax;
            Description = description;
            CategoryId = categoryId;
        }

    }
}
