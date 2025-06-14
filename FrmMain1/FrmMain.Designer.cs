using System.Drawing;

namespace FrmMain
{
    partial class FrmMain
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
            ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            btnOrder = new DevExpress.XtraBars.BarSubItem();
            ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            pnlRight = new DevExpress.XtraEditors.PanelControl();
            panelControl2 = new DevExpress.XtraEditors.PanelControl();
            labelControl13 = new DevExpress.XtraEditors.LabelControl();
            lblBranchName = new DevExpress.XtraEditors.LabelControl();
            labelControl12 = new DevExpress.XtraEditors.LabelControl();
            txtKhachThanhToan = new DevExpress.XtraEditors.TextEdit();
            labelControl11 = new DevExpress.XtraEditors.LabelControl();
            txtKhachCanTra = new DevExpress.XtraEditors.TextEdit();
            labelControl10 = new DevExpress.XtraEditors.LabelControl();
            txtThuKhac = new DevExpress.XtraEditors.TextEdit();
            labelControl9 = new DevExpress.XtraEditors.LabelControl();
            txtDiscount = new DevExpress.XtraEditors.TextEdit();
            txtTotal = new DevExpress.XtraEditors.TextEdit();
            labelControl8 = new DevExpress.XtraEditors.LabelControl();
            labelControl7 = new DevExpress.XtraEditors.LabelControl();
            labelControl6 = new DevExpress.XtraEditors.LabelControl();
            txtBuyer = new DevExpress.XtraEditors.TextEdit();
            labelControl5 = new DevExpress.XtraEditors.LabelControl();
            txtSale = new DevExpress.XtraEditors.TextEdit();
            labelControl4 = new DevExpress.XtraEditors.LabelControl();
            labelControl3 = new DevExpress.XtraEditors.LabelControl();
            pnlOrder = new DevExpress.XtraEditors.PanelControl();
            panelControl1 = new DevExpress.XtraEditors.PanelControl();
            grdOrder = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            pnlControl = new DevExpress.XtraEditors.PanelControl();
            drpOrders = new DevExpress.XtraEditors.DropDownButton();
            btnGetOrder = new DevExpress.XtraEditors.SimpleButton();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            txtProductCode = new DevExpress.XtraEditors.TextEdit();
            txtOrderCode = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)ribbon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pnlRight).BeginInit();
            pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelControl2).BeginInit();
            panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtKhachThanhToan.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtKhachCanTra.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtThuKhac.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtDiscount.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtTotal.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtBuyer.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtSale.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pnlOrder).BeginInit();
            pnlOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelControl1).BeginInit();
            panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdOrder).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pnlControl).BeginInit();
            pnlControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtProductCode.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtOrderCode.Properties).BeginInit();
            SuspendLayout();
            // 
            // ribbon
            // 
            ribbon.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(50, 53, 50, 53);
            ribbon.ExpandCollapseItem.Id = 0;
            ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] { ribbon.ExpandCollapseItem, btnOrder });
            ribbon.Location = new Point(0, 0);
            ribbon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ribbon.MaxItemId = 2;
            ribbon.Name = "ribbon";
            ribbon.OptionsMenuMinWidth = 550;
            ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { ribbonPage1 });
            ribbon.Size = new Size(1938, 158);
            ribbon.StatusBar = ribbonStatusBar;
            // 
            // btnOrder
            // 
            btnOrder.Caption = "Đơn hàng";
            btnOrder.Id = 1;
            btnOrder.Name = "btnOrder";
            // 
            // ribbonPage1
            // 
            ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroup1 });
            ribbonPage1.Name = "ribbonPage1";
            ribbonPage1.Text = "Home";
            // 
            // ribbonPageGroup1
            // 
            ribbonPageGroup1.ItemLinks.Add(btnOrder);
            ribbonPageGroup1.Name = "ribbonPageGroup1";
            ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // ribbonStatusBar
            // 
            ribbonStatusBar.Location = new Point(0, 1075);
            ribbonStatusBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            ribbonStatusBar.Name = "ribbonStatusBar";
            ribbonStatusBar.Ribbon = ribbon;
            ribbonStatusBar.Size = new Size(1938, 24);
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(panelControl2);
            pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            pnlRight.Location = new Point(1438, 158);
            pnlRight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(500, 917);
            pnlRight.TabIndex = 5;
            // 
            // panelControl2
            // 
            panelControl2.Controls.Add(labelControl13);
            panelControl2.Controls.Add(lblBranchName);
            panelControl2.Controls.Add(labelControl12);
            panelControl2.Controls.Add(txtKhachThanhToan);
            panelControl2.Controls.Add(labelControl11);
            panelControl2.Controls.Add(txtKhachCanTra);
            panelControl2.Controls.Add(labelControl10);
            panelControl2.Controls.Add(txtThuKhac);
            panelControl2.Controls.Add(labelControl9);
            panelControl2.Controls.Add(txtDiscount);
            panelControl2.Controls.Add(txtTotal);
            panelControl2.Controls.Add(labelControl8);
            panelControl2.Controls.Add(labelControl7);
            panelControl2.Controls.Add(labelControl6);
            panelControl2.Controls.Add(txtBuyer);
            panelControl2.Controls.Add(labelControl5);
            panelControl2.Controls.Add(txtSale);
            panelControl2.Controls.Add(labelControl4);
            panelControl2.Controls.Add(labelControl3);
            panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            panelControl2.Location = new Point(2, 2);
            panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            panelControl2.Name = "panelControl2";
            panelControl2.Size = new Size(496, 298);
            panelControl2.TabIndex = 9;
            // 
            // labelControl13
            // 
            labelControl13.Location = new Point(414, 62);
            labelControl13.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl13.Name = "labelControl13";
            labelControl13.Size = new Size(52, 13);
            labelControl13.TabIndex = 14;
            labelControl13.Text = "Chi nhánh:";
            // 
            // lblBranchName
            // 
            lblBranchName.Location = new Point(510, 62);
            lblBranchName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            lblBranchName.Name = "lblBranchName";
            lblBranchName.Size = new Size(70, 13);
            lblBranchName.TabIndex = 14;
            lblBranchName.Text = "lblBranchName";
            // 
            // labelControl12
            // 
            labelControl12.Appearance.Font = new Font("Tahoma", 9.75F);
            labelControl12.Appearance.Options.UseFont = true;
            labelControl12.Location = new Point(67, 247);
            labelControl12.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl12.Name = "labelControl12";
            labelControl12.Size = new Size(104, 16);
            labelControl12.TabIndex = 24;
            labelControl12.Text = "Khách thanh toán:";
            // 
            // txtKhachThanhToan
            // 
            txtKhachThanhToan.Location = new Point(414, 243);
            txtKhachThanhToan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtKhachThanhToan.MenuManager = ribbon;
            txtKhachThanhToan.Name = "txtKhachThanhToan";
            txtKhachThanhToan.Size = new Size(213, 20);
            txtKhachThanhToan.TabIndex = 23;
            // 
            // labelControl11
            // 
            labelControl11.Appearance.Font = new Font("Tahoma", 9.75F);
            labelControl11.Appearance.Options.UseFont = true;
            labelControl11.Location = new Point(67, 207);
            labelControl11.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl11.Name = "labelControl11";
            labelControl11.Size = new Size(78, 16);
            labelControl11.TabIndex = 22;
            labelControl11.Text = "Khách cần trả";
            // 
            // txtKhachCanTra
            // 
            txtKhachCanTra.Location = new Point(414, 203);
            txtKhachCanTra.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtKhachCanTra.MenuManager = ribbon;
            txtKhachCanTra.Name = "txtKhachCanTra";
            txtKhachCanTra.Size = new Size(213, 20);
            txtKhachCanTra.TabIndex = 21;
            // 
            // labelControl10
            // 
            labelControl10.Appearance.Font = new Font("Tahoma", 9.75F);
            labelControl10.Appearance.Options.UseFont = true;
            labelControl10.Location = new Point(67, 171);
            labelControl10.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl10.Name = "labelControl10";
            labelControl10.Size = new Size(52, 16);
            labelControl10.TabIndex = 20;
            labelControl10.Text = "Thu khác";
            // 
            // txtThuKhac
            // 
            txtThuKhac.Location = new Point(414, 167);
            txtThuKhac.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtThuKhac.MenuManager = ribbon;
            txtThuKhac.Name = "txtThuKhac";
            txtThuKhac.Size = new Size(213, 20);
            txtThuKhac.TabIndex = 19;
            // 
            // labelControl9
            // 
            labelControl9.Appearance.Font = new Font("Tahoma", 9.75F);
            labelControl9.Appearance.Options.UseFont = true;
            labelControl9.Location = new Point(67, 133);
            labelControl9.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl9.Name = "labelControl9";
            labelControl9.Size = new Size(55, 16);
            labelControl9.TabIndex = 18;
            labelControl9.Text = "Giảm giá:";
            // 
            // txtDiscount
            // 
            txtDiscount.Location = new Point(414, 130);
            txtDiscount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtDiscount.MenuManager = ribbon;
            txtDiscount.Name = "txtDiscount";
            txtDiscount.Size = new Size(213, 20);
            txtDiscount.TabIndex = 17;
            // 
            // txtTotal
            // 
            txtTotal.Location = new Point(414, 98);
            txtTotal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtTotal.MenuManager = ribbon;
            txtTotal.Name = "txtTotal";
            txtTotal.Size = new Size(213, 20);
            txtTotal.TabIndex = 16;
            // 
            // labelControl8
            // 
            labelControl8.Location = new Point(179, 109);
            labelControl8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl8.Name = "labelControl8";
            labelControl8.Size = new Size(18, 13);
            labelControl8.TabIndex = 14;
            labelControl8.Text = "lblSl";
            // 
            // labelControl7
            // 
            labelControl7.Appearance.Font = new Font("Tahoma", 9.75F);
            labelControl7.Appearance.Options.UseFont = true;
            labelControl7.Location = new Point(67, 107);
            labelControl7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl7.Name = "labelControl7";
            labelControl7.Size = new Size(91, 16);
            labelControl7.TabIndex = 13;
            labelControl7.Text = "Tổng tiền hàng:";
            // 
            // labelControl6
            // 
            labelControl6.Appearance.Font = new Font("Tahoma", 9.75F);
            labelControl6.Appearance.Options.UseFont = true;
            labelControl6.Location = new Point(67, 58);
            labelControl6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl6.Name = "labelControl6";
            labelControl6.Size = new Size(67, 16);
            labelControl6.TabIndex = 12;
            labelControl6.Text = "Người mua:";
            // 
            // txtBuyer
            // 
            txtBuyer.Location = new Point(161, 54);
            txtBuyer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtBuyer.MenuManager = ribbon;
            txtBuyer.Name = "txtBuyer";
            txtBuyer.Size = new Size(213, 20);
            txtBuyer.TabIndex = 11;
            // 
            // labelControl5
            // 
            labelControl5.Appearance.Font = new Font("Tahoma", 9.75F);
            labelControl5.Appearance.Options.UseFont = true;
            labelControl5.Location = new Point(67, 19);
            labelControl5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl5.Name = "labelControl5";
            labelControl5.Size = new Size(63, 16);
            labelControl5.TabIndex = 10;
            labelControl5.Text = "Người bán:";
            // 
            // txtSale
            // 
            txtSale.Location = new Point(161, 16);
            txtSale.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtSale.MenuManager = ribbon;
            txtSale.Name = "txtSale";
            txtSale.Size = new Size(213, 20);
            txtSale.TabIndex = 0;
            // 
            // labelControl4
            // 
            labelControl4.Location = new Point(510, 19);
            labelControl4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl4.Name = "labelControl4";
            labelControl4.Size = new Size(28, 13);
            labelControl4.TabIndex = 8;
            labelControl4.Text = "lblMut";
            // 
            // labelControl3
            // 
            labelControl3.Location = new Point(414, 19);
            labelControl3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl3.Name = "labelControl3";
            labelControl3.Size = new Size(32, 13);
            labelControl3.TabIndex = 7;
            labelControl3.Text = "lblTime";
            // 
            // pnlOrder
            // 
            pnlOrder.Controls.Add(panelControl1);
            pnlOrder.Controls.Add(pnlControl);
            pnlOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlOrder.Location = new Point(0, 158);
            pnlOrder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pnlOrder.Name = "pnlOrder";
            pnlOrder.Size = new Size(1438, 917);
            pnlOrder.TabIndex = 6;
            // 
            // panelControl1
            // 
            panelControl1.Controls.Add(grdOrder);
            panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControl1.Location = new Point(2, 109);
            panelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panelControl1.Name = "panelControl1";
            panelControl1.Size = new Size(1434, 806);
            panelControl1.TabIndex = 1;
            // 
            // grdOrder
            // 
            grdOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            grdOrder.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            grdOrder.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            grdOrder.Location = new Point(2, 2);
            grdOrder.MainView = gridView1;
            grdOrder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            grdOrder.MenuManager = ribbon;
            grdOrder.Name = "grdOrder";
            grdOrder.Size = new Size(1430, 802);
            grdOrder.TabIndex = 0;
            grdOrder.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });
            // 
            // gridView1
            // 
            gridView1.DetailHeight = 620;
            gridView1.GridControl = grdOrder;
            gridView1.Name = "gridView1";
            gridView1.OptionsEditForm.PopupEditFormWidth = 1333;
            gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // pnlControl
            // 
            pnlControl.Controls.Add(drpOrders);
            pnlControl.Controls.Add(btnGetOrder);
            pnlControl.Controls.Add(labelControl2);
            pnlControl.Controls.Add(labelControl1);
            pnlControl.Controls.Add(txtProductCode);
            pnlControl.Controls.Add(txtOrderCode);
            pnlControl.Dock = System.Windows.Forms.DockStyle.Top;
            pnlControl.Location = new Point(2, 2);
            pnlControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            pnlControl.Name = "pnlControl";
            pnlControl.Size = new Size(1434, 107);
            pnlControl.TabIndex = 0;
            // 
            // drpOrders
            // 
            drpOrders.Location = new Point(671, 55);
            drpOrders.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            drpOrders.MenuManager = ribbon;
            drpOrders.Name = "drpOrders";
            drpOrders.Size = new Size(239, 28);
            drpOrders.TabIndex = 7;
            drpOrders.Text = "Order";
            // 
            // btnGetOrder
            // 
            btnGetOrder.Location = new Point(671, 10);
            btnGetOrder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnGetOrder.Name = "btnGetOrder";
            btnGetOrder.Size = new Size(139, 30);
            btnGetOrder.TabIndex = 5;
            btnGetOrder.Text = "Lấy đơn hàng";
            btnGetOrder.Click += btnGetOrder_Click;
            // 
            // labelControl2
            // 
            labelControl2.Appearance.Font = new Font("Tahoma", 9.75F);
            labelControl2.Appearance.Options.UseFont = true;
            labelControl2.Location = new Point(11, 58);
            labelControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl2.Name = "labelControl2";
            labelControl2.Size = new Size(49, 16);
            labelControl2.TabIndex = 3;
            labelControl2.Text = "Mã hàng";
            // 
            // labelControl1
            // 
            labelControl1.Appearance.Font = new Font("Tahoma", 9.75F);
            labelControl1.Appearance.Options.UseFont = true;
            labelControl1.Location = new Point(11, 12);
            labelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new Size(76, 16);
            labelControl1.TabIndex = 2;
            labelControl1.Text = "Mã Đơn hàng";
            // 
            // txtProductCode
            // 
            txtProductCode.Location = new Point(230, 56);
            txtProductCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtProductCode.MenuManager = ribbon;
            txtProductCode.Name = "txtProductCode";
            txtProductCode.Properties.AutoHeight = false;
            txtProductCode.Size = new Size(416, 30);
            txtProductCode.TabIndex = 1;
            // 
            // txtOrderCode
            // 
            txtOrderCode.Location = new Point(230, 10);
            txtOrderCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtOrderCode.MenuManager = ribbon;
            txtOrderCode.Name = "txtOrderCode";
            txtOrderCode.Properties.AutoHeight = false;
            txtOrderCode.Size = new Size(416, 30);
            txtOrderCode.TabIndex = 0;
            // 
            // FrmMain
            // 
            AllowFormGlass = DevExpress.Utils.DefaultBoolean.False;
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new SizeF(10F, 23F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(1938, 1099);
            Controls.Add(pnlOrder);
            Controls.Add(pnlRight);
            Controls.Add(ribbonStatusBar);
            Controls.Add(ribbon);
            Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "FrmMain";
            Ribbon = ribbon;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            StatusBar = ribbonStatusBar;
            Text = "FrmMain";
            Load += FrmMain_Load;
            ((System.ComponentModel.ISupportInitialize)ribbon).EndInit();
            ((System.ComponentModel.ISupportInitialize)pnlRight).EndInit();
            pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)panelControl2).EndInit();
            panelControl2.ResumeLayout(false);
            panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtKhachThanhToan.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtKhachCanTra.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtThuKhac.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtDiscount.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtTotal.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtBuyer.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtSale.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)pnlOrder).EndInit();
            pnlOrder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)panelControl1).EndInit();
            panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdOrder).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pnlControl).EndInit();
            pnlControl.ResumeLayout(false);
            pnlControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtProductCode.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtOrderCode.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarSubItem btnOrder;
        private DevExpress.XtraEditors.PanelControl pnlRight;
        private DevExpress.XtraEditors.PanelControl pnlOrder;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl grdOrder;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PanelControl pnlControl;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtProductCode;
        private DevExpress.XtraEditors.TextEdit txtOrderCode;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton btnGetOrder;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtSale;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit txtBuyer;
        private DevExpress.XtraEditors.TextEdit txtTotal;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.TextEdit txtKhachCanTra;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.TextEdit txtThuKhac;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.TextEdit txtDiscount;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.TextEdit txtKhachThanhToan;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.LabelControl lblBranchName;
        private DevExpress.XtraEditors.DropDownButton drpOrders;
    }
}