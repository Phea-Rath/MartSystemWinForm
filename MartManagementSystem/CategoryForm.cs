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
    public partial class CategoryForm : Form
    {
        public CategoryForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            List<Category> categories = new List<Category>
            {
                new Category (1, "Beverages",101 ),
                new Category (2, "Snacks",102 ),
                new Category (3, "Dairy",103 ),
                new Category (4, "Bakery",104 ),
            };

            dgvData.DataSource = categories;    

        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
