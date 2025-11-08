using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class Sizes : BaseEntity
    {
        public static List<Sizes> SizeList { get; set; } = new List<Sizes>();

        public int SizeId { get; set; }
        public string SizeName { get; set; }

        public Sizes() { }

        public Sizes(int sizeId, string sizeName, int createdBy)
        {
            SizeId = sizeId;
            SizeName = sizeName;
            CreatedBy = createdBy;
        }

        //List<Product> products { get; set; } = new List<Product>();

        public void GetAllSizes()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = "SELECT size_id, size_name, created_by, created_at FROM Sizes WHERE is_deleted = 0";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                var result = cmd.ExecuteReader();
                if (result.HasRows)
                {
                    Sizes.SizeList = new List<Sizes>();
                    while (result.Read())
                    {
                        Sizes.SizeList.Add(new Sizes()
                        {
                            SizeId = result.GetInt32(0),
                            SizeName = result.GetString(1),
                            CreatedBy = result.GetInt32(2),
                            CreatedAt = result.GetDateTime(3)
                        });
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Errors: " + ex.Message);
            }
        }

        public bool InsertSize(string sizeName, int createdBy)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();

            string query = @"INSERT INTO Sizes (size_name, created_by)
                         VALUES (@size_name, @created_by)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@size_name", sizeName);
            cmd.Parameters.AddWithValue("@created_by", createdBy);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Size Inserted Successfully");
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

        public bool UpdateSize(int Id, string sizeName, DateTime updatedAt)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();

            string query = @"UPDATE Sizes SET size_name = @size_name, updated_at = @updated_at WHERE size_id = @size_id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@size_id", Id);
            cmd.Parameters.AddWithValue("@size_name", sizeName);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Size Updated Successfully");
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

        public bool DeleteSize(int Id)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();

            string query = @"DELETE Sizes WHERE size_id = @size_id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@size_id", Id);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Size Deleted Successfully");
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
