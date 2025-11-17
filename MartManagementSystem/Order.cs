using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    public class Order : BaseEntity
    {
        public int OrderId { get; set; }
        public decimal SubTotalPrice { get; set; }
        public decimal Discount { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }

        public static List<Order> orders { get; set; } = new List<Order>();

        public static bool InsertOrder(Order order, List<OrderDetail> details)
        {
            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // Insert into Orders
                    string queryOrder = @"
                INSERT INTO Orders (subtotal_price, discount, payment_method, total_price, created_by)
                VALUES (@subtotal_price, @discount, @payment_method, @total_price, @created_by);
                SELECT SCOPE_IDENTITY();
            ";

                    SqlCommand cmd = new SqlCommand(queryOrder, conn, trans);
                    cmd.Parameters.AddWithValue("@subtotal_price", order.SubTotalPrice);
                    cmd.Parameters.AddWithValue("@discount", order.Discount);
                    cmd.Parameters.AddWithValue("@payment_method", order.PaymentMethod);
                    cmd.Parameters.AddWithValue("@total_price", order.TotalPrice);
                    cmd.Parameters.AddWithValue("@created_by", User.UserLogin[0].UserId);

                    // Get inserted order_id
                    int orderId = Convert.ToInt32(cmd.ExecuteScalar());

                    if (orderId > 0)
                    {
                        // Insert order details
                        string queryDetail = @"
                    INSERT INTO Order_details (order_id, item_id, quantity, unit_price, total_price)
                    VALUES (@order_id, @item_id, @quantity, @unit_price, @total_price);
                ";

                        foreach (OrderDetail d in details)
                        {
                            SqlCommand cmd_detail = new SqlCommand(queryDetail, conn, trans);
                            cmd_detail.Parameters.AddWithValue("@order_id", orderId);
                            cmd_detail.Parameters.AddWithValue("@item_id", d.ProductId);
                            cmd_detail.Parameters.AddWithValue("@quantity", d.Quantity);
                            cmd_detail.Parameters.AddWithValue("@unit_price", d.UnitPrice);
                            cmd_detail.Parameters.AddWithValue("@total_price", d.TotalPrice);
                            cmd_detail.ExecuteNonQuery();
                        }

                        trans.Commit();
                        conn.Close();
                        Services.ShowAlert("Order inserted successfully!");
                        return true;
                    }
                    else
                    {
                        conn.Close();
                        Services.ShowAlert("Insert Failed!");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show("Error: " + ex.Message, "Error Insert Order");
                    return false;
                }
            }
        }


        public static void GetAllOrders()
        {
            Order.orders = new List<Order>();

            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                string query = @"SELECT order_id, subtotal_price, discount, payment_method, total_price, created_by, created_at 
                         FROM Orders
                         WHERE is_deleted = 0
                         ORDER BY order_id DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        orders.Add(new Order()
                        {
                            OrderId = r.GetInt32(0),
                            SubTotalPrice = r.IsDBNull(1) ? 0 : r.GetDecimal(1),
                            Discount = r.IsDBNull(2) ? 0 : r.GetDecimal(2),
                            PaymentMethod = r.IsDBNull(3) ? "" : r.GetString(3),
                            TotalPrice = r.IsDBNull(4) ? 0 : r.GetDecimal(4),
                            CreatedBy = r.IsDBNull(5) ? 0 : r.GetInt32(5),
                            CreatedAt = r.IsDBNull(6) ? DateTime.Now : r.GetDateTime(6)
                        });
                    }
                    r.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Load Orders: " + ex.Message);
                }
            }
        }

    }

}
