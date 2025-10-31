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
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            List<Product> product_list = new List<Product>()
            {
                new Product(1, "P001", "Product A", 1, 1, 101, 10.0, 0.0, 7.0, 1.5, "Description A", 1),
                new Product(2, "P002", "Product B", 2, 2, 102, 15.0, 1.0, 10.0, 2.0, "Description B", 2),
                new Product(3, "P003", "Product C", 3, 3, 103, 20.0, 2.0, 15.0, 2.5, "Description C", 3),
            };
            dgvData.DataSource = product_list;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
