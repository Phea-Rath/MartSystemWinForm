using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class Inventory:BaseEntity
    {
        public static List<Inventory> inventories {  get; set; } = new List<Inventory>();
        public int InventoryId { get; set; }
        public string Description { get; set; }

        public Inventory() { }

        public static void GetAllInventories()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"SELECT * FROM Stock_masters WHERE is_deleted = 0";
            SqlCommand cmd = new SqlCommand(query,conn);
            try
            {
                SqlDataReader r = cmd.ExecuteReader();
                Inventory.inventories = new List<Inventory>();
                if(r.HasRows)
                {
                    while (r.Read())
                    {
                        Inventory.inventories.Add(new Inventory()
                        {
                            InventoryId = r.GetInt32(0),
                            Description = r.GetString(1),
                            CreatedBy = r.GetInt32(2),
                            CreatedAt = r.IsDBNull(4)? (DateTime?)null:r.GetDateTime(4),
                            UpdatedAt = r.IsDBNull(5) ? (DateTime?)null : r.GetDateTime(5),
                        });
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Error Get Inentory");
            }
        }


        public static bool InsertInventory(Inventory inven, List<InventoryDetail> detail)
        {
            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string query = @"INSERT INTO Stock_masters (description, created_by) VALUES (@description, @created_by);SELECT SCOPE_IDENTITY();";
                    string query_detail = @"INSERT INTO Stock_details (stock_id, item_id, quantity, expire_date) VALUES (@stock_id, @item_id, @quantity, @expire_date)";
                    SqlCommand cmd = new SqlCommand(query, conn, trans);
                    cmd.Parameters.AddWithValue("@description", inven.Description);
                    cmd.Parameters.AddWithValue("@created_by", inven.CreatedBy);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    if (id > 0)
                    {
                        foreach (InventoryDetail d in detail)
                        {

                            SqlCommand cmd_detail = new SqlCommand(query_detail, conn, trans);
                            cmd_detail.Parameters.AddWithValue("@stock_id", id);
                            cmd_detail.Parameters.AddWithValue("@item_id", d.ProductId);
                            cmd_detail.Parameters.AddWithValue("@quantity", d.Quantity);
                            cmd_detail.Parameters.AddWithValue("@expire_date", d.ExpireDate);
                            cmd_detail.ExecuteNonQuery();
                        }
                        trans.Commit();
                        conn.Close();
                        Services.ShowAlert("Inventory inserted successfully!");
                        return true;
                    }
                    else
                    {
                        conn.Close();
                        Services.ShowAlert("Insert Faill!");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Error:" + ex.Message, "Error Insert Inventory"); return false;
                }
            }
                
        }

        public static bool UpdateInventory(Inventory inven, List<InventoryDetail> detail)
        {
            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // Update master record
                    string query_update_master = @"UPDATE Stock_masters 
                                           SET description = @description, updated_at = GETDATE()
                                           WHERE stock_id = @stock_id";

                    SqlCommand cmd = new SqlCommand(query_update_master, conn, trans);
                    cmd.Parameters.AddWithValue("@description", inven.Description);
                    cmd.Parameters.AddWithValue("@stock_id", inven.InventoryId);
                    cmd.ExecuteNonQuery();

                    // Remove old details
                    string delete_detail = "DELETE FROM Stock_details WHERE stock_id = @stock_id";
                    SqlCommand cmdDel = new SqlCommand(delete_detail, conn, trans);
                    cmdDel.Parameters.AddWithValue("@stock_id", inven.InventoryId);
                    cmdDel.ExecuteNonQuery();

                    // Insert new detail list
                    string insert_detail = @"INSERT INTO Stock_details (stock_id, item_id, quantity, expire_date) 
                                     VALUES (@stock_id, @item_id, @quantity, @expire_date)";
                    foreach (var d in detail)
                    {
                        SqlCommand cmdDetail = new SqlCommand(insert_detail, conn, trans);
                        cmdDetail.Parameters.AddWithValue("@stock_id", inven.InventoryId);
                        cmdDetail.Parameters.AddWithValue("@item_id", d.ProductId);
                        cmdDetail.Parameters.AddWithValue("@quantity", d.Quantity);
                        cmdDetail.Parameters.AddWithValue("@expire_date", d.ExpireDate);
                        cmdDetail.ExecuteNonQuery();
                    }

                    trans.Commit();
                    conn.Close();
                    Services.ShowAlert("Inventory updated successfully!");
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Error: " + ex.Message, "Error Update Inventory");
                    return false;
                }
            }
        }

        public static bool DeleteInventory(int inventoryId)
        {
            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string query = @"UPDATE Stock_masters 
                             SET is_deleted = 1, updated_at = GETDATE()
                             WHERE stock_id = @stock_id";
                    string query_detail = @"UPDATE Stock_details 
                             SET is_deleted = 1
                             WHERE stock_id = @stock_id";

                    SqlCommand cmd = new SqlCommand(query, conn, trans);
                    SqlCommand cmd_detail = new SqlCommand(query_detail, conn, trans);
                    cmd.Parameters.AddWithValue("@stock_id", inventoryId);
                    cmd_detail.Parameters.AddWithValue("@stock_id", inventoryId);
                    cmd.ExecuteNonQuery();
                    cmd_detail.ExecuteNonQuery();

                    trans.Commit();
                    conn.Close();
                    Services.ShowAlert("Inventory deleted successfully!");
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Error: " + ex.Message, "Error Delete Inventory");
                    return false;
                }
            }
        }

    }
}
