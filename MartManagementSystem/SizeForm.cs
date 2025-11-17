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
    public partial class SizeForm : Form
    {
        Sizes size = new Sizes();
        public event EventHandler SizeChanged;
        public SizeForm()
        {
            InitializeComponent();
            dgvData.RowPostPaint += dgvData_RowPostPaint;
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

            size.GetAllSizes();
            dgvData.DataSource = null; // Reset DataSource to refresh
            dgvData.DataSource = Sizes.SizeList;

            // Add index column only once
            if (!dgvData.Columns.Contains("No"))
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = "No";
                col.HeaderText = "#";
                col.Width = 50;
                dgvData.Columns.Insert(0, col);
            }

            // Hide unwanted columns
            dgvData.Columns["SizeId"].Visible = false;
            dgvData.Columns["CreatedBy"].Visible = false;
            dgvData.Columns["UpdatedAt"].Visible = false;
            dgvData.Columns["IsDeleted"].Visible = false;

            dgvData.Refresh();

            SizeChanged?.Invoke(this, EventArgs.Empty);
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

            bool result = size.InsertSize(name, createdBy);
            if (result)
            {
                setField();
                dgvData.DataSource = null;
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0) return;
            if (dgvData.CurrentRow == null) return;

            txtId.Text = dgvData.CurrentRow.Cells["SizeId"].Value?.ToString() ?? "";
            txtName.Text = dgvData.CurrentRow.Cells["SizeName"].Value?.ToString() ?? "";
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("⚠ Please select an item to update.");
                return;
            }

            int id = int.Parse(txtId.Text);
            string name = txtName.Text;
            DateTime updatedTime = DateTime.Now;

            bool result = size.UpdateSize(id, name, updatedTime);
            if (result)
            {
                setField();
                dgvData.DataSource = null;
                LoadData();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0) return;
            if (dgvData.CurrentRow == null) return;

            int id = int.Parse(dgvData.CurrentRow.Cells["SizeId"].Value?.ToString() ?? "0");

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this?", "Delete Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (confirm == DialogResult.OK)
            {
                bool result = size.DeleteSize(id);
                if (result)
                {
                    setField();
                    dgvData.DataSource = null;
                    LoadData();
                }
            }
        }
    }

}
