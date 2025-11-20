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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace MartManagementSystem
{
    public partial class LayoutForm : Form
    {
        public event EventHandler OpenDashboard;
        CategoryForm categoryForm = new CategoryForm();
        SizeForm sizeForm = new SizeForm();
        BrandForm brandForm = new BrandForm();
        ProductForm productForm;
        PurchaseForm purchaseForm;
        SupplierForm supplierForm = new SupplierForm();
        InventoryForm inventoryForm;
        RoleForm roleForm = new RoleForm();
        UserForm userForm;
        PermissionForm permissionForm = new PermissionForm();
        OrderForm orderForm;
        DashboardForm dashboardForm;
        OrderList orderListForm;
        public List<Form> formList = new List<Form>();
        Form1 _loginForm;
        private string _username;
        private int _user_id;
        public LayoutForm(Form1 loginForm, string username,int user_id)
        {
            InitializeComponent();
            dashboardForm = new DashboardForm(this);
            purchaseForm = new PurchaseForm(supplierForm);
            productForm = new ProductForm(categoryForm,brandForm,sizeForm);
            inventoryForm = new InventoryForm(purchaseForm,productForm);
            orderForm = new OrderForm(purchaseForm,inventoryForm);
            orderListForm = new OrderList(orderForm);
            userForm = new UserForm(roleForm);
            _loginForm = loginForm;
            _username = username;
            _user_id = user_id;
            formList.AddRange(new Form[] { categoryForm, sizeForm, brandForm, productForm, purchaseForm, supplierForm, inventoryForm, roleForm, userForm, permissionForm, orderForm, dashboardForm, orderListForm });

            foreach (Form form in formList)
            {
                form.TopLevel = false;
                this.pContent.Controls.Add(form);
                form.Dock = System.Windows.Forms.DockStyle.Fill;
                form.AutoSize = true;
            }
            btnSize.Click += btnSize_Click;
            btnCategory.Click += btnCategory_Click;
            btnBrand.Click += btnBrand_Click;
            btnSupplier.Click += btnSupplier_Click;
            btnRole.Click += btnRole_CLick;
            btnAccount.Click += btnAccount_CLick;
            btnPermission.Click += btnPermission_Click;
            btnOrder.Click += btnOrder_CLick;
            dashboardForm.Show();

            _loginForm.handleLogin += LoadLayout;
            orderForm.handleOrderList += (s, e) =>
            {
                activeForm(orderListForm);
            };
            orderListForm.handleOrder += (s, e) =>
            {
                activeForm(orderForm);
            };

            LoadPermission();
        }

        private void LoadLayout(object sender,EventArgs e)
        {
            Services.ShowAlert($"{_username} Logined!");
            LoadPermission();
        }

        public void LoadPermission()
        {

            // Always refresh permissions from DB
            Permission.GetPermissionUser(_user_id);

            string folder = Path.Combine(System.Windows.Forms.Application.StartupPath, "assets");
            string img = Convert.ToString(User.UserLogin[0].ImageUrl);
            string imgPath = Path.Combine(folder, img);

            if (File.Exists(imgPath))
            {
                pbProfile.Image = Image.FromFile(imgPath);
                pbProfile.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            lblAcc.Text = User.UserLogin[0].UserName;

            Dictionary<int, Button> map = new Dictionary<int, Button>()
            {
                {1, btnDashboard},
                {2, btnProduct},
                {3, btnSize},
                {4, btnCategory},
                {5, btnBrand},
                {6, btnInven},
                {13, btnPurchase},
                {8, btnOrder},
                {9, btnSetting},
                {12, btnRole},
                {11, btnPermission},
                {7, btnSupplier},
                {10, btnAccount},
            };

            // Hide all first (ensures reset)
            foreach (var btn in map.Values)
                btn.Visible = false;

            // Apply new visibility
            foreach (var p in Permission.permissions)
            {
                if (map.ContainsKey(p.MenuId))
                {
                    map[p.MenuId].Visible = p.IsActive;
                    //MessageBox.Show($"{p.IsActive}");
                    if (p.MenuId == 2)
                    {
                        btnBrand.Visible = false;
                    }
                }
            }

            //// Refresh the UI
            //this.Refresh();
            //System.Windows.Forms.Application.DoEvents();
        }




        private void btnOrder_CLick(object sender, EventArgs e)
        {
            lbTitle.Text = "Sales";
            activeForm(orderForm);
        }

        private void btnPermission_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Permission";
            activeForm(permissionForm);
        }

        private void btnAccount_CLick(object sender, EventArgs e)
        {
            lbTitle.Text = "Accounts";
            activeForm(userForm);
        }

        private void btnRole_CLick(object sender, EventArgs e)
        {
            lbTitle.Text = "Role";
            activeForm(roleForm);
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Supplier";
            activeForm(supplierForm);
        }

        public void activeForm(Form form)
        {
            foreach (Form btn in formList)
            {
                if (btn == form)
                {
                    btn.Show();
                    OpenDashboard?.Invoke(this,EventArgs.Empty);
                }
                else
                {
                    btn.Hide();
                }
            }
        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Brand";
            activeForm(brandForm);
        }

        private void btnSize_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Size";
            activeForm(sizeForm);
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Category";
            activeForm(categoryForm);
        }

        private void SettingTimer_Click(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            DropDown(timer, ref resizeSetting, pSetting);
        }

        bool resizeSetting = false;

        private void btnSetting_Click(object sender, EventArgs e)
        {
            timerSetting.Start();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Product";
            timerProduct.Start();
            activeForm(productForm);
        }
        bool resizeProduct = false;
        private void timerProduct_Click(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            DropDown(timer, ref resizeProduct, pProduct);   
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            bool off = true;
            DropDown(timerPurchase,ref off, pPurchase);
            DropDown(timerProduct, ref off, pProduct);
            DropDown(timerSetting, ref off, pSetting);
            this.Hide();
            Form1 login_form = new Form1();
            login_form.ShowDialog();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Dashboard";
            activeForm(dashboardForm);
        }

        

        bool resizePurchase = false;
        private void timerPurchase_Click(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            DropDown(timer,ref resizePurchase, pPurchase);
        }

        private void DropDown(Timer timer,ref bool resize,Panel panel)
        {
            if (!resize)
            {
                panel.Height += 10;
                if (panel.Height >= panel.MaximumSize.Height)
                {
                    timer.Stop();
                    resize = true;
                }
            }
            else
            {
                panel.Height -= 10;
                if (panel.Height <= panel.MinimumSize.Height)
                {
                    timer.Stop();
                    resize = false;
                }
            }
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Purchase";
            timerPurchase.Start();
            activeForm(purchaseForm);
        }

        private void btnInven_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Inventory";
            activeForm(inventoryForm);
        }

        
    }
}
