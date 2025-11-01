using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public Role(int roleId, string roleName, string description)
        {
            RoleId = roleId;
            RoleName = roleName;
            Description = description;
        }
    }
}
