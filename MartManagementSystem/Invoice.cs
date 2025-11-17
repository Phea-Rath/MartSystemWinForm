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
    public partial class Invoice : Form
    {
        Order order;
        public Invoice(OrderList _orderList,Order _order)
        {
            InitializeComponent();
            order = _order;
            LoadData();
        }

        private TableLayoutPanel CreateListItems(OrderDetail p)
        {
            
            // 
            // tlpList
            // 
            TableLayoutPanel tlpList = new TableLayoutPanel();
            tlpList.ColumnCount = 2;
            tlpList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tlpList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tlpList.Location = new System.Drawing.Point(8, 8);
            tlpList.Name = "tlpList";
            tlpList.RowCount = 2;
            tlpList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            tlpList.Size = new System.Drawing.Size(352, 41);
            tlpList.TabIndex = 3;
            // 
            // lblPrice
            // 
            Label lblPrice = new Label();
            lblPrice.AutoSize = true;
            lblPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            lblPrice.Location = new System.Drawing.Point(249, 0);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new System.Drawing.Size(100, 28);
            lblPrice.TabIndex = 0;
            lblPrice.Text = "$"+p.UnitPrice.ToString("N2");
            lblPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            tlpList.Controls.Add(lblPrice, 1, 0);
            // 
            // lblTitle
            // 
            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblTitle.Location = new System.Drawing.Point(3, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(240, 28);
            lblTitle.TabIndex = 0;
            lblTitle.Text = p.ProductName;
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            tlpList.Controls.Add(lblTitle, 0, 0);
            // lblQuan
            // 
            Label lblQuan = new Label();
            lblQuan.AutoSize = true;
            lblQuan.Location = new System.Drawing.Point(3, 28);
            lblQuan.Name = "lblQuan";
            lblQuan.Size = new System.Drawing.Size(24, 13);
            lblQuan.TabIndex = 1;
            lblQuan.Text = "x"+p.Quantity.ToString();
            tlpList.Controls.Add(lblQuan, 0, 1);

            return tlpList;
        }

        private void LoadData()
        {
            var orderItem = OrderDetail.GetOrderDetails(order.OrderId);
            LoadListItem(orderItem);
            lblOrderId.Text = "OrderID: #"+order.OrderId.ToString();
            lblSupPrice.Text = "$" + order.SubTotalPrice.ToString();
            lblDisPrice.Text = "$" + order.Discount.ToString();
            lblPmValue.Text = order.PaymentMethod.ToString();
            lblTotalPrice.Text = "$" + order.TotalPrice.ToString();
        }

        private void LoadListItem(List<OrderDetail> od)
        {
            flpItem.Controls.Clear();
            foreach (OrderDetail Item in od)
            {
                TableLayoutPanel tlp = CreateListItems(Item);
                flpItem.Controls.Add(tlp);
            }
        }
    }
}
