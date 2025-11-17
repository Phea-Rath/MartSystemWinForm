namespace MartManagementSystem
{
    partial class Invoice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblOrderId = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flpItem = new System.Windows.Forms.FlowLayoutPanel();
            this.tlpList = new System.Windows.Forms.TableLayoutPanel();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSup = new System.Windows.Forms.Label();
            this.lblSupPrice = new System.Windows.Forms.Label();
            this.lblDis = new System.Windows.Forms.Label();
            this.lblDisPrice = new System.Windows.Forms.Label();
            this.lblQuan = new System.Windows.Forms.Label();
            this.lblPmValue = new System.Windows.Forms.Label();
            this.lblPM = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalPrice = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flpItem.SuspendLayout();
            this.tlpList.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(140, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Invoice";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOrderId
            // 
            this.lblOrderId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOrderId.AutoSize = true;
            this.lblOrderId.Location = new System.Drawing.Point(140, 34);
            this.lblOrderId.Name = "lblOrderId";
            this.lblOrderId.Size = new System.Drawing.Size(87, 13);
            this.lblOrderId.TabIndex = 0;
            this.lblOrderId.Text = "OrderID: #10002";
            this.lblOrderId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblOrderId);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(368, 65);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblSup, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSupPrice, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDis, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDisPrice, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblPmValue, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblPM, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTotal, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalPrice, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 483);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(368, 125);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // flpItem
            // 
            this.flpItem.Controls.Add(this.tlpList);
            this.flpItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpItem.Location = new System.Drawing.Point(0, 65);
            this.flpItem.Name = "flpItem";
            this.flpItem.Padding = new System.Windows.Forms.Padding(5);
            this.flpItem.Size = new System.Drawing.Size(368, 418);
            this.flpItem.TabIndex = 3;
            // 
            // tlpList
            // 
            this.tlpList.ColumnCount = 2;
            this.tlpList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlpList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpList.Controls.Add(this.lblPrice, 1, 0);
            this.tlpList.Controls.Add(this.lblTitle, 0, 0);
            this.tlpList.Controls.Add(this.lblQuan, 0, 1);
            this.tlpList.Location = new System.Drawing.Point(8, 8);
            this.tlpList.Name = "tlpList";
            this.tlpList.RowCount = 2;
            this.tlpList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tlpList.Size = new System.Drawing.Size(352, 41);
            this.tlpList.TabIndex = 3;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPrice.Location = new System.Drawing.Point(249, 0);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(100, 28);
            this.lblPrice.TabIndex = 0;
            this.lblPrice.Text = "label3";
            this.lblPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(240, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "label3";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSup
            // 
            this.lblSup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSup.AutoSize = true;
            this.lblSup.Location = new System.Drawing.Point(8, 5);
            this.lblSup.Name = "lblSup";
            this.lblSup.Size = new System.Drawing.Size(173, 13);
            this.lblSup.TabIndex = 0;
            this.lblSup.Text = "SupTotal";
            // 
            // lblSupPrice
            // 
            this.lblSupPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSupPrice.AutoSize = true;
            this.lblSupPrice.Location = new System.Drawing.Point(187, 13);
            this.lblSupPrice.Name = "lblSupPrice";
            this.lblSupPrice.Size = new System.Drawing.Size(173, 13);
            this.lblSupPrice.TabIndex = 0;
            this.lblSupPrice.Text = "$123.00";
            this.lblSupPrice.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDis
            // 
            this.lblDis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDis.AutoSize = true;
            this.lblDis.Location = new System.Drawing.Point(8, 35);
            this.lblDis.Name = "lblDis";
            this.lblDis.Size = new System.Drawing.Size(173, 13);
            this.lblDis.TabIndex = 0;
            this.lblDis.Text = "Discount";
            // 
            // lblDisPrice
            // 
            this.lblDisPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDisPrice.AutoSize = true;
            this.lblDisPrice.Location = new System.Drawing.Point(187, 43);
            this.lblDisPrice.Name = "lblDisPrice";
            this.lblDisPrice.Size = new System.Drawing.Size(173, 13);
            this.lblDisPrice.TabIndex = 1;
            this.lblDisPrice.Text = "$2.00";
            this.lblDisPrice.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblQuan
            // 
            this.lblQuan.AutoSize = true;
            this.lblQuan.Location = new System.Drawing.Point(3, 28);
            this.lblQuan.Name = "lblQuan";
            this.lblQuan.Size = new System.Drawing.Size(24, 13);
            this.lblQuan.TabIndex = 1;
            this.lblQuan.Text = "x10";
            // 
            // lblPmValue
            // 
            this.lblPmValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPmValue.AutoSize = true;
            this.lblPmValue.Location = new System.Drawing.Point(187, 73);
            this.lblPmValue.Name = "lblPmValue";
            this.lblPmValue.Size = new System.Drawing.Size(173, 13);
            this.lblPmValue.TabIndex = 2;
            this.lblPmValue.Text = "Bank";
            this.lblPmValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblPM
            // 
            this.lblPM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPM.AutoSize = true;
            this.lblPM.Location = new System.Drawing.Point(8, 65);
            this.lblPM.Name = "lblPM";
            this.lblPM.Size = new System.Drawing.Size(173, 13);
            this.lblPM.TabIndex = 2;
            this.lblPM.Text = "Payment Method";
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(8, 95);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(173, 13);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "Total";
            // 
            // lblTotalPrice
            // 
            this.lblTotalPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalPrice.AutoSize = true;
            this.lblTotalPrice.Location = new System.Drawing.Point(187, 103);
            this.lblTotalPrice.Name = "lblTotalPrice";
            this.lblTotalPrice.Size = new System.Drawing.Size(173, 13);
            this.lblTotalPrice.TabIndex = 2;
            this.lblTotalPrice.Text = "$345.00";
            this.lblTotalPrice.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Invoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 608);
            this.Controls.Add(this.flpItem);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Name = "Invoice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invoice";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flpItem.ResumeLayout(false);
            this.tlpList.ResumeLayout(false);
            this.tlpList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOrderId;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblSup;
        private System.Windows.Forms.Label lblSupPrice;
        private System.Windows.Forms.Label lblDis;
        private System.Windows.Forms.Label lblDisPrice;
        private System.Windows.Forms.FlowLayoutPanel flpItem;
        private System.Windows.Forms.TableLayoutPanel tlpList;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblQuan;
        private System.Windows.Forms.Label lblPmValue;
        private System.Windows.Forms.Label lblPM;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalPrice;
    }
}