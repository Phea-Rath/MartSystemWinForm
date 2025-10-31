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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm register_form = new RegisterForm();
            register_form.ShowDialog();
            this.Hide();
        }

        

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            
            txtPassword.UseSystemPasswordChar = !chShowPassword.Checked;
            
        }
        private void chShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chShowPassword.Checked;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            LayoutForm layout_form = new LayoutForm();
            layout_form.ShowDialog();
        }
    }
}
