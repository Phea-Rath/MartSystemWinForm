using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class Size
    {
        public int SizeId { get; set; }
        public string SizeName { get; set; }    
        public int CreatedBy { get; set; }
        public Size(int sizeId, string sizeName, int createdBy)
        {
            SizeId = sizeId;
            SizeName = sizeName;
            CreatedBy = createdBy;
        }
    }
}
