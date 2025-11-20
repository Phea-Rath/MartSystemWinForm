using MartManagementSystem.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace MartManagementSystem
{
    public partial class UserForm : Form
    {
        Role role = new Role();
        User user = new User();
        ComponentResourceManager resources = new ComponentResourceManager(typeof(UserForm));
        RoleForm _role;
        public UserForm(RoleForm role)
        {
            InitializeComponent();
            dgvData.RowPostPaint += dgvData_RowPostPaint;
            LoadData();
            LoadRole();
            _role = role;
            _role.RoleChanged += (s, e) => LoadRole();
        }

        private void ClearField()
        {
            txtId.Text = "";
            txtName.Text = "";
            txtTel.Text = "";
            txtEmail.Text = "";
            txtPass.Text = "";
            txtImageUrl.Text = "";
            cbRole.SelectedIndex = -1;
            pbImage.Image = (Image)resources.GetObject("pbImage.Image");
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
            user.GetAllUsers();
            dgvData.DataSource = null;
            dgvData.DataSource = User.UserList;

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

            dgvData.Columns["UserId"].Visible = false;
            dgvData.Columns["RoleId"].Visible = false;
            dgvData.Columns["CreatedBy"].Visible = false;
            dgvData.Columns["Password"].Visible = false;
            dgvData.Columns["IsDeleted"].Visible = false;
            dgvData.Columns["LoginAt"].Visible = false;

            dgvData.Refresh();
        }

        private void LoadRole()
        {
            role.GetAllRoles();
            cbRole.DataSource = Role.RoleList;
            cbRole.DisplayMember = "RoleName";
            cbRole.ValueMember = "RoleId";
            cbRole.SelectedIndex = -1;
        }

        private string SaveImageToAssets(string originalPath)
        {
            if (string.IsNullOrEmpty(originalPath) || !File.Exists(originalPath))
                return null;

            string folder = Path.Combine(Application.StartupPath, "assets");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(originalPath);
            string destPath = Path.Combine(folder, fileName);

            File.Copy(originalPath, destPath, true);

            return fileName;
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pbImage.Image = Image.FromFile(ofd.FileName);
                    pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                    txtImageUrl.Text = ofd.FileName;
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            string conPass = txtConPass.Text;
            string Pass = txtPass.Text;
            if (conPass != Pass)
            {
                MessageBox.Show("Confirm Password is not matching!", "Error");
                return;
            }
            if (User.UserLogin == null || User.UserLogin.Count == 0)
            {
                MessageBox.Show("⚠ Please login first.", "Warning");
                return;
            }


            User u = new User
            {
                UserName = txtName.Text,
                PhoneNumber = txtTel.Text,
                Email = txtEmail.Text,
                Password = txtPass.Text,
                RoleId = (int?)cbRole.SelectedValue,
                CreatedBy = User.UserLogin[0].UserId,
                ImageUrl = SaveImageToAssets(txtImageUrl.Text)
            };

           int userId = user.InsertUser(u);

            if (userId > 0)
            {
                Permission.InsertPermission(userId);
                ClearField();
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0 || dgvData.CurrentRow == null) return;

            txtId.Text = dgvData.CurrentRow.Cells["UserId"].Value.ToString();
            txtName.Text = dgvData.CurrentRow.Cells["UserName"].Value.ToString();
            txtTel.Text = Convert.ToString(dgvData.CurrentRow.Cells["PhoneNumber"].Value);
            txtEmail.Text = dgvData.CurrentRow.Cells["Email"].Value.ToString();
            cbRole.SelectedValue = dgvData.CurrentRow.Cells["RoleId"].Value;

            string folder = Path.Combine(Application.StartupPath, "assets");
            string img =  Convert.ToString(dgvData.CurrentRow.Cells["ImageUrl"].Value);
            string imgPath = Path.Combine(folder, img);

            if (File.Exists(imgPath))
            {
                pbImage.Image = Image.FromFile(imgPath);
                pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                txtImageUrl.Text = imgPath;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Equals(""))
            {
                MessageBox.Show("⚠ Please select an item to update."); return;
            }

            string conPass = txtConPass.Text;
            string Pass = txtPass.Text;
            if (conPass != Pass)
            {
                MessageBox.Show("Confirm Password is not matching!","Error");
                return;
            }
            User u = new User
            {
                UserId = int.Parse(txtId.Text),
                UserName = txtName.Text,
                PhoneNumber = txtTel.Text,
                Email = txtEmail.Text,
                Password = txtPass.Text,
                RoleId = (int?)cbRole.SelectedValue,
                ImageUrl = SaveImageToAssets(txtImageUrl.Text)
            };

            bool result = user.UpdateUser(u);
            if (result)
            {
                ClearField();
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0 || dgvData.CurrentRow == null) return;

            int id = (int)dgvData.CurrentRow.Cells["UserId"].Value;
            DialogResult confirm = MessageBox.Show("Delete this user?", "Confirm Delete", MessageBoxButtons.OKCancel);

            if (confirm == DialogResult.OK)
            {
                bool del = user.DeleteUser(id);
                if (del)
                {
                    ClearField();
                    LoadData();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string find = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(find))
            {
                // Show all
                dgvData.DataSource = User.UserList;
            }
            else
            {
                // Filter list (case-insensitive, partial match)
                var newList = User.UserList
                    .Where(p => !string.IsNullOrEmpty(p.UserName) &&
                                p.UserName.IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                dgvData.DataSource = newList;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearField();
        }

    }
}
