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
    public partial class CategoryForm : Form
    {
        Category category = new Category();
        public event EventHandler CategoryChanged;
        public CategoryForm()
        {
            InitializeComponent();
            dgvData.RowPostPaint += dgvData_RowPostPaint; // Ensure event is wired
            LoadData();
        }

        public void setField()
        {
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
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
            category.GetAllCategory();
            dgvData.DataSource = null; // Reset DataSource to refresh
            dgvData.DataSource = Category.CategoryList;

            // Add index column if it doesn't exist
            if (!dgvData.Columns.Contains("No"))
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = "No";
                col.HeaderText = "#";
                col.Width = 50;
                dgvData.Columns.Insert(0, col);
            }

            // Hide fields
            dgvData.Columns["CategoryId"].Visible = false;
            dgvData.Columns["CreatedBy"].Visible = false;
            dgvData.Columns["UpdatedAt"].Visible = false;
            dgvData.Columns["IsDeleted"].Visible = false;

            dgvData.Refresh(); // Force repaint to update index column
            CategoryChanged?.Invoke(this, EventArgs.Empty);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (User.UserLogin == null || User.UserLogin.Count == 0)
            {
                MessageBox.Show("⚠ No user logged in. Please login first.");
                return;
            }

            string name = txtName.Text;
            int createdBy = User.UserLogin[0].UserId;

            bool result = category.InsertCategory(name, createdBy);
            if (result)
            {
                setField();
                LoadData(); // reload data to refresh index
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0 || dgvData.CurrentRow == null) return;

            txtId.Text = dgvData.CurrentRow.Cells["CategoryId"].Value?.ToString() ?? "";
            txtName.Text = dgvData.CurrentRow.Cells["CategoryName"].Value?.ToString() ?? "";
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("⚠ Please select an item to update.");
                return;
            }

            int id = int.Parse(txtId.Text);
            string name = txtName.Text;
            DateTime updatedTime = DateTime.Now;

            bool result = category.UpdateCategory(id, name, updatedTime);
            if (result)
            {
                setField();
                LoadData(); // reload data to refresh index
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0 || dgvData.CurrentRow == null) return;

            int id = int.Parse(dgvData.CurrentRow.Cells["CategoryId"].Value?.ToString() ?? "0");

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this?", "Delete Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (confirm == DialogResult.OK)
            {
                bool result = category.DeleteCategory(id);
                if (result)
                {
                    setField();
                    LoadData(); // reload data to refresh index
                }
            }
        }
    }


}
