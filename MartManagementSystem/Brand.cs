using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class Brand:BaseEntity
    {
        public static List<Brand> BrandList {  get; set; } = new List<Brand>();
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public Brand() { }
        public Brand(int brandId, string brandName, int createdBy)
        {
            BrandId = brandId;
            BrandName = brandName;
            CreatedBy = createdBy;
        }

        //List<Product> products { get; set; } = new List<Product>();

        public void GetAllBrand()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = "SELECT brand_id, brand_name, created_by, created_at  FROM Brands WHERE is_deleted = 0";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                var result = cmd.ExecuteReader();
                if (result.HasRows)
                {
                    Brand.BrandList = new List<Brand>();
                    while (result.Read())
                    {
                        Brand.BrandList.Add(new Brand()
                        {
                            BrandId = result.GetInt32(0),
                            BrandName = result.GetString(1),
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
        public bool InsertBrand(string brandName, int createdBy)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();

            string query = @"INSERT INTO Brands (brand_name, created_by)
                     VALUES (@brand_name, @created_by)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@brand_name", brandName);
            cmd.Parameters.AddWithValue("@created_by", createdBy);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Brand Inserted Successfully");
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

        public bool UpdateBrand(int Id, string brandName, DateTime updatedAt)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();

            string query = @"UPDATE Brands SET brand_name = @brand_name, updated_at = @updated_at WHERE brand_id = @brand_id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@brand_id", Id);
            cmd.Parameters.AddWithValue("@brand_name", brandName);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Brand Updated Successfully");
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

        public bool DeleteBrand(int Id)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();

            string query = @"DELETE Brands WHERE brand_id = @brand_id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@brand_id", Id);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Brand Deleted Successfully");
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
