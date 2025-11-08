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
        public InventoryForm()
        {
            InitializeComponent();
            LoadData();
            cbProduct.DataSource = Product.ProductList;
            cbProduct.DisplayMember = "ProductName";
            cbProduct.ValueMember = "ProductId";
            cbProduct.SelectedIndex = -1;
        }

        private void setForm()
        {
            cbProduct.SelectedIndex = -1;
            txtId.Clear();
            txtQuan.Clear();
            rtDes.Clear();
        }

        private void LoadData()
        {
            Inventory.GetAllInventories();
            dgvData.DataSource = null;
            dgvData.DataSource = Inventory.inventories;
            dgvData.Columns["IsDeleted"].Visible = false;
            dgvData.Columns["CreatedBy"].Visible = false;
            dgvData.Columns["InventoryId"].Visible = false;
        }

        private void LoadItemList()
        {
            dgvItem.DataSource = null;
            dgvItem.DataSource = InventoryDetail.InventoryDetailList;
            dgvItem.Columns["ProductId"].Visible = false;
            dgvItem.Columns["InventoryId"].Visible = false;
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
    }
}
