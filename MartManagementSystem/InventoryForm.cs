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
    
    public partial class InventoryForm : Form
    {
        PurchaseForm _pur;
        public event EventHandler InventoriesChanged;
        public InventoryForm(PurchaseForm pur)
        {
            InitializeComponent();
            _pur = pur;
            LoadData();
            cbProduct.DataSource = Product.ProductList;
            cbProduct.DisplayMember = "ProductName";
            cbProduct.ValueMember = "ProductId";
            cbProduct.SelectedIndex = -1;
            _pur.PurchaseChanged += (s, e) => LoadData();
        }

        private void setForm()
        {
            cbProduct.SelectedIndex = -1;
            txtId.Clear();
            txtQuan.Clear();
            rtDes.Clear();
            dgvItem.DataSource = null;
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
            Inventory.GetAllInventories();
            dgvData.DataSource = null;
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
            dgvData.DataSource = Inventory.inventories;
            dgvData.Columns["IsDeleted"].Visible = false;
            dgvData.Columns["CreatedBy"].Visible = false;
            dgvData.Columns["InventoryId"].Visible = false;

            dgvData.Refresh();
            InventoriesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void LoadItemList()
        {
            dgvItem.DataSource = null;
            dgvItem.DataSource = InventoryDetail.InventoryDetailList;

            dgvItem.Columns["ProductId"].DisplayIndex = 0;
            dgvItem.Columns["ProductName"].DisplayIndex = 1;
            dgvItem.Columns["ExpireDate"].DisplayIndex = 2;

            dgvItem.Columns["ProductId"].Visible = false;
            dgvItem.Columns["InventoryId"].Visible = false;
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
            dgvItem.Columns["Discount"].Visible = false;
            dgvItem.Columns["ImageUrl"].Visible = false;
            dgvItem.Columns["ProductCode"].Visible = false;
            dgvItem.Columns["Description"].Visible = false;
            dgvItem.Columns["CostPrice"].Visible = false;
            dgvItem.Columns["IsDeleted"].Visible = false;
        }
        

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQuan.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = (int)cbProduct.SelectedValue;
            string productName = Product.ProductList.FirstOrDefault((p) => p.ProductId == productId)?.ProductName;
            DateTime expiryDate = dtpExpire.Value;
            InventoryDetail.InventoryDetailList.Add(new InventoryDetail()
            {
                ProductName = productName,
                ProductId = productId,
                Quantity = quantity,
                ExpireDate = expiryDate,
            });
            LoadItemList();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if(dgvItem.Rows.Count <= 0) return;
            if(dgvItem.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvItem.CurrentRow.Cells["ProductId"].Value);
            InventoryDetail.InventoryDetailList = InventoryDetail.InventoryDetailList.Where((p)=>p.ProductId != id).ToList();
            LoadItemList();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if(InventoryDetail.InventoryDetailList.Count <= 0)return;
            Inventory inven = new Inventory()
            {
                Description = rtDes.Text,
                CreatedBy = User.UserLogin[0].UserId
            };
            bool res = Inventory.InsertInventory(inven, InventoryDetail.InventoryDetailList);

            if (res) { LoadData(); setForm(); }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(dgvData.Rows.Count <= 0) return;
            if(dgvData.CurrentRow == null)return;
            txtId.Text = dgvData.CurrentRow.Cells["InventoryId"].Value.ToString();
            rtDes.Text = dgvData.CurrentRow.Cells["Description"].Value.ToString();
            dgvItem.DataSource = null;
            InventoryDetail.GetInvetoryDetail(Convert.ToInt32(txtId.Text));
            LoadItemList();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (InventoryDetail.InventoryDetailList.Count <= 0) return;
            Inventory inven = new Inventory()
            {
                InventoryId = Convert.ToInt32(txtId.Text),
                Description = rtDes.Text,
            };
            bool res = Inventory.UpdateInventory(inven, InventoryDetail.InventoryDetailList);

            if (res) { LoadData(); setForm(); }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0) return;
            if (dgvData.CurrentRow == null) return;

            int id = int.Parse(dgvData.CurrentRow.Cells["InventoryId"].Value?.ToString() ?? "0");
            DialogResult result = MessageBox.Show("Are you sure You want delete it?", "DeleteItem", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                bool res = Inventory.DeleteInventory(id);
                if (res) { LoadData(); setForm(); }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string find = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(find))
            {
                // Show all
                dgvData.DataSource = Product.ProductList;
            }
            else
            {
                // Filter list (case-insensitive, partial match)
                var newList = Inventory.inventories
                    .Where(p => !string.IsNullOrEmpty(p.Description) &&
                                p.Description.IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                dgvData.DataSource = newList;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            setForm();
        }
    }
}
