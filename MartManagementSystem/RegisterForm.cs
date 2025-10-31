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
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Form1 login_form = new Form1();
            login_form.ShowDialog();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            txtConfirmPassword.UseSystemPasswordChar = !chShowPassword.Checked;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

            txtPassword.UseSystemPasswordChar = !chShowPassword.Checked;
        }

        private void chShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chShowPassword.Checked;
            txtConfirmPassword.UseSystemPasswordChar = !chShowPassword.Checked;
        }
    }
}
