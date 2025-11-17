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
        OrderForm _order;
        public OrderList(OrderForm order)
        {
            InitializeComponent();
            _order = order;
            _order.handleOrderList += OrderList_Load;
        }

        private void OrderList_Load(object sender,EventArgs e)
        {
            Order.GetAllOrders();
            dgvData.DataSource = null;
            dgvData.DataSource = Order.orders;
            dgvData.Columns["CreatedBy"].Visible = false;
            dgvData.Columns["UpdatedAt"].Visible = false;
            dgvData.Columns["IsDeleted"].Visible = false;
        }

        private void btnOrd_Click(object sender, EventArgs e)
        {
            handleOrder?.Invoke(this, e);
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            if(dgvData.Rows.Count <= 0) return;
            if (dgvData.CurrentRow == null) return;

            Order order = new Order()
            {
                OrderId = Convert.ToInt32(dgvData.CurrentRow.Cells["OrderId"].Value),
                SubTotalPrice = Convert.ToDecimal(dgvData.CurrentRow.Cells["SubTotalPrice"].Value),
                Discount = Convert.ToDecimal(dgvData.CurrentRow.Cells["Discount"].Value),
                PaymentMethod = dgvData.CurrentRow.Cells["PaymentMethod"].Value.ToString(),
                TotalPrice = Convert.ToDecimal(dgvData.CurrentRow.Cells["TotalPrice"].Value)
            };
            Invoice invoiceForm = new Invoice(this, order);
            invoiceForm.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string find = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(find))
            {
                // Show all
                dgvData.DataSource = Order.orders;
            }
            else
            {
                // Filter list (case-insensitive, partial match)
                var newList = Order.orders
                    .Where(p => !string.IsNullOrEmpty(p.OrderId.ToString()) &&
                                p.OrderId.ToString().IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                dgvData.DataSource = newList;
            }
        }

    }
}
