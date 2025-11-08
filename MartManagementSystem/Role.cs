using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class Role : BaseEntity
    {
        public static List<Role> RoleList { get; set; } = new List<Role>();

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public Role() { }

        public Role(int roleId, string roleName, string description, int createdBy)
        {
            RoleId = roleId;
            RoleName = roleName;
            Description = description;
            CreatedBy = createdBy;
        }

        // Get all roles
        public void GetAllRoles()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = "SELECT role_id, role_name, description, created_by, created_at FROM Roles WHERE is_deleted = 0";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                SqlDataReader result = cmd.ExecuteReader();
                Role.RoleList = new List<Role>();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        Role.RoleList.Add(new Role()
                        {
                            RoleId = result.GetInt32(0),
                            RoleName = result.GetString(1),
                            Description = result.IsDBNull(2) ? null : result.GetString(2),
                            CreatedBy = result.IsDBNull(3) ? (int?)null : result.GetInt32(3),   // FIX
                            CreatedAt = result.IsDBNull(4) ? (DateTime?)null : result.GetDateTime(4) // FIX
                        });
                    }
                }

                result.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error getting roles: " + ex.Message);
            }
        }


        // Insert role
        public bool InsertRole(string roleName, string description, int createdBy)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"INSERT INTO Roles (role_name, description, created_by) 
                         VALUES (@role_name, @description, @created_by)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@role_name", roleName);
            cmd.Parameters.AddWithValue("@description", description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@created_by", createdBy);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Role inserted successfully");
                    conn.Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("⚠ Insert failed");
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error: " + ex.Message);
                return false;
            }
        }

        // Update role
        public bool UpdateRole(int id, string roleName, string description, DateTime updatedAt)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"UPDATE Roles SET role_name = @role_name, description = @description, updated_at = @updated_at 
                         WHERE role_id = @role_id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@role_id", id);
            cmd.Parameters.AddWithValue("@role_name", roleName);
            cmd.Parameters.AddWithValue("@description", description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Role updated successfully");
                    conn.Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("⚠ Update failed");
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error: " + ex.Message);
                return false;
            }
        }

        // Delete role
        public bool DeleteRole(int id)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"DELETE FROM Roles WHERE role_id = @role_id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@role_id", id);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Role deleted successfully");
                    conn.Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("⚠ Delete failed");
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error: " + ex.Message);
                return false;
            }
        }
    }
}
