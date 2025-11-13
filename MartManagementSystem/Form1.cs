using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace MartManagementSystem
{
    public partial class Form1 : Form
    {
        public event EventHandler handleLogin;
        User _user = new User();
        public Form1()
        {
            InitializeComponent();   
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            string _email = txtEmail.Text.Trim();
            string _password = txtPassword.Text.Trim();
            User.UserLogin.Clear(); 

            if (string.IsNullOrEmpty(_email) || string.IsNullOrEmpty(_password))
            {
                MessageBox.Show("Please enter Email and Password!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

                    SqlConnection conn = SqlServerConnection.GetConnection();
    
                string query = "SELECT * FROM users WHERE email = @email AND password = @password AND is_deleted = 0";
                    SqlCommand cmd = new SqlCommand(query, conn);
        
                    cmd.Parameters.AddWithValue("@email", _email);
                    cmd.Parameters.AddWithValue("@password", _password);

                    try
                    {
                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.Read()) // ✅ Check if account exists
                        {
                    
                                User.UserLogin.Add(new User()
                                {
                                    UserId = r["user_id"] == DBNull.Value ? 0 : Convert.ToInt32(r["user_id"]),
                                    UserName = r["user_name"] == DBNull.Value ? null : r["user_name"].ToString(),
                                    Email = r["email"] == DBNull.Value ? null : r["email"].ToString(),
                                    PhoneNumber = r["phone_number"] == DBNull.Value ? null : r["phone_number"].ToString(),
                                    Password = r["password"] == DBNull.Value ? null : r["password"].ToString(),
                                    ImageUrl = r["image"] == DBNull.Value ? null : r["image"].ToString(),
                                    CreatedBy = r["created_by"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["created_by"]),
                                    IsActive = (bool)(r["is_active"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(r["is_active"])),
                                    LoginAt = r["login_at"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["login_at"]),
                                    CreatedAt = r["created_at"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["created_at"])
                                });
                    
                            if (!Convert.ToBoolean(r["is_active"]))
                            {
                                MessageBox.Show("Account is disabled!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            handleLogin?.Invoke(this, EventArgs.Empty);


                            this.Hide();
                            LayoutForm layout_form = new LayoutForm(this, r["user_name"].ToString(), Convert.ToInt32(r["user_id"]));
                            layout_form.FormClosed += LoyoutFormClosed;
                            layout_form.Show();
                            handleLogin?.Invoke(this, EventArgs.Empty); // event triggers correctly


                        }
                        else
                        {
                            MessageBox.Show("Invalid Email or Password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
        
    
        }

        private void LoyoutFormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
            txtEmail.Clear();
            txtPassword.Clear();
        }
    }
}
