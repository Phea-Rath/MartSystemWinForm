using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class Product : BaseEntity
    {
        public static List<Product> ProductList { get; set; } = new List<Product>();

        public int ProductId { get; set; }           // item_id
        public string ProductName { get; set; }      // item_name
        public string ProductCode { get; set; }      // item_code
        public decimal UnitPrice { get; set; } = 0;
        public decimal CostPrice { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? SizeId { get; set; }

        // Navigation properties
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string SizeName { get; set; }

        public Product() { }

        public Product(int productId, string productCode, string productName, int? categoryId, int? brandId, int? sizeId,
                       decimal unitPrice, decimal costPrice, decimal discount, decimal tax, string description, int createdBy)
        {
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            CategoryId = categoryId;
            BrandId = brandId;
            SizeId = sizeId;
            UnitPrice = unitPrice;
            CostPrice = costPrice;
            Discount = discount;
            Description = description;
            CreatedBy = createdBy;
        }

        // Load all products with joins
        public void GetAllProduct()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"
            SELECT 
                p.item_id, p.item_code, p.item_name, p.category_id, p.brand_id, p.size_id,
                p.unit_price, p.cost_price, p.discount, p.description, p.image, p.created_by, p.created_at, p.updated_at,
                c.category_name, b.brand_name, s.size_name
            FROM items p
            LEFT JOIN Categories c ON p.category_id = c.category_id
            LEFT JOIN Brands b ON p.brand_id = b.brand_id
            LEFT JOIN Sizes s ON p.size_id = s.size_id
            WHERE ISNULL(p.is_deleted, 0) = 0
        ";

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    ProductList = new List<Product>();
                    while (reader.Read())
                    {
                        ProductList.Add(new Product()
                        {
                            ProductId = reader.GetInt32(0),
                            ProductCode = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            ProductName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            CategoryId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            BrandId = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            SizeId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                            UnitPrice = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                            CostPrice = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                            Discount = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                            Description = reader.IsDBNull(9) ? "" : reader.GetString(9),
                            ImageUrl = reader.IsDBNull(10) ? "" : reader.GetString(10),
                            CreatedBy = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                            CreatedAt = reader.IsDBNull(12) ? DateTime.Now : reader.GetDateTime(12),
                            UpdatedAt = reader.IsDBNull(13) ? (DateTime?)null : reader.GetDateTime(13),
                            CategoryName = reader.IsDBNull(14) ? "" : reader.GetString(14),
                            BrandName = reader.IsDBNull(15) ? "" : reader.GetString(15),
                            SizeName = reader.IsDBNull(16) ? "" : reader.GetString(16)
                        });
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error: " + ex.Message);
            }
        }

        public bool InsertProduct(Product p)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"
            INSERT INTO items 
            (item_code, item_name, category_id, brand_id, size_id, unit_price, cost_price, discount, description, image, created_by)
            VALUES
            (@code, @name, @category_id, @brand_id, @size_id, @unit_price, @cost_price, @discount, @description, @image , @created_by)
        ";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@code", p.ProductCode ?? "");
            cmd.Parameters.AddWithValue("@name", p.ProductName ?? "");
            cmd.Parameters.AddWithValue("@category_id", p.CategoryId.HasValue ? (object)p.CategoryId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@brand_id", p.BrandId.HasValue ? (object)p.BrandId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@size_id", p.SizeId.HasValue ? (object)p.SizeId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@unit_price", p.UnitPrice);
            cmd.Parameters.AddWithValue("@cost_price", p.CostPrice);
            cmd.Parameters.AddWithValue("@discount", p.Discount);
            cmd.Parameters.AddWithValue("@description", p.Description ?? "");
            cmd.Parameters.AddWithValue("@image", p.ImageUrl);
            cmd.Parameters.AddWithValue("@created_by", p.CreatedBy);

            try
            {
                int result = cmd.ExecuteNonQuery();
                conn.Close();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Product Inserted Successfully");
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

        public bool UpdateProduct(Product p)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"
            UPDATE items SET 
                item_code = @code, item_name = @name, category_id = @category_id, brand_id = @brand_id, size_id = @size_id,
                unit_price = @unit_price, cost_price = @cost_price, discount = @discount, description = @description,image = @image,
                updated_at = @updated_at
            WHERE item_id = @id
        ";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", p.ProductId);
            cmd.Parameters.AddWithValue("@code", p.ProductCode ?? "");
            cmd.Parameters.AddWithValue("@name", p.ProductName ?? "");
            cmd.Parameters.AddWithValue("@category_id", p.CategoryId.HasValue ? (object)p.CategoryId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@brand_id", p.BrandId.HasValue ? (object)p.BrandId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@size_id", p.SizeId.HasValue ? (object)p.SizeId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@unit_price", p.UnitPrice);
            cmd.Parameters.AddWithValue("@cost_price", p.CostPrice);
            cmd.Parameters.AddWithValue("@discount", p.Discount);
            cmd.Parameters.AddWithValue("@image", p.ImageUrl);
            cmd.Parameters.AddWithValue("@description", p.Description ?? "");
            cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);

            try
            {
                int result = cmd.ExecuteNonQuery();
                conn.Close();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Product Updated Successfully");
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

        public bool DeleteProduct(int id)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"DELETE items WHERE item_id = @id";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                int result = cmd.ExecuteNonQuery();
                conn.Close();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Product Deleted Successfully");
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
