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

namespace MartManagementSystem
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
            LoadData();
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
            string query = "SELECT SUM(total_price) AS total FROM Orders WHERE is_deleted = 0";
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                decimal res = Convert.ToDecimal(cmd.ExecuteScalar());
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
            string query = @"SELECT SUM(sd.quantity - od.quantity) AS total FROM Stock_details as sd
                            JOIN Order_details as od ON od.item_id = sd.item_id
                            WHERE sd.is_deleted = 0";
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
                MessageBox.Show("Error: " + ex.Message, "Error TotalOfInventory");
                return 0;
            }
        }

        private decimal TotalOfPurchase()
        {
            SqlConnection conn = SqlServerConnection.GetConnection();
            string query = "SELECT SUM(total) AS total FROM Purchases WHERE is_deleted = 0";
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
            string query = @"SELECT TOP 3 i.item_name,  od.item_id, i.image, SUM(od.quantity) AS total_quantity,SUM(od.total_price), b.brand_name 
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
            string query = @"SELECT TOP 3 i.item_name,  od.item_id, i.image, SUM(od.quantity) AS total_quantity, b.brand_name 
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
            string query = @"SELECT TOP 3 i.item_name,  od.item_id, i.image, SUM(od.quantity) AS total_quantity, SUM(od.total) AS total, b.brand_name 
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
            // lblQuantity
            // 
            Label lblQuantity = new Label();
            lblQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new System.Drawing.Point(539, 31);
            lblQuantity.Size = new System.Drawing.Size(40, 13);
            lblQuantity.TabIndex = 4;
            lblQuantity.Text = "x" + p.Quantity;
            // pFrame
            // 
            this.pFrame.BackColor = System.Drawing.Color.Transparent;
            this.pFrame.Controls.Add(this.lblPrice);
            this.pFrame.Controls.Add(this.lblBrand);
            this.pFrame.Controls.Add(this.lblTitle);
            this.pFrame.Controls.Add(this.pbImage);
            this.pFrame.Controls.Add(lblQuantity);
            this.pFrame.Dock = System.Windows.Forms.DockStyle.Top;
            this.pFrame.Location = new System.Drawing.Point(10, 10);
            this.pFrame.Name = "pFrame";
            this.pFrame.Padding = new System.Windows.Forms.Padding(5);
            this.pFrame.Size = new System.Drawing.Size(586, 49);
            this.pFrame.TabIndex = 3;
            // 
            // lblPrice
            // 
            this.lblPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(539, 11);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(40, 13);
            this.lblPrice.TabIndex = 3;
            this.lblPrice.Text = "$" + p.TotalPrice;
            // 
            // lblBrand
            // 
            this.lblBrand.AutoSize = true;
            this.lblBrand.Location = new System.Drawing.Point(75, 31);
            this.lblBrand.Name = "lblBrand";
            this.lblBrand.Size = new System.Drawing.Size(34, 13);
            this.lblBrand.TabIndex = 2;
            this.lblBrand.Text = p.BrandName;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(72, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(40, 18);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = p.ProductName;
            // 
            // pbImage
            // 
            this.pbImage.Dock = System.Windows.Forms.DockStyle.Left;
            if (File.Exists(imgPath))
            {
                this.pbImage.Image = System.Drawing.Image.FromFile(imgPath);
            }
            this.pbImage.Location = new System.Drawing.Point(5, 5);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(57, 39);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;

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
