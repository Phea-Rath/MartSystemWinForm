using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    public partial class PermissionForm : Form
    {
        User _user = new User();
        Menu _menu = new Menu();
        public PermissionForm()
        {
            InitializeComponent();
            LoadData();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            _user.GetAllUsers();
            var users = User.UserList.Where((u) => u.UserId != User.UserLogin[0].UserId).ToList();
            cbUser.DataSource = users;
            cbUser.DisplayMember = "UserName";
            cbUser.ValueMember = "UserId";
            cbUser.SelectedIndex = -1;
        }

        private void LoadData()
        {
            _menu.GetAllMenus();
        }

        private int GetSelectedUserId()
        {
            if (cbUser.SelectedValue == null || cbUser.SelectedIndex == -1 || $"{cbUser.SelectedValue}" == "MartManagementSystem.User")
            {
                //MessageBox.Show("Please select a user first!", "Information");
                return 0;
            }
            return Convert.ToInt32(cbUser.SelectedValue);
        }


        // ================= PERMISSION CHECKBOX EVENTS =================

        private void cbPro_CheckedChanged(object sender, EventArgs e)
        {
            int userId = GetSelectedUserId();
            if (userId == 0) return;

            int[] arrMenuId = { 2, 3, 4, 5 };
            bool res = Permission.ChangePermission(userId, arrMenuId, cbPro.Checked);
            if (res) Services.ShowAlert("Product permission updated!");
        }

        private void cbPur_CheckedChanged(object sender, EventArgs e)
        {
            int userId = GetSelectedUserId();
            if (userId == 0) return;

            int[] arrMenuId = { 7 };
            bool res = Permission.ChangePermission(userId, arrMenuId, cbPur.Checked);
            if (res) Services.ShowAlert("Purchase permission updated!");
        }

        private void cbOrd_CheckedChanged(object sender, EventArgs e)
        {
            int userId = GetSelectedUserId();
            if (userId == 0) return;

            int[] arrMenuId = { 8 };
            bool res = Permission.ChangePermission(userId, arrMenuId, cbOrd.Checked);
            if (res) Services.ShowAlert("Order permission updated!");
        }

        private void cbSett_CheckedChanged(object sender, EventArgs e)
        {
            int userId = GetSelectedUserId();
            if (userId == 0) return;

            int[] arrMenuId = { 9, 10, 11, 12 };
            bool res = Permission.ChangePermission(userId, arrMenuId, cbSett.Checked);
            if (res) Services.ShowAlert("Settings permission updated!");
        }

        private void cbDas_CheckedChanged(object sender, EventArgs e)
        {
            int userId = GetSelectedUserId();
            if (userId == 0) return;

            int[] arrMenuId = { 1 };
            bool res = Permission.ChangePermission(userId, arrMenuId, cbDas.Checked);
            if (res) Services.ShowAlert("Dashboard permission updated!");
        }

        private void cbInv_CheckedChanged(object sender, EventArgs e)
        {
            int userId = GetSelectedUserId();
            if (userId == 0) return;

            int[] arrMenuId = { 6 };
            bool res = Permission.ChangePermission(userId, arrMenuId, cbInv.Checked);
            if (res) Services.ShowAlert("Inventory permission updated!");
        }


        // ================= LOAD PERMISSION FOR SELECTED USER =================

        private void LoadPermission()
        {
            cbActive.Checked = _user.IsActive;
            // Map menu IDs to checkboxes
            Dictionary<int, CheckBox> map = new Dictionary<int, CheckBox>()
        {
            {1, cbDas},
            {2, cbPro},
            {6, cbInv},
            {7, cbPur},
            {8, cbOrd},
            {9, cbSett},
        };

            foreach (var p in Permission.permissions)
            {
                if (map.ContainsKey(p.MenuId))
                {
                    map[p.MenuId].Checked = p.IsActive;
                    map[p.MenuId].Text = p.MenuName;
                }
            }
        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbUser.SelectedIndex == -1) return;

            int userId = GetSelectedUserId();
            if (userId == 0) return;
            
            _user = User.GetUserById(userId);
            Permission.GetPermissionUser(userId);
            LoadPermission();
        }

        private void cbActive_CheckedChanged(object sender, EventArgs e)
        {
            //cbActive.Checked = !cbActive.Checked;
            int userId = GetSelectedUserId();
            bool res = User.DisableUser(userId, cbActive.Checked);
            if (res)
            {
                Permission.GetPermissionUser(userId);
                //LoadPermission();
            }
        }
    }



}
