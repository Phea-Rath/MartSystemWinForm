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
    public partial class SizeForm : Form
    {
        public SizeForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            List<Size> size_list = new List<Size>()
            {
                new Size(1,"Small",201),
                new Size(2,"Medium",202),
                new Size(3,"Large",203),
                new Size(4,"Extra Large",204),
            };
            dgvData.DataSource = size_list;
        }
    }
}
