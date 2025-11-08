using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class SqlServerConnection
    {
        private static readonly string _connectionString = $"Server={serverName};Database={databaseName};Trusted_Connection=True;";
        private const string serverName = "(localdb)\\ProjectModels";
        private const string databaseName = "mart_system_db";
        

        /// <summary>
        /// Opens and returns a SQL connection
        /// </summary>
        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            try
            {
                connection.Open();
                //MessageBox.Show("Connection successful!");
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to SQL Server: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Executes a query and returns a DataTable
        /// </summary>
        public DataTable ExecuteQuery(string query)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            using (var adapter = new SqlDataAdapter(command))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Executes a non-query (INSERT, UPDATE, DELETE)
        /// </summary>
        public int ExecuteNonQuery(string query)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                return command.ExecuteNonQuery();
            }
        }
    }
}
