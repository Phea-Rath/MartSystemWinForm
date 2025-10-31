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
        List<Form> formList = new List<Form>();
        public LayoutForm()
        {
            InitializeComponent();
            formList.AddRange(new Form[] { categoryForm, sizeForm, brandForm, productForm}); 
            categoryForm.TopLevel = false;
            this.pContent.Controls.Add(categoryForm);
            categoryForm.Dock = System.Windows.Forms.DockStyle.Fill;
            categoryForm.AutoSize = true;
            btnCategory.Click += btnCategory_Click;

            sizeForm.TopLevel = false;
            this.pContent.Controls.Add(sizeForm);
            sizeForm.Dock = System.Windows.Forms.DockStyle.Fill;
            sizeForm.AutoSize = true;
            btnSize.Click += btnSize_Click;

            brandForm.TopLevel = false;
            this.pContent.Controls.Add(brandForm);
            brandForm.Dock = System.Windows.Forms.DockStyle.Fill;
            brandForm.AutoSize = true;
            btnBrand.Click += btnBrand_Click;

            productForm.TopLevel = false;
            this.pContent.Controls.Add(productForm);
            productForm.Dock = System.Windows.Forms.DockStyle.Fill;
            productForm.AutoSize = true;
        }

        private void activeForm(Form form)
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
            activeForm(brandForm);
        }

        private void btnSize_Click(object sender, EventArgs e)
        {
            activeForm(sizeForm);
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            activeForm(categoryForm);
        }

        private void SettingTimer_Click(object sender, EventArgs e)
        {
            if (resizeSetting == false)
            {
                // Expand panel
                pSetting.Height += 10; // speed of animation (increase for faster)
                if (pSetting.Height >= pSetting.MaximumSize.Height) // 200 = desired expanded height
                {
                    timerSetting.Stop();
                    resizeSetting = true; // mark as expanded
                }
            }
            else
            {
                // Collapse panel
                pSetting.Height -= 10;
                if (pSetting.Height <= pSetting.MinimumSize.Height) // 50 = collapsed height
                {
                    timerSetting.Stop();
                    resizeSetting = false; // mark as collapsed
                }
            }
        }

        bool resizeSetting = false;

        private void btnSetting_Click(object sender, EventArgs e)
        {
            timerSetting.Start();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            timerProduct.Start();
            activeForm(productForm);
        }
        bool resizeProduct = false;
        private void timerProduct_Click(object sender, EventArgs e)
        {
            if (resizeProduct == false)
            {
                // Expand panel
                pProduct.Height += 10; // speed of animation (increase for faster)
                if (pProduct.Height >= pProduct.MaximumSize.Height) // 200 = desired expanded height
                {
                    timerProduct.Stop();
                    resizeProduct = true; // mark as expanded
                }
            }
            else
            {
                // Collapse panel
                pProduct.Height -= 10;
                if (pProduct.Height <= pProduct.MinimumSize.Height) // 50 = collapsed height
                {
                    timerProduct.Stop();
                    resizeProduct = false; // mark as collapsed
                }
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login_form = new Form1();
            login_form.ShowDialog();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            categoryForm.Hide();
        }
    }
}
