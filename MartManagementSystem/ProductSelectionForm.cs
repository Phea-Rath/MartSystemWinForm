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
    public partial class ProductSelectionForm : Form
    {
        public event EventHandler RefreshListProduct;
        Product _pro = new Product();
        public ProductSelectionForm()
        {
            InitializeComponent();
            LoadComboboxes();
        }

        private void setField()
        {
            txtCost.Clear();
            txtQuantity.Clear();
            txtUnitPrice.Clear();
            cbProduct.SelectedIndex = -1;
        }

        private void LoadComboboxes()
        {
            _pro.GetAllProduct();
            cbProduct.DataSource = Product.ProductList;
            cbProduct.DisplayMember = "ProductName";
            cbProduct.ValueMember = "ProductId";
            cbProduct.SelectedIndex = -1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            int? id = cbProduct.SelectedValue as int?;
            if (id == null) return;

            var pro = Product.ProductList.FirstOrDefault(p => p.ProductId == id.Value);
            if (pro == null) return;

            txtUnitPrice.Text = pro.UnitPrice.ToString("N2");
            txtQuantity.Text = "1";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbProduct.SelectedValue == null || txtQuantity.Text.Equals("")|| txtQuantity.Text.Equals(0) || txtCost.Text.Equals(""))
            {
                MessageBox.Show("Please input on field!");
                return;
            }

            int? id = cbProduct.SelectedValue as int?;

            var pro = Product.ProductList.FirstOrDefault(p => p.ProductId == id.Value);
            decimal? unit_price = Convert.ToDecimal(txtUnitPrice.Text);
            int? quantity = Convert.ToInt32(txtQuantity.Text);
            decimal? cost = Convert.ToDecimal(txtCost.Text);

            PurchaseDetail.products.Add(new PurchaseDetail()
            {
                ProductId = (int)id,
                ProductName = pro.ProductName,
                UnitPrice = (decimal)unit_price,
                Quantity = (int)quantity,
                CostPrice = (decimal)cost,
                SubTotal = (decimal)cost * (int)quantity
            });
            RefreshListProduct.Invoke(this, new EventArgs());
            setField();
        }
    }
}
