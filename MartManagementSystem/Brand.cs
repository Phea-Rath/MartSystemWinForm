using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int CreatedBy { get; set; }
        public Brand(int brandId, string brandName, int createdBy)
        {
            BrandId = brandId;
            BrandName = brandName;
            CreatedBy = createdBy;
        }
    }
}
