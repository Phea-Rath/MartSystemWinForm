using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    
    public partial class InventoryForm : Form
    {
        List<Product> products = new List<Product>()
        {
            new Product(1,"00001", "Product A", 1, 1, 1, 100.00, 1,23,34,"High quality",1),
            new Product(2,"00002", "Product B", 2, 2, 2, 200.00, 1,45,56,"Durable",1),
        };
        List<ProductInventory> selectedProducts = new List<ProductInventory>()
        {
            new ProductInventory(1, "Product A", 10, new DateTime(2025, 12, 31)),
            new ProductInventory(2, "Product B", 5, new DateTime(2026, 1, 15)),
        };
        public InventoryForm()
        {
            InitializeComponent();
            LoadData();
            cbProduct.DataSource = products;
            cbProduct.DisplayMember = "ProductName";
            cbProduct.ValueMember = "ProductId";
            cbProduct.SelectedIndex = 0;
            dgvItem.DataSource = selectedProducts;
        }

        private void LoadData()
        {
            List<Inventory> inventories = new List<Inventory>
            {
                new Inventory(1, "Stock in", 50, 10, 1, new DateTime(2025, 11, 1)),
                new Inventory(2, "Stock in", 30, 5, 2, new DateTime(2025, 12, 15)),
                new Inventory(3, "Stock in", 20, 2, 3, DateTime.Now)
            };
            dgvData.DataSource = inventories;
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQuan.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = (int)cbProduct.SelectedValue;
            DateTime expiryDate = dtpExpire.Value;

            var selectedProduct = products.FirstOrDefault(p => p.ProductId == productId);
            if (selectedProduct == null)
            {
                MessageBox.Show("Selected product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProductInventory productInventory = new ProductInventory(productId, selectedProduct.ProductName, quantity, expiryDate);
            selectedProducts.Add(productInventory);

            dgvItem.DataSource = null;           
            dgvItem.DataSource = selectedProducts; 
        }

    }
}
