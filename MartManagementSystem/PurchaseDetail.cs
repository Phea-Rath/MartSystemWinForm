using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartManagementSystem
{
    internal class PurchaseDetail:Product
    {
        public int PurchaseId { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

        // Navigation
        public static List<PurchaseDetail> products { get; set; } = new List<PurchaseDetail>();
        public Purchase Purchase { get; set; }
        public Product Product { get; set; }
        public PurchaseDetail() { }

        public static List<PurchaseDetail> GetPurchaseDetailsByPurchaseId(int purchaseId)
        {
            List<PurchaseDetail> details = new List<PurchaseDetail>();

            using (SqlConnection conn = SqlServerConnection.GetConnection())
            {
                string query = @"
            SELECT i.item_name, pd.purchase_id, pd.item_id, pd.cost_price, pd.quantity, pd.total
            FROM Purchase_details as pd JOIN items as i ON pd.item_id = i.item_id
            WHERE purchase_id = @purchase_id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@purchase_id", purchaseId);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        details.Add(new PurchaseDetail()
                        {
                            ProductName = r.GetString(0),
                            PurchaseId = r.GetInt32(1),
                            ProductId = r.GetInt32(2),
                            CostPrice = r.GetDecimal(3),
                            Quantity = r.GetInt32(4),
                            SubTotal = r.GetDecimal(5)
                        });
                    }
                }
            }

            return details;
        }

    }

}
