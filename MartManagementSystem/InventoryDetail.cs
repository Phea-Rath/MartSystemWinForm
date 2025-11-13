using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class InventoryDetail:Product
    {
        public static List<InventoryDetail> InventoryDetailList { get; set; }= new List<InventoryDetail>();
        public static List<InventoryDetail> saleList { get; set; } = new List<InventoryDetail>();
        public int InventoryId { get; set; }
        public int Quantity { get; set; }
        public DateTime? ExpireDate { get; set; }
        public InventoryDetail() { }
        public static void GetSaleItems()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"SELECT 
                            i.item_id,
                            i.item_name,
                            i.brand_id,
                            i.category_id,
                            i.size_id,
                            b.brand_name,
                            c.category_name,
                            s.size_name,
                            i.discount,
                            i.unit_price,
                            i.image,
                            SUM(ISNULL(sd.quantity, 0)) AS total_quantity
                        FROM Stock_details sd
                        JOIN Items i ON sd.item_id = i.item_id
                        JOIN Brands b ON i.brand_id = b.brand_id
                        JOIN Categories c ON i.category_id = c.category_id
                        JOIN Sizes s ON i.size_id = s.size_id
                        WHERE sd.is_deleted = 0
                        GROUP BY 
                            i.item_id,
                            i.item_name,
                            i.brand_id,
                            i.category_id,
                            i.size_id,
                            b.brand_name,
                            c.category_name,
                            s.size_name,
                            i.discount,
                            i.unit_price,
                            i.image
                        ORDER BY i.item_name;
                        ";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                SqlDataReader r = cmd.ExecuteReader();
                InventoryDetail.saleList = new List<InventoryDetail>();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        saleList.Add(new InventoryDetail()
                        {
                            ProductId = r.GetInt32(0),
                            ProductName = r.GetString(1),
                            BrandId = r.GetInt32(2),
                            CategoryId = r.GetInt32(3),
                            SizeId = r.GetInt32(4),
                            BrandName = r.GetString(5),
                            CategoryName = r.GetString(6),
                            SizeName = r.GetString(7),
                            Discount = r.GetDecimal(8),
                            UnitPrice = r.GetDecimal(9),
                            ImageUrl = r.GetString(10),
                            Quantity = r.IsDBNull(11) ? 0 : r.GetInt32(11),
                        });
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message, "Error GetSaleitems");
            }
        }

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
