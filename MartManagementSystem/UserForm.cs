using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            List<User> users = new List<User>()
           {
               new User(1,"Admin","123","admin@gmail.com","0979797977","image.png")
           };
        }
    }
}
