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
    public partial class RoleForm : Form
    {
        Role role = new Role();
        public event EventHandler RoleChanged;

        public RoleForm()
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
            rtDes.Text = string.Empty;
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
            role.GetAllRoles();
            dgvData.DataSource = null;
            dgvData.DataSource = Role.RoleList;

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

            // Hide fields
            dgvData.Columns["RoleId"].Visible = false;
            dgvData.Columns["CreatedBy"].Visible = false;
            dgvData.Columns["CreatedAt"].Visible = false;
            dgvData.Columns["UpdatedAt"].Visible = false;
            dgvData.Columns["IsDeleted"].Visible = false;

            dgvData.Refresh();
            RoleChanged?.Invoke(this, EventArgs.Empty);
        }

        // Create new role
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (User.UserLogin == null || User.UserLogin.Count == 0)
            {
                MessageBox.Show("⚠ No user logged in. Please login first.");
                return;
            }

            string roleName = txtName.Text;
            string description = rtDes.Text;
            int createdBy = User.UserLogin[0].UserId;

            if (string.IsNullOrWhiteSpace(roleName))
            {
                MessageBox.Show("⚠ Please enter a role name.");
                return;
            }

            bool result = role.InsertRole(roleName, description, createdBy);
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

            txtId.Text = dgvData.CurrentRow.Cells["RoleId"].Value?.ToString() ?? "";
            txtName.Text = dgvData.CurrentRow.Cells["RoleName"].Value?.ToString() ?? "";
            rtDes.Text = dgvData.CurrentRow.Cells["Description"].Value?.ToString() ?? "";
        }

        // Update selected role
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("⚠ Please select a role to update.");
                return;
            }

            int id = int.Parse(txtId.Text);
            string roleName = txtName.Text;
            string description = rtDes.Text;
            DateTime updatedTime = DateTime.Now;

            bool result = role.UpdateRole(id, roleName, description, updatedTime);
            if (result)
            {
                setField();
                LoadData();
            }
        }

        // Delete selected role
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0 || dgvData.CurrentRow == null) return;

            int id = int.Parse(dgvData.CurrentRow.Cells["RoleId"].Value?.ToString() ?? "0");
            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this role?", "Delete Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (confirm == DialogResult.OK)
            {
                bool result = role.DeleteRole(id);
                if (result)
                {
                    setField();
                    LoadData();
                }
            }
        }
    }
}
