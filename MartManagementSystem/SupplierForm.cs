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
    public partial class SupplierForm : Form
    {
        public SupplierForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            List<Supplier> suppliers = new List<Supplier>
            {
                new Supplier(1, "Supplier A", "123-456-7890", "123 Main St", "supplier1@gmail.com",1),
                new Supplier(2, "Supplier B", "987-654-3210", "456 Elm St", "supplier2@gmail.com",1)
            };
            dgvData.DataSource = suppliers;
        }
    }
}
