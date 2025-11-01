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
    public partial class PurchaseForm : Form
    {
        ProductSelectionForm productSelectionForm = new ProductSelectionForm();
        public PurchaseForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            List<Purchase> purchases = new List<Purchase>
            {
                new Purchase(
                    1,
                    1,
                    DateTime.Now.AddDays(-10),
                    1000.00m,
                     5,
                    10,
                    950.00m,
                    1,
                     "First purchase",
                    500.00m,
                     450.00m,
                    0.00m
                ),
                new Purchase(
                    2,
                    2,
                    DateTime.Now.AddDays(-5),
                    2000.00m,
                     10,
                    20,
                    1890.00m,
                    2,
                     "Second purchase",
                    1000.00m,
                     890.00m,
                    0.00m
                )
            };
            dgvData.DataSource = purchases;
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            productSelectionForm.ShowDialog();
        }
    }
}
