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
        Supplier supplier = new Supplier();
        public event EventHandler SupplierChanged;

        public SupplierForm()
        {
            InitializeComponent();
            dgvData.RowPostPaint += dgvData_RowPostPaint;
            LoadData();
        }

        // Clear input fields
        private void setField()
        {
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtTel.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAddr.Text = string.Empty;
        }

        // Add index column
        private void dgvData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (dgvData.Columns.Contains("No"))
            {
                dgvData.Rows[e.RowIndex].Cells["No"].Value = (e.RowIndex + 1).ToString();
            }
        }

        // Load data into DataGridView
        private void LoadData()
        {
            supplier.GetAllSuppliers();
            dgvData.DataSource = null;
            dgvData.DataSource = Supplier.SupplierList;

            // Add index column if missing
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

            // Hide database-specific fields
            dgvData.Columns["SupplierId"].Visible = false;
            dgvData.Columns["CreatedBy"].Visible = false;
            dgvData.Columns["CreatedAt"].Visible = false;
            dgvData.Columns["UpdatedAt"].Visible = false;
            dgvData.Columns["IsDeleted"].Visible = false;

            dgvData.Refresh();
            SupplierChanged?.Invoke(this, EventArgs.Empty);
        }

        // Create new supplier
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (User.UserLogin == null || User.UserLogin.Count == 0)
            {
                MessageBox.Show("⚠ No user logged in. Please login first.");
                return;
            }

            string name = txtName.Text;
            string phone = txtTel.Text;
            string email = txtEmail.Text;
            string address = txtAddr.Text;
            int createdBy = User.UserLogin[0].UserId;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("⚠ Please enter supplier name.");
                return;
            }

            bool result = supplier.InsertSupplier(name, phone, email, address, createdBy);
            if (result)
            {
                setField();
                LoadData();
            }
        }

        // Load selected row to input fields
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0 || dgvData.CurrentRow == null) return;

            txtId.Text = dgvData.CurrentRow.Cells["SupplierId"].Value?.ToString() ?? "";
            txtName.Text = dgvData.CurrentRow.Cells["SupplierName"].Value?.ToString() ?? "";
            txtTel.Text = dgvData.CurrentRow.Cells["PhoneNumber"].Value?.ToString() ?? "";
            txtEmail.Text = dgvData.CurrentRow.Cells["Email"].Value?.ToString() ?? "";
            txtAddr.Text = dgvData.CurrentRow.Cells["Address"].Value?.ToString() ?? "";
        }

        // Update selected supplier
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("⚠ Please select a supplier to update.");
                return;
            }

            int id = int.Parse(txtId.Text);
            string name = txtName.Text;
            string phone = txtTel.Text;
            string email = txtEmail.Text;
            string address = txtAddr.Text;
            DateTime updatedTime = DateTime.Now;

            bool result = supplier.UpdateSupplier(id, name, phone, email, address, updatedTime);
            if (result)
            {
                setField();
                LoadData();
            }
        }

        // Delete selected supplier
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0 || dgvData.CurrentRow == null) return;

            int id = int.Parse(dgvData.CurrentRow.Cells["SupplierId"].Value?.ToString() ?? "0");
            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this supplier?", "Delete Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (confirm == DialogResult.OK)
            {
                bool result = supplier.DeleteSupplier(id);
                if (result)
                {
                    setField();
                    LoadData();
                }
            }
        }
    }
}
