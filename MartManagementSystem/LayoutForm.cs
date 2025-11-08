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
    public partial class LayoutForm : Form
    {
        CategoryForm categoryForm = new CategoryForm();
        SizeForm sizeForm = new SizeForm();
        BrandForm brandForm = new BrandForm();
        ProductForm productForm = new ProductForm();
        PurchaseForm purchaseForm = new PurchaseForm();
        SupplierForm supplierForm = new SupplierForm(); 
        InventoryForm inventoryForm = new InventoryForm();
        RoleForm roleForm = new RoleForm();
        UserForm userForm = new UserForm();
        PermissionForm permissionForm = new PermissionForm();
        OrderForm orderForm = new OrderForm();
        DashboardForm dashboardForm = new DashboardForm();
        OrderList orderListForm = new OrderList();
        public List<Form> formList = new List<Form>();
        public LayoutForm()
        {
            InitializeComponent();
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

            orderForm.handleOrderList += (s, e) =>
            {
                activeForm(orderListForm);
            };

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
