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
    public partial class PermissionForm : Form
    {
        User _user = new User();
        Menu _menu = new Menu();

        // Collect all permission checkboxes
        private List<CheckBox> cbList = new List<CheckBox>();

        // Parent → Children relation map
        private Dictionary<CheckBox, List<CheckBox>> permissionMap;

        public PermissionForm()
        {
            InitializeComponent();
            BuildCheckBoxList();
            BuildPermissionRelations();
            LoadData();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            _user.GetAllUsers();
            cbUser.DataSource = User.UserList;
            cbUser.DisplayMember = "UserName";
            cbUser.ValueMember = "UserId";
            cbUser.SelectedIndex = -1;
        }

        private void LoadData()
        {
            _menu.GetAllMenus();
        }

        // ✅ Collect all permission CheckBoxes
        private void BuildCheckBoxList()
        {
            cbList.AddRange(new[]
            {
            cbAcc, cbBrand, cbCat, cbDas, cbInv, cbOrd, cbOrdl, cbPro, cbPur,
            cbSize, cbSup, cbRole, cbSett, cbPer
        });

            foreach (var cb in cbList)
            {
                cb.CheckedChanged += PermissionCheckBox_CheckedChanged;
            }
        }

        // ✅ Define Parent → Child checkbox logic (Clean and Maintainable)
        private void BuildPermissionRelations()
        {
            permissionMap = new Dictionary<CheckBox, List<CheckBox>>()
        {
            { cbPro, new List<CheckBox> { cbBrand, cbSize, cbCat } },
            { cbOrd, new List<CheckBox> { cbOrdl } },
            { cbPur, new List<CheckBox> { cbSup } },
            { cbSett, new List<CheckBox> { cbAcc, cbRole, cbPer } }
        };
        }

        // ✅ "Select All" functionality
        private void cbAllAllow_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var cb in cbList)
                cb.CheckedChanged -= PermissionCheckBox_CheckedChanged;

            foreach (var cb in cbList)
                cb.Checked = cbAllAllow.Checked;

            foreach (var cb in cbList)
                cb.CheckedChanged += PermissionCheckBox_CheckedChanged;
        }

        // ✅ Handle Parent-Child Permission Logic
        private void PermissionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Stop events temporarily to avoid loop
            foreach (var cb in cbList)
                cb.CheckedChanged -= PermissionCheckBox_CheckedChanged;

            CheckBox changed = sender as CheckBox;

            // If changed checkbox is a parent → apply state to children
            if (permissionMap.ContainsKey(changed))
            {
                foreach (var child in permissionMap[changed])
                    child.Checked = changed.Checked;
            }
            else
            {
                // If changed checkbox is a child → update parent state
                foreach (var parent in permissionMap.Keys)
                {
                    var children = permissionMap[parent];
                    parent.Checked = children.All(c => c.Checked);
                }
            }

            // Auto update Select All checkbox
            cbAllAllow.CheckedChanged -= cbAllAllow_CheckedChanged;
            cbAllAllow.Checked = cbList.All(c => c.Checked);
            cbAllAllow.CheckedChanged += cbAllAllow_CheckedChanged;

            // Re-enable event handlers
            foreach (var cb in cbList)
                cb.CheckedChanged += PermissionCheckBox_CheckedChanged;
        }
    }


}
