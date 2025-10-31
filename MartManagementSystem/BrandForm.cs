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
    public partial class BrandForm : Form
    {
        public BrandForm()
        {
            InitializeComponent();
            LoadData(); 
        }

        private void LoadData()
        {
            List<Brand> brand_list = new List<Brand>()
            {
                new Brand(1,"Brand A",301),
                new Brand(2,"Brand B",302),
                new Brand(3,"Brand C",303),
                new Brand(4,"Brand D",304),
            };
            dgvData.DataSource = brand_list;
        }
    }
}
