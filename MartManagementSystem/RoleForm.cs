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
    public partial class RoleForm : Form
    {
        public RoleForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            List<Role> roles = new List<Role>()
            {
                new Role(1,"Admin","Administrator"),
                new Role(2,"Cachier","Generate invoice")
            };
            dgvData.DataSource = roles;
        }
    }
}
