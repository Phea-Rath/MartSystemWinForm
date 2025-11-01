using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Image { get; set; }
        public User(int userId, string userName, string password, string email, string telephone, string image)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            Email = email;
            Tel = telephone;
            Image = image;
        }
    }
}
