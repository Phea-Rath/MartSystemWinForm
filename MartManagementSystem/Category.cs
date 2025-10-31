using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CreatedBy { get; set; }
        public Category(int categoryId, string categoryName, int createdBy)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            CreatedBy = createdBy;
        }
    }
}
