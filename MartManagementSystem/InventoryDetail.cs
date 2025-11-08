using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class InventoryDetail
    {
        public static List<InventoryDetail> InventoryDetailList { get; set; }= new List<InventoryDetail>();
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public string ProductName {  get; set; }
        public int Quantity { get; set; }
        public DateTime? ExpireDate { get; set; }
        public InventoryDetail() { }

        public static void GetInvetoryDetail(int Id)
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"SELECT sd.*, i.item_name FROM Stock_details as sd JOIN items as i ON sd.item_id = i.item_id WHERE stock_id = @Id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", Id);
            try
            {
                SqlDataReader r = cmd.ExecuteReader();
                InventoryDetail.InventoryDetailList = new List<InventoryDetail>();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        InventoryDetailList.Add(new InventoryDetail()
                        {
                            InventoryId = r.GetInt32(0),
                            ProductId = r.GetInt32(1),
                            Quantity = r.GetInt32(2),
                            ExpireDate = r.GetDateTime(3),
                            ProductName = r.GetString(5),
                        });
                    }
                }
            }catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message, "Error Get InventoryDetail");
            }
        }
    }
}
