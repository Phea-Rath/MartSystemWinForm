using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MartManagementSystem
{
    internal class Purchase : BaseEntity
    {
        public int PurchaseId { get; set; }
        public int SupplierId { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Tax { get; set; }
        public decimal Payment { get; set; }
        public decimal Balance { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string SupplierName { get; set; }

        // Navigation
        public Supplier Supplier { get; set; }
        public List<PurchaseDetail> PurchaseDetails { get; set; }
        public Purchase() { }


        public static bool InsertPurchase(Purchase pur, List<PurchaseDetail> details)
        {
            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    string query = @"
                INSERT INTO Purchases 
                (supplier_id, price, delivery_fee, tax, payment, balance, description, status, total, created_by)
                VALUES (@supplier_id, @price, @delivery_fee, @tax, @payment, @balance, @description, @status, @total, @created_by);
                SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(query, conn, trans);

                    cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = pur.SupplierId;
                    cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = pur.Price;
                    cmd.Parameters.Add("@delivery_fee", SqlDbType.Decimal).Value = pur.DeliveryFee;
                    cmd.Parameters.Add("@tax", SqlDbType.Decimal).Value = pur.Tax;
                    cmd.Parameters.Add("@payment", SqlDbType.Decimal).Value = pur.Payment;
                    cmd.Parameters.Add("@balance", SqlDbType.Decimal).Value = pur.Balance;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar, 500).Value = pur.Description ?? "";
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50).Value = string.IsNullOrEmpty(pur.Status) ? "Active" : pur.Status;
                    cmd.Parameters.Add("@total", SqlDbType.Decimal).Value = pur.Total;
                    cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = User.UserLogin[0].UserId;

                    int purchaseId = Convert.ToInt32(cmd.ExecuteScalar());
                    if (purchaseId <= 0) throw new Exception("Insert purchase failed.");

                    string query_detail = @"
                INSERT INTO Purchase_details (purchase_id, item_id, cost_price, quantity, expire_date, total)
                VALUES (@purchase_id, @item_id, @cost_price, @quantity, @expire_date, @total)";

                    foreach (var d in details)
                    {
                        SqlCommand cmdDetail = new SqlCommand(query_detail, conn, trans);
                        cmdDetail.Parameters.Add("@purchase_id", SqlDbType.Int).Value = purchaseId;
                        cmdDetail.Parameters.Add("@item_id", SqlDbType.Int).Value = d.ProductId;
                        cmdDetail.Parameters.Add("@cost_price", SqlDbType.Decimal).Value = d.CostPrice;
                        cmdDetail.Parameters.Add("@quantity", SqlDbType.Int).Value = d.Quantity;
                        cmdDetail.Parameters.Add("@expire_date", SqlDbType.DateTime).Value = d.ExpireDate;
                        cmdDetail.Parameters.Add("@total", SqlDbType.Decimal).Value = d.SubTotal;
                        cmdDetail.ExecuteNonQuery();
                    }

                    trans.Commit();
                    Services.ShowAlert("✅ Purchase products successfully");
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("❌ Error: " + ex.Message);
                    return false;
                }
            }
        }


        public static List<Purchase> GetAllPurchases()
        {
            List<Purchase> list = new List<Purchase>();
            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                string query = @"SELECT s.*, sp.supplier_name
                                FROM Purchases AS s
                                JOIN Suppliers AS sp
                                    ON s.supplier_id = sp.supplier_id
                                WHERE s.is_deleted = 0
                                ORDER BY s.purchase_id DESC;
                                ";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    list.Add(new Purchase()
                    {
                        PurchaseId = r.GetInt32(0),
                        SupplierId = r.GetInt32(1),
                        Price = r.GetDecimal(2),
                        DeliveryFee = r.GetDecimal(3),
                        Tax = r.GetDecimal(4),
                        Payment = r.GetDecimal(5),
                        Balance = r.GetDecimal(6),
                        Description = r.GetString(8),
                        Status = r.GetString(7),
                        Total = r.GetDecimal(9),
                        CreatedAt = r.IsDBNull(12) ? (DateTime?)null : r.GetDateTime(12),
                        UpdatedAt = r.IsDBNull(13) ? (DateTime?)null : r.GetDateTime(13),
                        SupplierName = r.GetString(14),
                    });
                }
            }
            return list;
        }

        public static Purchase GetPurchaseById(int id)
        {
            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                string query = "SELECT * FROM Purchases WHERE purchase_id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader r = cmd.ExecuteReader();

                if (r.Read())
                {
                    return new Purchase()
                    {
                        PurchaseId = r.GetInt32(0),
                        SupplierId = r.GetInt32(1),
                        Price = r.GetDecimal(2),
                        DeliveryFee = r.GetDecimal(3),
                        Tax = r.GetDecimal(4),
                        Payment = r.GetDecimal(5),
                        Balance = r.GetDecimal(6),
                        Description = r.GetString(7),
                        Status = r.GetString(8),
                    };
                }
                return null;
            }
        }

        public static bool UpdatePurchase(Purchase pur, List<PurchaseDetail> details)
        {
            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // 1️⃣ Update Purchase
                    string queryPurchase = @"
            UPDATE Purchases SET
                supplier_id=@supplier_id,
                price=@price,
                delivery_fee=@delivery_fee,
                tax=@tax,
                payment=@payment,
                balance=@balance,
                description=@description,
                status=@status,
                updated_at=@updated_at
            WHERE purchase_id=@id";

                    SqlCommand cmd = new SqlCommand(queryPurchase, conn, trans);
                    cmd.Parameters.AddWithValue("@id", pur.PurchaseId);
                    cmd.Parameters.AddWithValue("@supplier_id", pur.SupplierId);
                    cmd.Parameters.AddWithValue("@price", pur.Price);
                    cmd.Parameters.AddWithValue("@delivery_fee", pur.DeliveryFee);
                    cmd.Parameters.AddWithValue("@tax", pur.Tax);
                    cmd.Parameters.AddWithValue("@payment", pur.Payment);
                    cmd.Parameters.AddWithValue("@balance", pur.Balance);
                    cmd.Parameters.AddWithValue("@description", pur.Description);
                    cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);
                    cmd.Parameters.AddWithValue("@status", pur.Status);

                    cmd.ExecuteNonQuery();

                    // 2️⃣ Delete old PurchaseDetails
                    string queryDeleteDetail = "DELETE FROM Purchase_details WHERE purchase_id=@purchase_id";
                    SqlCommand cmdDel = new SqlCommand(queryDeleteDetail, conn, trans);
                    cmdDel.Parameters.AddWithValue("@purchase_id", pur.PurchaseId);
                    cmdDel.ExecuteNonQuery();

                    // 3️⃣ Insert new PurchaseDetails
                    string queryInsertDetail = @"
            INSERT INTO Purchase_details (purchase_id, item_id, cost_price, quantity, expire_date, total)
            VALUES (@purchase_id, @item_id, @cost_price, @quantity, @expire_date, @total)";

                    foreach (var d in details)
                    {
                        SqlCommand cmdDetail = new SqlCommand(queryInsertDetail, conn, trans);
                        cmdDetail.Parameters.AddWithValue("@purchase_id", pur.PurchaseId);
                        cmdDetail.Parameters.AddWithValue("@item_id", d.ProductId);
                        cmdDetail.Parameters.AddWithValue("@cost_price", d.CostPrice);
                        cmdDetail.Parameters.AddWithValue("@quantity", d.Quantity);
                        cmdDetail.Parameters.AddWithValue("@expire_date", d.ExpireDate);
                        cmdDetail.Parameters.AddWithValue("@total", d.SubTotal);
                        cmdDetail.ExecuteNonQuery();
                    }

                    trans.Commit();
                    Services.ShowAlert("✅ Purchase updated successfully");
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("❌ Error: " + ex.Message);
                    return false;
                }
            }
        }

        public static bool DeletePurchase(int id)
        {
            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // 1️⃣ Soft delete purchase
                    string queryPurchase = "UPDATE Purchases SET is_deleted = 1 WHERE purchase_id=@id";
                    SqlCommand cmd = new SqlCommand(queryPurchase, conn, trans);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    // 2️⃣ Delete purchase details
                    string queryDetail = "UPDATE Purchase_details SET is_deleted = 1 WHERE purchase_id=@id";
                    SqlCommand cmdDetail = new SqlCommand(queryDetail, conn, trans);
                    cmdDetail.Parameters.AddWithValue("@id", id);
                    cmdDetail.ExecuteNonQuery();

                    trans.Commit();
                    Services.ShowAlert("✅ Purchase deleted successfully");
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("❌ Error: " + ex.Message);
                    return false;
                }
            }
        }

    }

}
