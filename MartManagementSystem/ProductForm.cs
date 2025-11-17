using MartManagementSystem.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace MartManagementSystem
{

    public partial class ProductForm : Form
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductForm));
        Product product = new Product();
        Category category = new Category();
        Brand brand = new Brand();
        Sizes size = new Sizes();

        CategoryForm _cat;
        BrandForm _bra;
        SizeForm _size;
        public event EventHandler ProductChanged;

        public ProductForm(CategoryForm cat, BrandForm bra, SizeForm siz)
        {
            InitializeComponent();
            _cat = cat;
            _bra = bra;
            _size = siz;
            dgvData.RowPostPaint += dgvData_RowPostPaint;
            LoadData();
            LoadComboboxes();
            _cat.CategoryChanged += (s, e) => LoadComboboxes();
            _bra.BrandChanged += (s, e) => LoadComboboxes();
            _size.SizeChanged += (s, e) => LoadComboboxes();
        }

        private void setField()
        {
            txtId.Text = string.Empty;
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            txtDis.Text = string.Empty;
            rtDes.Text = string.Empty;
            txtImageUrl.Text = string.Empty;

            cbCategory.SelectedIndex = -1;
            cbBrand.SelectedIndex = -1;
            cbSize.SelectedIndex = -1;
            pbImage.Image = ((System.Drawing.Image)(resources.GetObject("pbImage.Image")));
        }

        private void dgvData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (dgvData.Columns.Contains("No"))
                dgvData.Rows[e.RowIndex].Cells["No"].Value = (e.RowIndex + 1).ToString();
        }

        private void LoadData()
        {
            product.GetAllProduct();
            dgvData.DataSource = null;
            dgvData.DataSource = Product.ProductList;

            if (!dgvData.Columns.Contains("No"))
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn
                {
                    Name = "No",
                    HeaderText = "#",
                    Width = 50
                };
                dgvData.Columns.Insert(0, col);
            }

            dgvData.Columns["ProductId"].Visible = false;
            dgvData.Columns["CategoryId"].Visible = false;
            dgvData.Columns["BrandId"].Visible = false;
            dgvData.Columns["SizeId"].Visible = false;
            dgvData.Columns["CreatedBy"].Visible = false;
            //dgvData.Columns["CreatedAt"].Visible = false;
            //dgvData.Columns["UpdatedAt"].Visible = false;
            //dgvData.Columns["ImageUrl"].Visible = false;
            dgvData.Columns["CostPrice"].Visible = false;
            dgvData.Columns["IsDeleted"].Visible = false;

            dgvData.Refresh();
            ProductChanged?.Invoke(this, EventArgs.Empty);
        }

        private void LoadComboboxes()
        {
            category.GetAllCategory();
            cbCategory.DataSource = Category.CategoryList;
            cbCategory.DisplayMember = "CategoryName";
            cbCategory.ValueMember = "CategoryId";
            cbCategory.SelectedIndex = -1;

            brand.GetAllBrand();
            cbBrand.DataSource = Brand.BrandList;
            cbBrand.DisplayMember = "BrandName";
            cbBrand.ValueMember = "BrandId";
            cbBrand.SelectedIndex = -1;

            size.GetAllSizes();
            cbSize.DataSource = Sizes.SizeList;
            cbSize.DisplayMember = "SizeName";
            cbSize.ValueMember = "SizeId";
            cbSize.SelectedIndex = -1;
        }

        private string SaveImageToAssets(string originalPath)
        {
            // Check if path is valid
            if (string.IsNullOrEmpty(originalPath) || !File.Exists(originalPath))
                return null;

            try
            {
                // Create assets folder if it doesn't exist
                string folder = Path.Combine(Application.StartupPath, "assets");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // Generate unique file name with same extension
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(originalPath);
                string destPath = Path.Combine(folder, fileName);

                // Copy file
                File.Copy(originalPath, destPath, true);

                return fileName; // Return saved path
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error saving image: " + ex.Message);
                return null;
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Select Product Image";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pbImage.Image = Image.FromFile(ofd.FileName);
                    pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                    txtImageUrl.Text = ofd.FileName;
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (User.UserLogin == null || User.UserLogin.Count == 0)
            {
                MessageBox.Show("⚠ No user logged in. Please login first.");
                return;
            }

            Product p = new Product
            {
                ProductCode = Guid.NewGuid().ToString(),
                ProductName = txtName.Text,
                CategoryId = cbCategory.SelectedValue as int?,
                BrandId = cbBrand.SelectedValue as int?,
                SizeId = cbSize.SelectedValue as int?,
                UnitPrice = decimal.TryParse(txtUnitPrice.Text, out var up) ? up : 0,
                Discount = decimal.TryParse(txtDis.Text, out var d) ? d : 0,
                Description = rtDes.Text,
                CreatedBy = User.UserLogin[0].UserId,
                ImageUrl = SaveImageToAssets(txtImageUrl.Text)
            };

            bool result = product.InsertProduct(p);
            if (result)
            {
                setField();
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0 || dgvData.CurrentRow == null) return;

            txtId.Text = dgvData.CurrentRow.Cells["ProductId"].Value?.ToString() ?? "";
            txtCode.Text = dgvData.CurrentRow.Cells["ProductCode"].Value?.ToString() ?? "";
            txtName.Text = dgvData.CurrentRow.Cells["ProductName"].Value?.ToString() ?? "";
            txtUnitPrice.Text = dgvData.CurrentRow.Cells["UnitPrice"].Value?.ToString() ?? "";
            txtDis.Text = dgvData.CurrentRow.Cells["Discount"].Value?.ToString() ?? "";
            rtDes.Text = dgvData.CurrentRow.Cells["Description"].Value?.ToString() ?? "";

            cbCategory.SelectedValue = dgvData.CurrentRow.Cells["CategoryId"].Value ?? -1;
            cbBrand.SelectedValue = dgvData.CurrentRow.Cells["BrandId"].Value ?? -1;
            cbSize.SelectedValue = dgvData.CurrentRow.Cells["SizeId"].Value ?? -1;
            string folder = Path.Combine(Application.StartupPath, "assets");
            string imgPath = Path.Combine(folder, dgvData.CurrentRow.Cells["ImageUrl"].Value?.ToString());
            if (!string.IsNullOrEmpty(imgPath) && System.IO.File.Exists(imgPath))
            {
                pbImage.Image = Image.FromFile(imgPath);
                pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                txtImageUrl.Text = imgPath;
            }
            else
            {
                pbImage.Image = null;
                txtImageUrl.Text = string.Empty;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text)) { MessageBox.Show("Please select a product to update."); return; }

            Product p = new Product
            {
                ProductId = int.Parse(txtId.Text),
                ProductCode = txtCode.Text,
                ProductName = txtName.Text,
                CategoryId = cbCategory.SelectedValue as int?,
                BrandId = cbBrand.SelectedValue as int?,
                SizeId = cbSize.SelectedValue as int?,
                UnitPrice = decimal.TryParse(txtUnitPrice.Text, out var up) ? up : 0,
                Discount = decimal.TryParse(txtDis.Text, out var d) ? d : 0,
                Description = rtDes.Text,
                ImageUrl = SaveImageToAssets(txtImageUrl.Text)
            };

            bool result = product.UpdateProduct(p);
            if (result)
            {
                setField();
                LoadData();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count <= 0 || dgvData.CurrentRow == null) return;

            int id = int.Parse(dgvData.CurrentRow.Cells["ProductId"].Value?.ToString() ?? "0");
            DialogResult result = MessageBox.Show("Are you sure you want to delete this product?", "Delete Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                bool del = product.DeleteProduct(id);
                if (del)
                {
                    setField();
                    LoadData();
                }
            }
        }
    

    private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string find = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(find))
            {
                // Show all
                dgvData.DataSource = Product.ProductList;
            }
            else
            {
                // Filter list (case-insensitive, partial match)
                var newList = Product.ProductList
                    .Where(p => !string.IsNullOrEmpty(p.ProductName) &&
                                p.ProductName.IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                dgvData.DataSource = newList;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            setField();
        }
    }
}
