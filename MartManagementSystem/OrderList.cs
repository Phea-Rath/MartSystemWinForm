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

    public partial class OrderList : Form
    {
        public event EventHandler handleOrder;
        public OrderList()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Order.GetAllOrders();
            dgvData.DataSource = null;
            dgvData.DataSource = Order.orders;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            handleOrder?.Invoke(this, EventArgs.Empty);
        }
    }
}
