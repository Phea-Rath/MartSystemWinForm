using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class Menu:BaseEntity
    {
        public static List<Menu> menus = new List<Menu>();
        public int MenuId { get; set; }
        public string MenuName { get; set; }

        public List<Permission> Permissions { get; set; } = new List<Permission>();

        public void GetAllMenus()
        {
            var conn = SqlServerConnection.GetConnection();
            string query = @"SELECT menu_id, menu_name FROM Menus";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        Menu.menus.Add(new Menu()
                        {
                            MenuId = r.GetInt32(0),
                            MenuName = r.GetString(1),
                        });
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }
    }
}
