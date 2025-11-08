using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class Permission:Menu
    {
        public int UserId { get; set; }

        public Menu Menu { get; set; }
        public User User { get; set; }
    }
}
