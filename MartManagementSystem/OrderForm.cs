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
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace MartManagementSystem
{
    public partial class OrderForm : Form
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderForm));
        
        public event EventHandler handleOrderList;
        PurchaseForm _pur;
        InventoryForm _inv;
        public OrderForm(PurchaseForm pur,InventoryForm inv)
        {
            InitializeComponent();
            this._pur = pur;
            this._inv = inv;
            InventoryDetail.GetSaleItems();
            LoadProducts(InventoryDetail.saleList);
            LoadOrderItems(OrderDetail.orderItems);
            LoadCategory(Category.CategoryList);
            this.Shown += (s, e) =>
            {
                InventoryDetail.GetSaleItems();
                LoadProducts(InventoryDetail.saleList);
                LoadOrderItems(OrderDetail.orderItems);
                LoadCategory(Category.CategoryList);
            }; 
            this.FormClosed += (s, e) =>
            {
                InventoryDetail.GetSaleItems();
                LoadProducts(InventoryDetail.saleList);
                LoadOrderItems(OrderDetail.orderItems);
                LoadCategory(Category.CategoryList);
            }; 
            this.FormClosing += (s, e) =>
            {
                InventoryDetail.GetSaleItems();
                LoadProducts(InventoryDetail.saleList);
                LoadOrderItems(OrderDetail.orderItems);
                LoadCategory(Category.CategoryList);
            };
            _pur.PurchaseChanged += (s, e) =>
            {
                InventoryDetail.GetSaleItems();
                LoadProducts(InventoryDetail.saleList);
                LoadOrderItems(OrderDetail.orderItems);
                LoadCategory(Category.CategoryList);
            };
            _inv.InventoriesChanged += (s, e) =>
            {
                InventoryDetail.GetSaleItems();
                LoadProducts(InventoryDetail.saleList);
                LoadOrderItems(OrderDetail.orderItems);
                LoadCategory(Category.CategoryList);
            };

        }
        private TableLayoutPanel CreateListOrder(OrderDetail p)
        {
            // Safe image load
            string folder = Path.Combine(System.Windows.Forms.Application.StartupPath, "assets");
            string imgFile = string.IsNullOrEmpty(p.ImageUrl) ? "" : p.ImageUrl;
            string imgPath = Path.Combine(folder, imgFile);
            decimal discount = p.Discount / 100;
            decimal disPrice = discount > 0 ? discount * p.UnitPrice : 0;
            decimal amount = p.UnitPrice - disPrice;
            decimal total = amount * p.Quantity;

            // 
            // label88
            // 
            Label lblQuan = new Label();
            lblQuan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            lblQuan.AutoSize = true;
            lblQuan.ForeColor = System.Drawing.Color.Red;
            lblQuan.Location = new System.Drawing.Point(160, 25);
            lblQuan.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            lblQuan.Size = new System.Drawing.Size(46, 13);
            lblQuan.TabIndex = 4;
            lblQuan.Text = "x"+p.Quantity.ToString();
            

            Label lblTotal = new Label();
            lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            lblTotal.AutoSize = true;
            lblTotal.ForeColor = System.Drawing.Color.Red;
            lblTotal.Location = new System.Drawing.Point(140, 1);
            lblTotal.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            lblTotal.Size = new System.Drawing.Size(46, 13);
            lblTotal.TabIndex = 2;
            lblTotal.Text = "$"+total.ToString("N2");
            // 
            // label91

            Panel pframe = new Panel();
            Label lblCost = new Label();
            Label lblDis = new Label();
            if (p.Discount > 0)
            {
                lblDis.AutoSize = true;
                lblDis.BackColor = System.Drawing.Color.Transparent;
                lblDis.ForeColor = System.Drawing.Color.Red;
                lblDis.Location = new System.Drawing.Point(0, 1);
                lblDis.Size = new System.Drawing.Size(30, 13);
                lblDis.TabIndex = 2;
                lblDis.Text = "-"+p.Discount.ToString("N0")+"%";

                lblCost.AutoSize = true;
                lblCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Strikeout))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblCost.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                lblCost.Location = new System.Drawing.Point(95, 25);
                lblCost.Size = new System.Drawing.Size(40, 13);
                lblCost.TabIndex = 3;
                lblCost.Text = "$" + p.UnitPrice;

                pframe.Controls.Add(lblCost);
                pframe.Controls.Add(lblDis);
            }
            // 
            // label92
            // 
            Label lblPrice = new Label();
            lblPrice.AutoSize = true;
            lblPrice.ForeColor = System.Drawing.Color.Red;
            lblPrice.Location = new System.Drawing.Point(48, 24);
            lblPrice.Size = new System.Drawing.Size(40, 13);
            lblPrice.TabIndex = 1;
            lblPrice.Text = "$"+amount.ToString("N2");
            // 
            // label93
            // 
            Label lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblTitle.Location = new System.Drawing.Point(48, 4);
            lblTitle.Size = new System.Drawing.Size(35, 15);
            lblTitle.TabIndex = 1;
            lblTitle.Text = p.ProductName.ToString();
            // 
            // pictureBox15
            // 
            PictureBox pbImage = new PictureBox();
            //pbImage.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox15.Image")));
            if (File.Exists(imgPath))
                pbImage.Image = System.Drawing.Image.FromFile(imgPath);
            else
                pbImage.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox15.Image")));
            pbImage.Dock = DockStyle.Left;
            pbImage.Size = new Size(42, 41);
            pbImage.SizeMode = PictureBoxSizeMode.Zoom;
            //pbImage.TabIndex = 0;
            //pbImage.TabStop = false;
            // 
            // button23
            // 
            Button btnRemove = new Button();
            btnRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("button23.Image")));
            btnRemove.Location = new System.Drawing.Point(254, 3);
            btnRemove.Size = new System.Drawing.Size(29, 41);
            btnRemove.TabIndex = 1;
            btnRemove.UseVisualStyleBackColor = true;

            // Frame item
            // 
            pframe.BackColor = System.Drawing.Color.Transparent;
            pframe.Controls.Add(lblTotal);
            pframe.Controls.Add(lblPrice);
            pframe.Controls.Add(lblTitle);
            pframe.Controls.Add(pbImage);
            pframe.Controls.Add(lblQuan);
            pframe.Dock = System.Windows.Forms.DockStyle.Fill;
            pframe.Location = new System.Drawing.Point(3, 3);
            pframe.Size = new System.Drawing.Size(245, 41);
            pframe.TabIndex = 0;

            //Layout
            TableLayoutPanel tlpOrder = new TableLayoutPanel();
            tlpOrder.BackColor = System.Drawing.SystemColors.Control;
            tlpOrder.ColumnCount = 2;
            tlpOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.76224F));
            tlpOrder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.23776F));
            tlpOrder.Controls.Add(pframe, 0, 0);
            tlpOrder.Controls.Add(btnRemove, 1, 0);
            tlpOrder.Location = new System.Drawing.Point(8, 8);
            tlpOrder.RowCount = 1;
            tlpOrder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpOrder.Size = new System.Drawing.Size(286, 47);
            tlpOrder.TabIndex = 0;
            // 

            btnRemove.Click += (s, e) =>
            {
                OrderDetail.orderItems = OrderDetail.orderItems.Where((i)=>i.ProductId != p.ProductId).ToList();
                OrderChange();
                LoadOrderItems(OrderDetail.orderItems);
            };
            return tlpOrder;
        }
        private Button CreateCategory(Category b)
        {
            Button btnCat = new Button();
            btnCat.Size = new Size(75, 23);
            btnCat.UseVisualStyleBackColor = true;
            btnCat.Text = b.CategoryName;
            btnCat.Click += (s, e) =>
            {
                // Action
                if (string.IsNullOrEmpty(b.CategoryName))
                {
                    // Show all

                    LoadProducts(InventoryDetail.saleList);
                }
                else
                {
                    // Filter list (case-insensitive, partial match)
                    var newList = InventoryDetail.saleList
                        .Where(p => !string.IsNullOrEmpty(p.CategoryName) &&
                                    p.CategoryName.IndexOf(b.CategoryName, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();


                    LoadProducts(newList);
                }
            };
            return btnCat;
        }
        private Panel CreateProductCard(InventoryDetail p)
        {
            string folder = Path.Combine(System.Windows.Forms.Application.StartupPath, "assets");
            string imgPath = Path.Combine(folder, p.ImageUrl);
            decimal discount = p.Discount / 100;
            decimal disPrice = discount > 0 ? discount * p.UnitPrice : 0;
            decimal amount = p.UnitPrice - disPrice;


            Panel card = new Panel();
            card.Size = new Size(180, 220);
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Margin = new Padding(10);

            //Discount
            if (p.Discount > 0)
            {
                Label lblDis = new Label();
                lblDis.AutoSize = true;
                lblDis.BackColor = Color.Red;
                lblDis.Padding = new Padding(1, 1, 1, 1);
                lblDis.ForeColor = System.Drawing.Color.White;
                lblDis.Location = new System.Drawing.Point(0, 3);
                lblDis.Size = new System.Drawing.Size(30, 13);
                lblDis.Text = "- "+p.Discount.ToString("N0")+"%";
                card.Controls.Add(lblDis);


                Label lblDisPrice = new Label();
                lblDisPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                lblDisPrice.AutoSize = true;
                lblDisPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Strikeout))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblDisPrice.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                lblDisPrice.Location = new System.Drawing.Point(53, 200);
                lblDisPrice.Size = new System.Drawing.Size(40, 13);
                lblDisPrice.Text = "$" + p.UnitPrice.ToString("N2");
                card.Controls.Add(lblDisPrice);
            }

            // Product Image
            PictureBox pic = new PictureBox();
            pic.Size = new Size(160, 120);
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.Location = new Point(10, 10);
            if (File.Exists(imgPath))
            {
                pic.Image = System.Drawing.Image.FromFile(imgPath);
            }
            //pic.Image = p.ImageUrl; // Assuming p.Image is Image type
            card.Controls.Add(pic);
            

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = p.ProductName;
            lblTitle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            lblTitle.Location = new Point(10, 140);
            lblTitle.AutoSize = true;
            card.Controls.Add(lblTitle);

            // Brand
            Label lblBrand = new Label();
            lblBrand.Text = p.BrandName;
            lblBrand.Location = new Point(10, 160);
            lblBrand.AutoSize = true;
            card.Controls.Add(lblBrand);

            //Size
            Label lblSize = new Label();
            lblSize.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            lblSize.AutoSize = true;
            lblSize.Location = new Point(125, 140);
            lblSize.RightToLeft = RightToLeft.Yes;
            lblSize.Text = p.SizeName;
            lblSize.TextAlign = ContentAlignment.MiddleRight;
            card.Controls.Add(lblSize);

            // Category
            Label lblCategory = new Label();
            lblCategory.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(125, 160);
            lblCategory.RightToLeft = RightToLeft.Yes;
            lblCategory.Text = p.CategoryName;
            lblCategory.TextAlign = ContentAlignment.MiddleRight;
            card.Controls.Add(lblCategory);

            // Price
            Label lblPrice = new Label();
            lblPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            lblPrice.AutoSize = true;
            lblPrice.ForeColor = System.Drawing.Color.Red;
            lblPrice.Location = new Point(7, 200);
            lblPrice.Size = new System.Drawing.Size(40, 13);
            lblPrice.Text = "$" + amount.ToString("N2");
            card.Controls.Add(lblPrice);

            // Add to cart btn
            Button btnAdd = new Button();
            btnAdd.Size = new Size(35, 25);
            btnAdd.Location = new Point(135, 185);
            btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("button8.Image")));
            btnAdd.Click += (s, e) =>
            {
                var item = OrderDetail.orderItems.FirstOrDefault(i => i.ProductId == p.ProductId);

                // Correct discount formula
                //decimal discountRate = p.Discount / 100m;
                if(p.Quantity == 0){
                    MessageBox.Show("Product is not have!", "Inventory Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                decimal supAmount = p.UnitPrice * p.Quantity;
                if (item != null)
                {
                    if(item.Quantity == p.Quantity)
                    {
                        MessageBox.Show("Product is not have!","Inventory Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return;
                    }
                    // item exists → update quantity and total price
                    item.Quantity++;
                    item.TotalPrice = item.Quantity * amount;
                }
                else
                {
                    // item not exists → add new
                    OrderDetail.orderItems.Add(new OrderDetail()
                    {
                        ProductId = p.ProductId,
                        ProductName = p.ProductName,
                        UnitPrice = p.UnitPrice,
                        Discount = p.Discount,
                        Quantity = 1,
                        TotalPrice = amount,
                        SupPrice = supAmount,
                        ImageUrl = p.ImageUrl
                    });

                }
                    OrderChange();

                // Refresh UI
                LoadOrderItems(OrderDetail.orderItems);
            };
            card.Controls.Add(btnAdd);

            return card;
        }

        private void OrderChange()
        {
            decimal supTotal = OrderDetail.orderItems.Sum(i => i.TotalPrice);
            decimal disTotal = OrderDetail.orderItems.Sum(i => (i.Discount / 100) * i.UnitPrice);
            lblSup.Text = supTotal.ToString("N2");
            lblDisPrice.Text = disTotal.ToString("N2");
            lblTotalPrice.Text = (supTotal - disTotal).ToString("N2");
        }

        private void LoadProducts(List<InventoryDetail> products)
        {
            flpCards.Controls.Clear();

            foreach (var p in products)
            {
                Panel card = CreateProductCard(p);
                flpCards.Controls.Add(card);
            }
        }

        private void LoadCategory(List<Category> category)
        {
            flpCat.Controls.Clear();

            foreach (var c in category)
            {
                Button btn = CreateCategory(c);
                flpCat.Controls.Add(btn);
            }
        }


        private void LoadOrderItems(List<OrderDetail> product)
        {
            flpListOrd.Controls.Clear();

            foreach (var p in product)
            {
                TableLayoutPanel btn = CreateListOrder(p);
                flpListOrd.Controls.Add(btn);
            }
        }

        public void activeForm(Form form, List<Form> formList)
        {
            foreach (Form btn in formList)
            {
                if (btn == form)
                {
                    btn.Show();
                }
                else
                {
                    btn.Hide();
                }
            }
        }
        private void btnOrderList_Click(object sender, EventArgs e)
        {

            handleOrderList?.Invoke(this, EventArgs.Empty);
        }

        private void setField()
        {
            OrderDetail.orderItems = new List<OrderDetail>();
            LoadOrderItems(OrderDetail.orderItems);
            lblDisPrice.Text = "$0.00";
            lblSup.Text = "$0.00";
            lblTotalPrice.Text = "$0.00";
            cbPayMeth.Text = "";
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (OrderDetail.orderItems.Count <= 0) return;
            var order = new Order()
            {
                SubTotalPrice = Convert.ToDecimal(lblSup.Text),
                Discount = Convert.ToDecimal(lblDisPrice.Text),
                TotalPrice = Convert.ToDecimal(lblTotalPrice.Text),
                PaymentMethod = cbPayMeth.Text
            };
            bool res = Order.InsertOrder(order, OrderDetail.orderItems);
            if (res)
            {
                setField();
                handleOrderList?.Invoke(this, EventArgs.Empty);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            setField();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string find = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(find))
            {
                // Show all

                LoadProducts(InventoryDetail.saleList);
            }
            else
            {
                // Filter list (case-insensitive, partial match)
                var newList = InventoryDetail.saleList
                    .Where(p => !string.IsNullOrEmpty(p.ProductName) &&
                                p.ProductName.IndexOf(find, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();


                LoadProducts(newList);
            }
        }
    }
}
