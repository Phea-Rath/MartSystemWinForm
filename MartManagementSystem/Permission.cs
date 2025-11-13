using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class Permission:Menu
    {
        public User user { get; set; }
        public bool IsActive { get; set; }
        public static List<Permission> permissions { get; set; } = new List<Permission>();
        public static bool InsertPermission(int id)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"INSERT INTO Permission (menu_id, user_id)
                            SELECT menu_id, @UserId FROM Menus;";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool ChangePermission(int userId, int[] menuIds, bool check)
        {
            if (userId == 0 || menuIds == null || menuIds.Length == 0) return false;

            SqlConnection conn = SqlServerConnection.GetConnection();

            // Convert array to comma-separated string: 1,3,5,...
            string inClause = string.Join(",", menuIds);

            string query = $@"
        UPDATE Permission 
        SET is_active = {(check ? 1 : 0)} 
        WHERE user_id = @userId 
        AND menu_id IN ({inClause})";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            try
            {
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error ChangePermission");
                return false;
            }
        }


        public static void GetPermissionUser(int userId)
        {
            permissions.Clear();
            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                string query = @"SELECT menu_id, user_id, is_active 
                         FROM Permission
                         WHERE user_id = @userId
                         ORDER BY menu_id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                try
                {
                    SqlDataReader r = cmd.ExecuteReader();
                    permissions = new List<Permission>();

                    while (r.Read())
                    {
                        permissions.Add(new Permission()
                        {
                            MenuId = r.GetInt32(0),
                            user = new User() { UserId = r.GetInt32(1) },
                            IsActive = r.GetBoolean(2),
                        });
                    }
                    r.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error GetPermissionUser");
                }
            }
        }

    }

}
