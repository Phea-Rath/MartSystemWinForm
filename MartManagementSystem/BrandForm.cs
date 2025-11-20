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
    public partial class BrandForm : Form
    {
        //User user = new User();
        Brand brand = new Brand();
        public event EventHandler BrandChanged;
        public BrandForm()
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
            brand.GetAllBrand();
            dgvData.DataSource = null;
            dgvData.DataSource = Brand.BrandList;
            if (!dgvData.Columns.Contains("No"))
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = "No";
                col.HeaderText = "#";
                col.Width = 50;
                dgvData.Columns.Insert(0, col); // Insert at first column
            }

            dgvData.Columns["BrandId"].Visible = false;
            dgvData.Columns["CreatedBy"].Visible = false;
            dgvData.Columns["UpdatedAt"].Visible = false;
            dgvData.Columns["IsDeleted"].Visible = false;

            dgvData.Refresh();
            BrandChanged?.Invoke(this, EventArgs.Empty);
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

            bool result = brand.InsertBrand(name, createdBy);
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

            txtId.Text = dgvData.CurrentRow.Cells["BrandId"].Value?.ToString() ?? "";
            txtName.Text = dgvData.CurrentRow.Cells["BrandName"].Value?.ToString() ?? "";
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Equals("")) { MessageBox.Show("⚠ Please select an item to update."); return; }
            int id = int.Parse(txtId.Text);
            string name = txtName.Text;
            DateTime dateTime = DateTime.Now;
            if (name.Equals("")) { MessageBox.Show($"Please select data for update!"); return; }
            bool result = brand.UpdateBrand(id, name, dateTime);
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

            int id = int.Parse(dgvData.CurrentRow.Cells["BrandId"].Value?.ToString() ?? "0");
            DialogResult result = MessageBox.Show("Are you sure You want delete it?", "DeleteItem", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                bool del = brand.DeleteBrand(id);
                if (del)
                {
                    setField();
                    dgvData.DataSource = null;
                    LoadData();
                }
            }
        }
    }
}
