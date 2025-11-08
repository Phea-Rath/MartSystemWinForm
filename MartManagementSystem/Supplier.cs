using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class Supplier : BaseEntity
    {
        public static List<Supplier> SupplierList { get; set; } = new List<Supplier>();

        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public Supplier() { }

        public Supplier(int supplierId, string supplierName, string phoneNumber, string email, string address, int createdBy)
        {
            SupplierId = supplierId;
            SupplierName = supplierName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            CreatedBy = createdBy;
        }

        // Get all suppliers
        public void GetAllSuppliers()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"SELECT supplier_id, supplier_name, phone_number, email, address, created_by, created_at 
                         FROM Suppliers WHERE is_deleted = 0";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                SqlDataReader result = cmd.ExecuteReader();
                SupplierList = new List<Supplier>();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        SupplierList.Add(new Supplier()
                        {
                            SupplierId = result.GetInt32(0),
                            SupplierName = result.IsDBNull(1) ? null : result.GetString(1),
                            PhoneNumber = result.IsDBNull(2) ? null : result.GetString(2),
                            Email = result.IsDBNull(3) ? null : result.GetString(3),
                            Address = result.IsDBNull(4) ? null : result.GetString(4),
                            CreatedBy = result.GetInt32(5),
                            CreatedAt = result.GetDateTime(6)
                        });
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error getting suppliers: " + ex.Message);
            }
        }

        // Insert supplier
        public bool InsertSupplier(string name, string phone, string email, string address, int createdBy)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"INSERT INTO Suppliers (supplier_name, phone_number, email, address, created_by) 
                         VALUES (@name, @phone, @email, @address, @created_by)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", phone ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@address", address ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@created_by", createdBy);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Supplier inserted successfully");
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

        // Update supplier
        public bool UpdateSupplier(int id, string name, string phone, string email, string address, DateTime updatedAt)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"UPDATE Suppliers 
                         SET supplier_name = @name, phone_number = @phone, email = @email, address = @address, updated_at = @updated_at 
                         WHERE supplier_id = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@phone", phone ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@address", address ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@updated_at", updatedAt);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Supplier updated successfully");
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

        // Delete supplier
        public bool DeleteSupplier(int id)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"DELETE FROM Suppliers WHERE supplier_id = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Services.ShowAlert("✅ Supplier deleted successfully");
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
