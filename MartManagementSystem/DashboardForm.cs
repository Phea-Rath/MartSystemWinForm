using MartManagementSystem.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MartManagementSystem
{
    public partial class DashboardForm : Form
    {
        LayoutForm _layout;
        public DashboardForm(LayoutForm layout)
        {
            InitializeComponent();
            _layout = layout;
            LoadData();
            this.Shown += (s, e) => LoadData();
            this.FormClosed += (s, e) => LoadData();
            this.FormClosing += (s, e) => LoadData();
            _layout.OpenDashboard += (s, e) => LoadData();
        }

        private void LoadData()
        {
            decimal total = TotalOfSale();
            lblSale.Text = "$"+total.ToString();
            int quenInv = TotalOfInventory();
            lblInv.Text = quenInv.ToString() + " in stock";
            decimal purchase = TotalOfPurchase();
            lblPur.Text = "$"+purchase.ToString();
            int supplier = TotalOfSupplier();
            lblSup.Text = supplier.ToString();
            var ProList = PopularSale();
            LoadPopSale(ProList);
            var InvenList = PopularInventory();
            LoadPopInventory(InvenList);
            var PurchaseList = PopularPurchase();
            LoadPopPurchase(PurchaseList);
        }

        private decimal TotalOfSale()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = "SELECT COALESCE(SUM(total_price), 0) AS total FROM Orders WHERE is_deleted = 0";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                int res = Convert.ToInt32(cmd.ExecuteScalar());
                if(res > 0)
                {
                    return res;
                }
                return 0;
            }catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error TotalOfSale");
                return 0;
            }
        }

        private int TotalOfInventory()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"SELECT COALESCE(SUM(quantity),0) AS total FROM Stock_details 
                            WHERE is_deleted = 0";
            string queryOrder = @"SELECT COALESCE(SUM(quantity),0) AS total FROM Order_details
                            WHERE is_deleted = 0";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlCommand cmdOrder = new SqlCommand(queryOrder, conn);
            try
            {
                int res = Convert.ToInt32(cmd.ExecuteScalar());
                int resOrder = Convert.ToInt32(cmdOrder.ExecuteScalar());
                if (res > 0&&resOrder>0)
                {
                    return res - resOrder;
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error TotalOfInventory");
                return 0;
            }
        }

        private decimal TotalOfPurchase()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = "SELECT COALESCE(SUM(total),0) AS total FROM Purchases WHERE is_deleted = 0";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                decimal res = Convert.ToDecimal(cmd.ExecuteScalar());
                if (res > 0)
                {
                    return res;
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error TotalOfPurchase");
                return 0;
            }
        }

        private int TotalOfSupplier()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"SELECT COUNT(*) FROM Suppliers 
                            WHERE is_deleted = 0";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                int res = Convert.ToInt32(cmd.ExecuteScalar());
                if (res > 0)
                {
                    return res;
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error TotalOfSupplier");
                return 0;
            }
        }

        private List<OrderDetail> PopularSale()
        {
            List<OrderDetail> list = new List<OrderDetail>();
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"SELECT TOP 3 i.item_name,  od.item_id, i.image, COALESCE(SUM(od.quantity),0) AS total_quantity,COALESCE(SUM(od.total_price),0), b.brand_name 
                            FROM Order_Details od 
                            JOIN Items i ON od.item_id = i.item_id                           
                            JOIN Brands b ON b.brand_id = i.brand_id 
                            WHERE od.is_deleted = 0 
                            GROUP BY od.item_id, i.item_name, i.image, b.brand_name  
                            ORDER BY total_quantity DESC;";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new OrderDetail()
                        {
                            ProductName = reader.GetString(0),
                            ProductId = reader.GetInt32(1),
                            ImageUrl = reader.GetString(2),
                            Quantity = reader.GetInt32(3),
                            TotalPrice = reader.GetDecimal(4),
                            BrandName = reader.GetString(5),
                        });
                    }
                }
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error TotalOfSale");
                return null;
            }
        }

        private List<InventoryDetail> PopularInventory()
        {
            List<InventoryDetail> list = new List<InventoryDetail>();
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"SELECT TOP 3 i.item_name,  od.item_id, i.image, COALESCE(SUM(od.quantity),0) AS total_quantity, b.brand_name 
                            FROM Stock_details od 
                            JOIN Items i ON od.item_id = i.item_id                           
                            JOIN Brands b ON b.brand_id = i.brand_id 
                            WHERE od.is_deleted = 0 
                            GROUP BY od.item_id, i.item_name, i.image, b.brand_name  
                            ORDER BY total_quantity DESC;";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new InventoryDetail()
                        {
                            ProductName = reader.GetString(0),
                            ProductId = reader.GetInt32(1),
                            ImageUrl = reader.GetString(2),
                            Quantity = reader.GetInt32(3),
                            BrandName = reader.GetString(4),
                        });
                    }
                }
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error TotalOfSale");
                return null;
            }
        }

        private List<PurchaseDetail> PopularPurchase()
        {
            List<PurchaseDetail> list = new List<PurchaseDetail>();
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = @"SELECT TOP 3 i.item_name,  od.item_id, i.image, COALESCE(SUM(od.quantity),0) AS total_quantity, COALESCE(SUM(od.total),0) AS total, b.brand_name 
                            FROM Purchase_details od 
                            JOIN Items i ON od.item_id = i.item_id                           
                            JOIN Brands b ON b.brand_id = i.brand_id 
                            WHERE od.is_deleted = 0 
                            GROUP BY od.item_id, i.item_name, i.image, b.brand_name  
                            ORDER BY total_quantity DESC;";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new PurchaseDetail()
                        {
                            ProductName = reader.GetString(0),
                            ProductId = reader.GetInt32(1),
                            ImageUrl = reader.GetString(2),
                            Quantity = reader.GetInt32(3),
                            SubTotal = reader.GetDecimal(4),
                            BrandName = reader.GetString(5),
                        });
                    }
                }
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error PopularPurchase");
                return null;
            }
        }

        private Panel CreateCartList(OrderDetail p)
        {
            string folder = Path.Combine(System.Windows.Forms.Application.StartupPath, "assets");
            string imgPath = Path.Combine(folder, p.ImageUrl);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            
            // pFrame
            // 
            Panel pFrame = new Panel();
            pFrame.BackColor = System.Drawing.Color.Transparent;
            pFrame.Dock = System.Windows.Forms.DockStyle.Top;
            pFrame.Location = new System.Drawing.Point(10, 10);
            pFrame.Name = "pFrame";
            pFrame.Padding = new System.Windows.Forms.Padding(5);
            pFrame.Size = new System.Drawing.Size(586, 49);
            pFrame.TabIndex = 3;
            // lblQuantity
            // 
            Label lblQuantity = new Label();
            lblQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new System.Drawing.Point(539, 31);
            lblQuantity.Size = new System.Drawing.Size(40, 13);
            lblQuantity.TabIndex = 4;
            lblQuantity.Text = "x" + p.Quantity;
            pFrame.Controls.Add(lblQuantity);
            // 
            // lblPrice
            // 

            Label lblPrice = new Label();
           lblPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
           lblPrice.AutoSize = true;
           lblPrice.Location = new System.Drawing.Point(539, 11);
           lblPrice.Name = "lblPrice";
           lblPrice.Size = new System.Drawing.Size(40, 13);
           lblPrice.TabIndex = 3;
           lblPrice.Text = "$" + p.TotalPrice;
            pFrame.Controls.Add(lblPrice);
            // 
            // lblBrand
            // 
            Label lblBrand = new Label();
            lblBrand.AutoSize = true;
            lblBrand.Location = new System.Drawing.Point(75, 31);
            lblBrand.Name = "lblBrand";
            lblBrand.Size = new System.Drawing.Size(34, 13);
            lblBrand.TabIndex = 2;
            lblBrand.Text = p.BrandName;
            pFrame.Controls.Add(lblBrand);
            // 
            // lblTitle
            // 
            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblTitle.Location = new System.Drawing.Point(72, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(40, 18);
            lblTitle.TabIndex = 1;
            lblTitle.Text = p.ProductName;
            pFrame.Controls.Add(lblTitle);
            // 
            // pbImage
            // 
            PictureBox pbImage = new PictureBox();
            pbImage.Dock = System.Windows.Forms.DockStyle.Left;
            if (File.Exists(imgPath))
            {
                pbImage.Image = System.Drawing.Image.FromFile(imgPath);
            }
            pbImage.Location = new System.Drawing.Point(5, 5);
            pbImage.Name = "pbImage";
            pbImage.Size = new System.Drawing.Size(57, 39);
            pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pbImage.TabIndex = 0;
            pbImage.TabStop = false;
            pFrame.Controls.Add(pbImage);

            return pFrame;
        }

        private Panel CreateCartListInven(InventoryDetail p)
        {
            string folder = Path.Combine(System.Windows.Forms.Application.StartupPath, "assets");
            string imgPath = Path.Combine(folder, p.ImageUrl);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            // lblQuantity
            // 
            Label lblQuantity = new Label();
            Label lblBrand = new Label();
            Label lblTitle = new Label();
            PictureBox pbImage = new PictureBox();
            Panel pFrame = new Panel();
            lblQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new System.Drawing.Point(150, 31);
            lblQuantity.Size = new System.Drawing.Size(40, 13);
            lblQuantity.TabIndex = 4;
            lblQuantity.Text = "x" + p.Quantity;
            // pFrame
            // 
            pFrame.BackColor = System.Drawing.Color.Transparent;
            //pFrame.Controls.Add(lblPrice);
            pFrame.Controls.Add(lblBrand);
            pFrame.Controls.Add(lblTitle);
            pFrame.Controls.Add(pbImage);
            pFrame.Controls.Add(lblQuantity);
            pFrame.Dock = System.Windows.Forms.DockStyle.Top;
            pFrame.Location = new System.Drawing.Point(10, 10);
            pFrame.Padding = new System.Windows.Forms.Padding(5);
            pFrame.Size = new System.Drawing.Size(586, 49);
            pFrame.TabIndex = 3;
            // 
            // lblPrice
            // 
            //lblPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //lblPrice.AutoSize = true;
            //lblPrice.Location = new System.Drawing.Point(539, 11);
            //lblPrice.Name = "lblPrice";
            //lblPrice.Size = new System.Drawing.Size(40, 13);
            //lblPrice.TabIndex = 3;
            //lblPrice.Text = "$" + p.TotalPrice;
            // 
            // lblBrand
            // 
            lblBrand.AutoSize = true;
            lblBrand.Location = new System.Drawing.Point(75, 31);
            lblBrand.Size = new System.Drawing.Size(34, 13);
            lblBrand.TabIndex = 2;
            lblBrand.Text = p.BrandName;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblTitle.Location = new System.Drawing.Point(72, 9);
            lblTitle.Size = new System.Drawing.Size(40, 18);
            lblTitle.TabIndex = 1;
            lblTitle.Text = p.ProductName;
            // 
            // pbImage
            // 
            pbImage.Dock = System.Windows.Forms.DockStyle.Left;
            if (File.Exists(imgPath))
            {
                pbImage.Image = System.Drawing.Image.FromFile(imgPath);
            }
            pbImage.Location = new System.Drawing.Point(5, 5);
            pbImage.Size = new System.Drawing.Size(57, 39);
            pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pbImage.TabIndex = 0;
            pbImage.TabStop = false;

            return pFrame;
        }

        private Panel CreateCartListPur(PurchaseDetail p)
        {
            string folder = Path.Combine(System.Windows.Forms.Application.StartupPath, "assets");
            string imgPath = Path.Combine(folder, p.ImageUrl);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            // lblQuantity
            // 
            Label lblQuantity = new Label();
            Label lblBrand = new Label();
            Label lblTitle = new Label();
            Label lblPrice = new Label();
            PictureBox pbImage = new PictureBox();
            Panel pFrame = new Panel();
            lblQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new System.Drawing.Point(150, 31);
            lblQuantity.Size = new System.Drawing.Size(40, 13);
            lblQuantity.TabIndex = 4;
            lblQuantity.Text = "x" + p.Quantity;
            // pFrame
            // 
            pFrame.BackColor = System.Drawing.Color.Transparent;
            pFrame.Controls.Add(lblPrice);
            pFrame.Controls.Add(lblBrand);
            pFrame.Controls.Add(lblTitle);
            pFrame.Controls.Add(pbImage);
            pFrame.Controls.Add(lblQuantity);
            pFrame.Dock = System.Windows.Forms.DockStyle.Top;
            pFrame.Location = new System.Drawing.Point(10, 10);
            pFrame.AutoSize = true;
            pFrame.Padding = new System.Windows.Forms.Padding(5);
            pFrame.Size = new System.Drawing.Size(586, 49);
            pFrame.TabIndex = 3;
            // 
            // lblPrice
            // 
            lblPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            lblPrice.AutoSize = true;
            lblPrice.Location = new System.Drawing.Point(520, 11);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new System.Drawing.Size(40, 13);
            lblPrice.TabIndex = 3;
            lblPrice.Text = "$" + p.SubTotal;
            // 
            // lblBrand
            // 
            lblBrand.AutoSize = true;
            lblBrand.Location = new System.Drawing.Point(75, 31);
            lblBrand.Size = new System.Drawing.Size(34, 13);
            lblBrand.TabIndex = 2;
            lblBrand.Text = p.BrandName;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblTitle.Location = new System.Drawing.Point(72, 9);
            lblTitle.Size = new System.Drawing.Size(40, 18);
            lblTitle.TabIndex = 1;
            lblTitle.Text = p.ProductName;
            // 
            // pbImage
            // 
            pbImage.Dock = System.Windows.Forms.DockStyle.Left;
            if (File.Exists(imgPath))
            {
                pbImage.Image = System.Drawing.Image.FromFile(imgPath);
            }
            pbImage.Location = new System.Drawing.Point(5, 5);
            pbImage.Size = new System.Drawing.Size(57, 39);
            pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pbImage.TabIndex = 0;
            pbImage.TabStop = false;

            return pFrame;
        }

        private void LoadPopSale(List<OrderDetail> product)
        {
            pPopSale.Controls.Clear();

            foreach (var p in product)
            {
                Panel panel = CreateCartList(p);
                pPopSale.Controls.Add(panel);
            }
        }

        private void LoadPopInventory(List<InventoryDetail> product)
        {
            pInven.Controls.Clear();

            foreach (var p in product)
            {
                Panel panel = CreateCartListInven(p);
                pInven.Controls.Add(panel);
            }
        }

        private void LoadPopPurchase(List<PurchaseDetail> product)
        {
            pPopPur.Controls.Clear();

            foreach (var p in product)
            {
                Panel panel = CreateCartListPur(p);
                pPopPur.Controls.Add(panel);
            }
        }
    }
}
