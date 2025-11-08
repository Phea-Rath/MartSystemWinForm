using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class User : BaseEntity
    {
        public int UserId { get; set; }
        public int? RoleId { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LoginAt { get; set; }

        // Relationship
        public Role Role { get; set; }

        // Static list to bind with DataGridView
        public static List<User> UserList { get; set; } = new List<User>();

        public static List<User> UserLogin { get; set; } = new List<User>();

        public User() { }

        public User(int userId, string userName, string phone, string email, string image, string pass, int? roleId, bool isActive, DateTime? loginAt, int createdBy)
        {
            UserId = userId;
            UserName = userName;
            PhoneNumber = phone;
            Email = email;
            ImageUrl = image;
            Password = pass;
            RoleId = roleId;
            IsActive = isActive;
            LoginAt = loginAt;
            CreatedBy = createdBy;
        }

        // SELECT Users with Role Name (JOIN)
        public void GetAllUsers()
        {
            UserList.Clear();
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"
    SELECT u.user_id, u.user_name, u.phone_number, u.email, u.image, 
           u.password, u.role_id, u.is_active, u.login_at, u.created_by,
           u.created_at,u.updated_at,
           r.role_name
    FROM Users u
    LEFT JOIN Roles r ON u.role_id = r.role_id
    WHERE u.is_deleted = 0;
    ";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader r = cmd.ExecuteReader();

            while (r.Read())
            {
                UserList.Add(new User()
                {
                    UserId = r.GetInt32(0),
                    UserName = r.GetString(1),
                    PhoneNumber = r.IsDBNull(2) ? null : r.GetString(2),
                    Email = r.IsDBNull(3) ? null : r.GetString(3),
                    ImageUrl = r.IsDBNull(4) ? null : r.GetString(4),
                    Password = r.GetString(5),
                    RoleId = r.IsDBNull(6) ? 0 : r.GetInt32(6),
                    IsActive = r.GetBoolean(7),
                    LoginAt = r.IsDBNull(8) ? (DateTime?)null : r.GetDateTime(8),
                    CreatedBy = r.IsDBNull(9) ? 0 : r.GetInt32(9),
                    UpdatedAt = r.IsDBNull(10) ? (DateTime?)null : r.GetDateTime(10),
                    CreatedAt = r.IsDBNull(11) ? (DateTime?)null : r.GetDateTime(11),

                    Role = new Role()
                    {
                        RoleId = r.IsDBNull(6) ? 0 : r.GetInt32(6),
                        RoleName = r.IsDBNull(12) ? null : r.GetString(12)
                    }
                });
            }

            r.Close();
            conn.Close();
        }


        // INSERT
        public bool InsertUser(User u)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"INSERT INTO Users (user_name, phone_number, email, image, password, role_id, created_by, is_active)
                         VALUES (@name, @phone, @email, @image, @pass, @role, @createdBy, 1)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", u.UserName);
            cmd.Parameters.AddWithValue("@phone", (object)u.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object)u.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@image", (object)u.ImageUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@pass", u.Password);
            cmd.Parameters.AddWithValue("@role", (object)u.RoleId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@createdBy", u.CreatedBy);

            return cmd.ExecuteNonQuery() > 0;
        }

        // UPDATE
        public bool UpdateUser(User u)
        {
            
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"UPDATE Users SET user_name=@name, phone_number=@phone, email=@email, 
                         image=@image, role_id=@role, updated_at=@updated
                         WHERE user_id=@id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", u.UserName);
            cmd.Parameters.AddWithValue("@phone", (object)u.PhoneNumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object)u.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@image", (object)u.ImageUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@role", (object)u.RoleId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@updated", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", u.UserId);

            return cmd.ExecuteNonQuery() > 0;
        }

        // DELETE (Soft Delete)
        public bool DeleteUser(int id)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"UPDATE Users SET is_deleted = 1 WHERE user_id = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
