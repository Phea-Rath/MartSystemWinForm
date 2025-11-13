using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class OrderDetail:Product
    {
        public static List<OrderDetail> orderItems { get; set; } = new List<OrderDetail>();
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal SupPrice { get; set; }

        public static List<OrderDetail> GetOrderDetails(int orderId)
        {
            List<OrderDetail> details = new List<OrderDetail>();

            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                string query = @"
            SELECT od.item_id, i.item_name, od.quantity, od.unit_price, od.total_price
            FROM Order_details od
            JOIN Items i ON od.item_id = i.item_id
            WHERE od.order_id = @order_id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@order_id", orderId);

                try
                {
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        details.Add(new OrderDetail()
                        {
                            ProductId = r.GetInt32(0),
                            ProductName = r.GetString(1),
                            Quantity = r.IsDBNull(2) ? 0 : r.GetInt32(2),
                            UnitPrice = r.IsDBNull(3) ? 0 : r.GetDecimal(3),
                            TotalPrice = r.IsDBNull(4) ? 0 : r.GetDecimal(4)
                        });
                    }
                    r.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Load Order Details: " + ex.Message);
                }
            }

            return details;
        }


    }

}
