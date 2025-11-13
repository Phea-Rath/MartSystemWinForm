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
        Supplier _sup = new Supplier();
        public PurchaseForm()
        {
            InitializeComponent();
            LoadData();
            LoadComboBoxse();
            productSelectionForm.RefreshListProduct += LoadList;
        }

        private void LoadComboBoxse()
        {
            _sup.GetAllSuppliers();
            cbSupplier.DataSource = Supplier.SupplierList;
            cbSupplier.DisplayMember = "SupplierName";
            cbSupplier.ValueMember = "SupplierId";
            cbSupplier.SelectedIndex = -1;
        }

        private void dgvData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (dgvData.Columns.Contains("No"))
            {
                dgvData.Rows[e.RowIndex].Cells["No"].Value = (e.RowIndex + 1).ToString();
            }
        }

        private void LoadData()
        {
            dgvData.DataSource = null;
            var purchaseList = Purchase.GetAllPurchases();
            dgvData.DataSource = purchaseList;
            if (!dgvData.Columns.Contains("No"))
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn
                {
                    Name = "No",
                    HeaderText = "#",
                    Width = 50
                };
                dgvData.Columns.Insert(0, col);
            }
            dgvData.Columns["Supplier"].Visible = false;
            dgvData.Columns["PurchaseId"].Visible = false;
            dgvData.Columns["SupplierId"].Visible = false;
            dgvData.Columns["CreatedBy"].Visible = false;
            dgvData.Columns["IsDeleted"].Visible = false;
        }

        private void LoadList(object sender,EventArgs e)
        {
            dgvItem.DataSource = null;
            dgvItem.DataSource = PurchaseDetail.products;
            dgvItem.Columns["ProductId"].DisplayIndex = 0;
            dgvItem.Columns["ProductName"].DisplayIndex = 1;
            dgvItem.Columns["CostPrice"].DisplayIndex = 2;
            dgvItem.Columns["Quantity"].DisplayIndex = 3;
            dgvItem.Columns["SubTotal"].DisplayIndex = 4;
            dgvItem.Columns["ProductId"].Visible = false;
            dgvItem.Columns["CategoryId"].Visible = false;
            dgvItem.Columns["BrandId"].Visible = false;
            dgvItem.Columns["SizeId"].Visible = false;
            dgvItem.Columns["CategoryName"].Visible = false;
            dgvItem.Columns["BrandName"].Visible = false;
            dgvItem.Columns["SizeName"].Visible = false;
            dgvItem.Columns["CreatedBy"].Visible = false;
            dgvItem.Columns["CreatedAt"].Visible = false;
            dgvItem.Columns["UpdatedAt"].Visible = false;
            dgvItem.Columns["UnitPrice"].Visible = false;
            dgvItem.Columns["ImageUrl"].Visible = false;
            dgvItem.Columns["Product"].Visible = false;
            dgvItem.Columns["Purchase"].Visible = false;
            dgvItem.Columns["PurchaseId"].Visible = false;
            dgvItem.Columns["Tax"].Visible = false;
            dgvItem.Columns["IsDeleted"].Visible = false;
            dgvItem.Columns["ProductCode"].Visible = false;
            dgvItem.Columns["Discount"].Visible = false;
            dgvItem.Columns["Description"].Visible = false;

            decimal? subTotal = PurchaseDetail.products.Sum((p)=>p.SubTotal);
            txtSupTotal.Text = subTotal.ToString();
            txtAmount.Text = subTotal.ToString();
            txtBalance.Text = subTotal.ToString();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            productSelectionForm.ShowDialog();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if(dgvItem.Rows.Count == 0) return;
            if (dgvItem.CurrentRow == null) return;

            int _id = Convert.ToInt32(dgvItem.CurrentRow.Cells["ProductId"].Value);
            PurchaseDetail.products = PurchaseDetail.products.Where((p)=>p.ProductId != _id).ToList();
            LoadList(sender,e);
        }

        //private void txtDis_TextChanged(object sender, EventArgs e) => CalculateAmount();
        private void txtFee_TextChanged(object sender, EventArgs e) => CalculateAmount();
        private void txtTax_TextChanged(object sender, EventArgs e) => CalculateAmount();
        private void txtPayment_TextChanged(object sender, EventArgs e) => CalculateAmount();


        private void CalculateAmount()
        { 
            decimal subTotal = ToDecimal(txtSupTotal.Text);
            decimal taxPercent = ToDecimal(txtTax.Text) / 100;
            decimal fee = ToDecimal(txtFee.Text);

            decimal taxValue = subTotal * taxPercent;

            decimal totalAmount = subTotal - fee + taxValue;

            txtAmount.Text = totalAmount.ToString("0.##");
            decimal payment = ToDecimal(txtPayment.Text);
            decimal balance = totalAmount - payment;
            txtBalance.Text = balance.ToString("0.##");
        }

        private decimal ToDecimal(string text)
        {
            if (decimal.TryParse(text, out decimal value))
                return value;
            return 0;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if(PurchaseDetail.ProductList.Count <= 0)
            {
                MessageBox.Show("Please select product!");
                return;
            }
            int? supplier_id = (int)cbSupplier.SelectedValue as int?;
            decimal subTotal = Convert.ToDecimal(txtSupTotal.Text);
            decimal tax = Convert.ToDecimal(txtTax.Text);  
            decimal fee = Convert.ToDecimal(txtFee.Text);
            decimal totalAmount = Convert.ToDecimal(txtAmount.Text);
            decimal payment = Convert.ToDecimal(txtPayment.Text);
            decimal balance = Convert.ToDecimal(txtBalance.Text);
            string description = rtDes.Text; 
            string status = cbStatus.Text;
            if(supplier_id <= 0)
            {
                MessageBox.Show("Please select supplier!");
                return;
            }

            Purchase _purchase = new Purchase()
            {
                SupplierId = (int)supplier_id,
                Price = subTotal,
                Tax = tax,
                Total = totalAmount,
                DeliveryFee = fee,
                Payment = payment,
                Balance = balance,
                Description = description,
                Status = status,
                
            };
            bool res = Purchase.InsertPurchase(_purchase, PurchaseDetail.products);
            if(res)
            {
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0) return;
            if (dgvData.CurrentRow == null) return;
            string id = dgvData.CurrentRow.Cells["PurchaseId"].Value.ToString();
            txtId.Text = id;
            cbSupplier.SelectedValue = dgvData.CurrentRow.Cells["SupplierId"].Value;
            cbStatus.Text = dgvData.CurrentRow.Cells["Status"].Value.ToString();
            txtSupTotal.Text = dgvData.CurrentRow.Cells["Price"].Value.ToString();
            txtFee.Text = dgvData.CurrentRow.Cells["DeliveryFee"].Value.ToString();
            txtAmount.Text = dgvData.CurrentRow.Cells["Total"].Value.ToString();
            txtBalance.Text = dgvData.CurrentRow.Cells["Balance"].Value.ToString();
            txtPayment.Text = dgvData.CurrentRow.Cells["Payment"].Value.ToString();
            rtDes.Text = dgvData.CurrentRow.Cells["Description"].Value.ToString();
            var purdetails = PurchaseDetail.GetPurchaseDetailsByPurchaseId(Convert.ToInt32(id));
            dgvItem.DataSource = null;
            dgvItem.DataSource = purdetails;
            dgvItem.Columns["ProductId"].DisplayIndex = 0;
            dgvItem.Columns["ProductName"].DisplayIndex = 1;
            dgvItem.Columns["CostPrice"].DisplayIndex = 2;
            dgvItem.Columns["Quantity"].DisplayIndex = 3;
            dgvItem.Columns["SubTotal"].DisplayIndex = 4;
            dgvItem.Columns["ProductId"].Visible = false;
            dgvItem.Columns["CategoryId"].Visible = false;
            dgvItem.Columns["BrandId"].Visible = false;
            dgvItem.Columns["SizeId"].Visible = false;
            dgvItem.Columns["CategoryName"].Visible = false;
            dgvItem.Columns["BrandName"].Visible = false;
            dgvItem.Columns["SizeName"].Visible = false;
            dgvItem.Columns["CreatedBy"].Visible = false;
            dgvItem.Columns["CreatedAt"].Visible = false;
            dgvItem.Columns["UpdatedAt"].Visible = false;
            dgvItem.Columns["UnitPrice"].Visible = false;
            dgvItem.Columns["ImageUrl"].Visible = false;
            dgvItem.Columns["Product"].Visible = false;
            dgvItem.Columns["Purchase"].Visible = false;
            dgvItem.Columns["IsDeleted"].Visible = false;
            dgvItem.Columns["ProductCode"].Visible = false;
            dgvItem.Columns["Discount"].Visible = false;
            dgvItem.Columns["Description"].Visible = false;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (PurchaseDetail.ProductList.Count <= 0)
            {
                MessageBox.Show("Please select product!");
                return;
            }
            int id = Convert.ToInt32(txtId.Text);
            int? supplier_id = (int)cbSupplier.SelectedValue as int?;
            decimal subTotal = Convert.ToDecimal(txtSupTotal.Text);
            decimal tax = Convert.ToDecimal(txtTax.Text);
            decimal fee = Convert.ToDecimal(txtFee.Text);
            decimal totalAmount = Convert.ToDecimal(txtAmount.Text);
            decimal payment = Convert.ToDecimal(txtPayment.Text);
            decimal balance = Convert.ToDecimal(txtBalance.Text);
            string description = rtDes.Text;
            string status = cbStatus.Text;
            if (supplier_id <= 0)
            {
                MessageBox.Show("Please select supplier!");
                return;
            }

            Purchase _purchase = new Purchase()
            {
                PurchaseId = id,
                SupplierId = (int)supplier_id,
                Price = subTotal,
                Tax = tax,
                Total = totalAmount,
                DeliveryFee = fee,
                Payment = payment,
                Balance = balance,
                Description = description,
                Status = status,

            };
            bool res = Purchase.UpdatePurchase(_purchase, PurchaseDetail.products);
            if (res)
            {
                LoadData();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if(dgvData.Rows.Count <= 0) return;
            if (dgvData.CurrentRow == null) return;
            string id = dgvData.CurrentRow.Cells["PurchaseId"].Value.ToString();
            if (string.IsNullOrEmpty(id)) return;
            DialogResult result = MessageBox.Show("Are you sure You want delete it?", "DeleteItem", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                bool res = Purchase.DeletePurchase(Convert.ToInt32(id));
                if (res)LoadData();
            }
        }
    }
}
