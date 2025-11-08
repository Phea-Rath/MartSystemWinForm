using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class Category : BaseEntity
    {
        public static List<Category> CategoryList { get; set; } = new List<Category>();

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Category() { }

        public Category(int categoryId, string categoryName, int createdBy)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            CreatedBy = createdBy;
        }

        //List<Product> products { get; set; } = new List<Product>();

        public void GetAllCategory()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = "SELECT category_id, category_name, created_by, created_at FROM Categories WHERE is_deleted = 0";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                var result = cmd.ExecuteReader();
                if (result.HasRows)
                {
                    Category.CategoryList = new List<Category>();
                    while (result.Read())
                    {
                        Category.CategoryList.Add(new Category()
                        {
                            CategoryId = result.GetInt32(0),
                            CategoryName = result.GetString(1),
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

        public bool InsertCategory(string categoryName, int createdBy)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();

            string query = @"INSERT INTO Categories (category_name, created_by)
                         VALUES (@category_name, @created_by)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@category_name", categoryName);
            cmd.Parameters.AddWithValue("@created_by", createdBy);

            try
            {
                int result = cmd.ExecuteNonQuery();
                conn.Close();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Category Inserted Successfully");
                    return true;
                }
                else
                {
                    MessageBox.Show("⚠ Insert failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error: " + ex.Message);
                return false;
            }
        }

        public bool UpdateCategory(int id, string categoryName, DateTime updatedAt)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();

            string query = @"UPDATE Categories 
                         SET category_name = @category_name, updated_at = @updated_at 
                         WHERE category_id = @category_id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@category_id", id);
            cmd.Parameters.AddWithValue("@category_name", categoryName);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);

            try
            {
                int result = cmd.ExecuteNonQuery();
                conn.Close();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Category Updated Successfully");
                    return true;
                }
                else
                {
                    MessageBox.Show("⚠ Update failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error: " + ex.Message);
                return false;
            }
        }

        public bool DeleteCategory(int id)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();

            string query = @"DELETE Categories WHERE category_id = @category_id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@category_id", id);

            try
            {
                int result = cmd.ExecuteNonQuery();
                conn.Close();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Category Deleted Successfully");
                    return true;
                }
                else
                {
                    MessageBox.Show("⚠ Delete failed");
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
