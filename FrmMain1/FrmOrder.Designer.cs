namespace FrmMain
{
    partial class FrmOrder
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
            groupControl1 = new DevExpress.XtraEditors.GroupControl();
            layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            lookUpEdit3 = new DevExpress.XtraEditors.LookUpEdit();
            lookUpEdit2 = new DevExpress.XtraEditors.LookUpEdit();
            lookUpEdit1 = new DevExpress.XtraEditors.LookUpEdit();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            grdControlOrders = new DevExpress.XtraGrid.GridControl();
            grdViewOrders = new DevExpress.XtraGrid.Views.Grid.GridView();
            stt = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmProductName = new DevExpress.XtraGrid.Columns.GridColumn();
            quantity = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmDiscount = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmCheck = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)groupControl1).BeginInit();
            groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)layoutControl1).BeginInit();
            layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dateEdit1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dateEdit1.Properties.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit3.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit2.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grdControlOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grdViewOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemCheckEdit1).BeginInit();
            SuspendLayout();
            // 
            // groupControl1
            // 
            groupControl1.Controls.Add(layoutControl1);
            groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            groupControl1.Location = new System.Drawing.Point(0, 0);
            groupControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupControl1.Name = "groupControl1";
            groupControl1.Size = new System.Drawing.Size(1290, 93);
            groupControl1.TabIndex = 0;
            groupControl1.Text = "Lọc Đơn hàng";
            // 
            // layoutControl1
            // 
            layoutControl1.Controls.Add(dateEdit1);
            layoutControl1.Controls.Add(lookUpEdit3);
            layoutControl1.Controls.Add(lookUpEdit2);
            layoutControl1.Controls.Add(lookUpEdit1);
            layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            layoutControl1.Location = new System.Drawing.Point(2, 22);
            layoutControl1.Name = "layoutControl1";
            layoutControl1.Root = Root;
            layoutControl1.Size = new System.Drawing.Size(1286, 69);
            layoutControl1.TabIndex = 0;
            layoutControl1.Text = "layoutControl1";
            // 
            // dateEdit1
            // 
            dateEdit1.EditValue = null;
            dateEdit1.Location = new System.Drawing.Point(762, 36);
            dateEdit1.Name = "dateEdit1";
            dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dateEdit1.Size = new System.Drawing.Size(512, 20);
            dateEdit1.StyleController = layoutControl1;
            dateEdit1.TabIndex = 4;
            // 
            // lookUpEdit3
            // 
            lookUpEdit3.EditValue = "<Null>";
            lookUpEdit3.Location = new System.Drawing.Point(129, 36);
            lookUpEdit3.Name = "lookUpEdit3";
            lookUpEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            lookUpEdit3.Size = new System.Drawing.Size(512, 20);
            lookUpEdit3.StyleController = layoutControl1;
            lookUpEdit3.TabIndex = 3;
            // 
            // lookUpEdit2
            // 
            lookUpEdit2.Location = new System.Drawing.Point(762, 12);
            lookUpEdit2.Name = "lookUpEdit2";
            lookUpEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            lookUpEdit2.Size = new System.Drawing.Size(512, 20);
            lookUpEdit2.StyleController = layoutControl1;
            lookUpEdit2.TabIndex = 2;
            // 
            // lookUpEdit1
            // 
            lookUpEdit1.Location = new System.Drawing.Point(129, 12);
            lookUpEdit1.Name = "lookUpEdit1";
            lookUpEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            lookUpEdit1.Size = new System.Drawing.Size(512, 20);
            lookUpEdit1.StyleController = layoutControl1;
            lookUpEdit1.TabIndex = 0;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem1, layoutControlItem3, layoutControlItem2, layoutControlItem4 });
            Root.Name = "Root";
            Root.Size = new System.Drawing.Size(1286, 69);
            Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = lookUpEdit1;
            layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new System.Drawing.Size(633, 24);
            layoutControlItem1.Text = "Chi nhánh";
            layoutControlItem1.TextSize = new System.Drawing.Size(113, 14);
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = lookUpEdit3;
            layoutControlItem3.Location = new System.Drawing.Point(0, 24);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new System.Drawing.Size(633, 25);
            layoutControlItem3.Text = "Người tạo";
            layoutControlItem3.TextSize = new System.Drawing.Size(113, 14);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = lookUpEdit2;
            layoutControlItem2.Location = new System.Drawing.Point(633, 0);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new System.Drawing.Size(633, 24);
            layoutControlItem2.Text = "Trạng thái Đơn hàng";
            layoutControlItem2.TextSize = new System.Drawing.Size(113, 14);
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = dateEdit1;
            layoutControlItem4.Location = new System.Drawing.Point(633, 24);
            layoutControlItem4.Name = "layoutControlItem4";
            layoutControlItem4.Size = new System.Drawing.Size(633, 25);
            layoutControlItem4.Text = "Thời gian";
            layoutControlItem4.TextSize = new System.Drawing.Size(113, 14);
            // 
            // grdControlOrders
            // 
            grdControlOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            grdControlOrders.Location = new System.Drawing.Point(0, 93);
            grdControlOrders.MainView = grdViewOrders;
            grdControlOrders.Name = "grdControlOrders";
            grdControlOrders.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemCheckEdit1 });
            grdControlOrders.Size = new System.Drawing.Size(1290, 515);
            grdControlOrders.TabIndex = 1;
            grdControlOrders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grdViewOrders });
            // 
            // grdViewOrders
            // 
            grdViewOrders.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { stt, grdClmProductName, quantity, grdClmPrice, grdClmDiscount, grdClmTotal, grdClmCheck });
            grdViewOrders.GridControl = grdControlOrders;
            grdViewOrders.Name = "grdViewOrders";
            grdViewOrders.OptionsView.ShowGroupPanel = false;
            // 
            // stt
            // 
            stt.Caption = "STT";
            stt.FieldName = "stt";
            stt.Name = "stt";
            stt.Visible = true;
            stt.VisibleIndex = 0;
            stt.Width = 58;
            // 
            // grdClmProductName
            // 
            grdClmProductName.Caption = "Tên sản phẩm";
            grdClmProductName.FieldName = "productName";
            grdClmProductName.Name = "grdClmProductName";
            grdClmProductName.Visible = true;
            grdClmProductName.VisibleIndex = 1;
            grdClmProductName.Width = 268;
            // 
            // quantity
            // 
            quantity.Caption = "Số lượng";
            quantity.Name = "quantity";
            quantity.Visible = true;
            quantity.VisibleIndex = 2;
            quantity.Width = 130;
            // 
            // grdClmPrice
            // 
            grdClmPrice.Caption = "Đơn giá";
            grdClmPrice.FieldName = "price";
            grdClmPrice.Name = "grdClmPrice";
            grdClmPrice.Visible = true;
            grdClmPrice.VisibleIndex = 3;
            grdClmPrice.Width = 129;
            // 
            // grdClmDiscount
            // 
            grdClmDiscount.Caption = "Giảm giá";
            grdClmDiscount.FieldName = "discount";
            grdClmDiscount.Name = "grdClmDiscount";
            grdClmDiscount.Visible = true;
            grdClmDiscount.VisibleIndex = 4;
            grdClmDiscount.Width = 229;
            // 
            // grdClmTotal
            // 
            grdClmTotal.Caption = "Thành tiền";
            grdClmTotal.FieldName = "totalPayment";
            grdClmTotal.Name = "grdClmTotal";
            grdClmTotal.Visible = true;
            grdClmTotal.VisibleIndex = 5;
            grdClmTotal.Width = 249;
            // 
            // grdClmCheck
            // 
            grdClmCheck.Caption = "Check";
            grdClmCheck.ColumnEdit = repositoryItemCheckEdit1;
            grdClmCheck.Name = "grdClmCheck";
            grdClmCheck.Visible = true;
            grdClmCheck.VisibleIndex = 6;
            grdClmCheck.Width = 212;
            // 
            // repositoryItemCheckEdit1
            // 
            repositoryItemCheckEdit1.AutoHeight = false;
            repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // FrmOrder
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1290, 608);
            Controls.Add(grdControlOrders);
            Controls.Add(groupControl1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmOrder";
            Text = "Danh sách Đơn hàng";
            Load += FrmOrder_LoadAsync;
            Shown += FrmOrder_Shown;
            ((System.ComponentModel.ISupportInitialize)groupControl1).EndInit();
            groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)layoutControl1).EndInit();
            layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dateEdit1.Properties.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)dateEdit1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit3.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit2.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).EndInit();
            ((System.ComponentModel.ISupportInitialize)grdControlOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)grdViewOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemCheckEdit1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit2;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.DateEdit dateEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraGrid.GridControl grdControlOrders;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewOrders;
        private DevExpress.XtraGrid.Columns.GridColumn stt;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmProductName;
        private DevExpress.XtraGrid.Columns.GridColumn quantity;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmPrice;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmDiscount;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmTotal;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmCheck;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    }
}