namespace FrmMain
{
    partial class FrmPurchase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPurchase));
            groupControl1 = new DevExpress.XtraEditors.GroupControl();
            layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            btnReloadPurchase = new DevExpress.XtraEditors.SimpleButton();
            layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            chkCancel = new DevExpress.XtraEditors.CheckEdit();
            chkFinish = new DevExpress.XtraEditors.CheckEdit();
            chkDraft = new DevExpress.XtraEditors.CheckEdit();
            dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            lookUpEdit3 = new DevExpress.XtraEditors.LookUpEdit();
            lkupBranch = new DevExpress.XtraEditors.LookUpEdit();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            grdControlOrders = new DevExpress.XtraGrid.GridControl();
            grdViewOrders = new DevExpress.XtraGrid.Views.Grid.GridView();
            grdClmCode = new DevExpress.XtraGrid.Columns.GridColumn();
            purchaseDate = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmSupplierName = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmStatusValue = new DevExpress.XtraGrid.Columns.GridColumn();
            clmQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            clmId = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            ((System.ComponentModel.ISupportInitialize)groupControl1).BeginInit();
            groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)layoutControlTop).BeginInit();
            layoutControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)layoutControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkCancel.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkFinish.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkDraft.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dateEdit1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dateEdit1.Properties.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit3.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lkupBranch.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grdControlOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grdViewOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemCheckEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit2.CalendarTimeProperties).BeginInit();
            SuspendLayout();
            // 
            // groupControl1
            // 
            groupControl1.Controls.Add(layoutControlTop);
            groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            groupControl1.Location = new System.Drawing.Point(0, 0);
            groupControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupControl1.Name = "groupControl1";
            groupControl1.Size = new System.Drawing.Size(1230, 128);
            groupControl1.TabIndex = 0;
            groupControl1.Text = "Lọc phiếu Nhập hàng";
            // 
            // layoutControlTop
            // 
            layoutControlTop.AutoScroll = false;
            layoutControlTop.Controls.Add(btnReloadPurchase);
            layoutControlTop.Controls.Add(layoutControl1);
            layoutControlTop.Controls.Add(chkCancel);
            layoutControlTop.Controls.Add(chkFinish);
            layoutControlTop.Controls.Add(chkDraft);
            layoutControlTop.Controls.Add(dateEdit1);
            layoutControlTop.Controls.Add(lookUpEdit3);
            layoutControlTop.Controls.Add(lkupBranch);
            layoutControlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            layoutControlTop.Location = new System.Drawing.Point(2, 22);
            layoutControlTop.Name = "layoutControlTop";
            layoutControlTop.Root = Root;
            layoutControlTop.Size = new System.Drawing.Size(1226, 104);
            layoutControlTop.TabIndex = 0;
            layoutControlTop.Text = "layoutControl1";
            // 
            // btnReloadPurchase
            // 
            btnReloadPurchase.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("btnReloadPurchase.ImageOptions.Image");
            btnReloadPurchase.Location = new System.Drawing.Point(355, 12);
            btnReloadPurchase.Name = "btnReloadPurchase";
            btnReloadPurchase.Size = new System.Drawing.Size(106, 22);
            btnReloadPurchase.StyleController = layoutControlTop;
            btnReloadPurchase.TabIndex = 9;
            btnReloadPurchase.Text = "Tải lại dữ liệu";
            // 
            // layoutControl1
            // 
            layoutControl1.Location = new System.Drawing.Point(697, 12);
            layoutControl1.Name = "layoutControl1";
            layoutControl1.Root = layoutControlGroup1;
            layoutControl1.Size = new System.Drawing.Size(517, 80);
            layoutControl1.TabIndex = 8;
            layoutControl1.Text = "layoutControl1";
            // 
            // layoutControlGroup1
            // 
            layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            layoutControlGroup1.GroupBordersVisible = false;
            layoutControlGroup1.Name = "layoutControlGroup1";
            layoutControlGroup1.Size = new System.Drawing.Size(517, 80);
            layoutControlGroup1.TextVisible = false;
            // 
            // chkCancel
            // 
            chkCancel.Location = new System.Drawing.Point(634, 12);
            chkCancel.Name = "chkCancel";
            chkCancel.Properties.Caption = "Đã huỷ";
            chkCancel.Size = new System.Drawing.Size(59, 19);
            chkCancel.StyleController = layoutControlTop;
            chkCancel.TabIndex = 7;
            chkCancel.CheckedChanged += Handler_CheckedChanged;
            // 
            // chkFinish
            // 
            chkFinish.Location = new System.Drawing.Point(545, 12);
            chkFinish.Name = "chkFinish";
            chkFinish.Properties.Caption = "Hoàn thành";
            chkFinish.Size = new System.Drawing.Size(85, 19);
            chkFinish.StyleController = layoutControlTop;
            chkFinish.TabIndex = 6;
            chkFinish.CheckedChanged += Handler_CheckedChanged;
            // 
            // chkDraft
            // 
            chkDraft.EditValue = true;
            chkDraft.Location = new System.Drawing.Point(465, 12);
            chkDraft.Name = "chkDraft";
            chkDraft.Properties.Caption = "Phiếu tạm";
            chkDraft.Size = new System.Drawing.Size(76, 19);
            chkDraft.StyleController = layoutControlTop;
            chkDraft.TabIndex = 5;
            chkDraft.CheckedChanged += Handler_CheckedChanged;
            // 
            // dateEdit1
            // 
            dateEdit1.EditValue = null;
            dateEdit1.Location = new System.Drawing.Point(70, 62);
            dateEdit1.Name = "dateEdit1";
            dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dateEdit1.Size = new System.Drawing.Size(391, 20);
            dateEdit1.StyleController = layoutControlTop;
            dateEdit1.TabIndex = 4;
            // 
            // lookUpEdit3
            // 
            lookUpEdit3.EditValue = "<Null>";
            lookUpEdit3.Location = new System.Drawing.Point(70, 38);
            lookUpEdit3.Name = "lookUpEdit3";
            lookUpEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            lookUpEdit3.Properties.NullText = "Chọn Người Tạo";
            lookUpEdit3.Size = new System.Drawing.Size(391, 20);
            lookUpEdit3.StyleController = layoutControlTop;
            lookUpEdit3.TabIndex = 3;
            // 
            // lkupBranch
            // 
            lkupBranch.Location = new System.Drawing.Point(70, 12);
            lkupBranch.Name = "lkupBranch";
            lkupBranch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            lkupBranch.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] { new DevExpress.XtraEditors.Controls.LookUpColumnInfo("BranchName", "", 150, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default) });
            lkupBranch.Properties.DisplayMember = "BranchName";
            lkupBranch.Properties.NullText = "Chọn Chi Nhánh";
            lkupBranch.Properties.ShowFooter = false;
            lkupBranch.Properties.ValueMember = "BranchId";
            lkupBranch.Size = new System.Drawing.Size(281, 20);
            lkupBranch.StyleController = layoutControlTop;
            lkupBranch.TabIndex = 0;
            lkupBranch.EditValueChanged += lkupBranch_EditValueChanged;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem1, layoutControlItem3, layoutControlItem5, layoutControlItem6, layoutControlItem7, layoutControlItem4, layoutControlItem2, layoutControlItem8 });
            Root.Name = "Root";
            Root.Size = new System.Drawing.Size(1226, 104);
            Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = lkupBranch;
            layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new System.Drawing.Size(343, 26);
            layoutControlItem1.Text = "Chi nhánh";
            layoutControlItem1.TextSize = new System.Drawing.Size(54, 14);
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = lookUpEdit3;
            layoutControlItem3.Location = new System.Drawing.Point(0, 26);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new System.Drawing.Size(453, 24);
            layoutControlItem3.Text = "Người tạo";
            layoutControlItem3.TextSize = new System.Drawing.Size(54, 14);
            // 
            // layoutControlItem5
            // 
            layoutControlItem5.Control = chkDraft;
            layoutControlItem5.Location = new System.Drawing.Point(453, 0);
            layoutControlItem5.Name = "layoutControlItem5";
            layoutControlItem5.Size = new System.Drawing.Size(80, 84);
            layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            layoutControlItem6.Control = chkFinish;
            layoutControlItem6.Location = new System.Drawing.Point(533, 0);
            layoutControlItem6.Name = "layoutControlItem6";
            layoutControlItem6.Size = new System.Drawing.Size(89, 84);
            layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            layoutControlItem7.Control = chkCancel;
            layoutControlItem7.Location = new System.Drawing.Point(622, 0);
            layoutControlItem7.Name = "layoutControlItem7";
            layoutControlItem7.Size = new System.Drawing.Size(63, 84);
            layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = dateEdit1;
            layoutControlItem4.Location = new System.Drawing.Point(0, 50);
            layoutControlItem4.Name = "layoutControlItem4";
            layoutControlItem4.Size = new System.Drawing.Size(453, 34);
            layoutControlItem4.Text = "Thời gian";
            layoutControlItem4.TextSize = new System.Drawing.Size(54, 14);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = layoutControl1;
            layoutControlItem2.Location = new System.Drawing.Point(685, 0);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new System.Drawing.Size(521, 84);
            layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            layoutControlItem8.Control = btnReloadPurchase;
            layoutControlItem8.Location = new System.Drawing.Point(343, 0);
            layoutControlItem8.Name = "layoutControlItem8";
            layoutControlItem8.Size = new System.Drawing.Size(110, 26);
            layoutControlItem8.TextVisible = false;
            // 
            // grdControlOrders
            // 
            grdControlOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            grdControlOrders.Location = new System.Drawing.Point(0, 128);
            grdControlOrders.MainView = grdViewOrders;
            grdControlOrders.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grdControlOrders.Name = "grdControlOrders";
            grdControlOrders.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemCheckEdit1, repositoryItemDateEdit1, repositoryItemDateEdit2 });
            grdControlOrders.Size = new System.Drawing.Size(1230, 480);
            grdControlOrders.TabIndex = 1;
            grdControlOrders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grdViewOrders });
            // 
            // grdViewOrders
            // 
            grdViewOrders.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { grdClmCode, purchaseDate, grdClmSupplierName, grdClmTotal, grdClmStatusValue, clmQuantity, clmId });
            grdViewOrders.GridControl = grdControlOrders;
            grdViewOrders.Name = "grdViewOrders";
            grdViewOrders.OptionsBehavior.Editable = false;
            grdViewOrders.OptionsView.ShowGroupPanel = false;
            grdViewOrders.DoubleClick += grdViewOrders_DoubleClick;
            // 
            // grdClmCode
            // 
            grdClmCode.Caption = "Mã nhập hàng";
            grdClmCode.FieldName = "Code";
            grdClmCode.Name = "grdClmCode";
            grdClmCode.Visible = true;
            grdClmCode.VisibleIndex = 0;
            grdClmCode.Width = 133;
            // 
            // purchaseDate
            // 
            purchaseDate.Caption = "Thời gian";
            purchaseDate.FieldName = "PurchaseDate";
            purchaseDate.Name = "purchaseDate";
            purchaseDate.Visible = true;
            purchaseDate.VisibleIndex = 1;
            purchaseDate.Width = 156;
            // 
            // grdClmSupplierName
            // 
            grdClmSupplierName.Caption = "Nhà cung cấp";
            grdClmSupplierName.FieldName = "SupplierName";
            grdClmSupplierName.Name = "grdClmSupplierName";
            grdClmSupplierName.Visible = true;
            grdClmSupplierName.VisibleIndex = 2;
            grdClmSupplierName.Width = 188;
            // 
            // grdClmTotal
            // 
            grdClmTotal.Caption = "Cần trả NCC";
            grdClmTotal.DisplayFormat.FormatString = "n0";
            grdClmTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdClmTotal.FieldName = "Total";
            grdClmTotal.Name = "grdClmTotal";
            grdClmTotal.Visible = true;
            grdClmTotal.VisibleIndex = 4;
            grdClmTotal.Width = 233;
            // 
            // grdClmStatusValue
            // 
            grdClmStatusValue.Caption = "Trạng thái";
            grdClmStatusValue.FieldName = "StatusValue";
            grdClmStatusValue.Name = "grdClmStatusValue";
            grdClmStatusValue.Visible = true;
            grdClmStatusValue.VisibleIndex = 5;
            grdClmStatusValue.Width = 160;
            // 
            // clmQuantity
            // 
            clmQuantity.Caption = "Tổng số mặt hàng";
            clmQuantity.FieldName = "Quantity";
            clmQuantity.Name = "clmQuantity";
            clmQuantity.Visible = true;
            clmQuantity.VisibleIndex = 3;
            clmQuantity.Width = 341;
            // 
            // clmId
            // 
            clmId.Caption = "ID";
            clmId.FieldName = "Id";
            clmId.Name = "clmId";
            // 
            // repositoryItemCheckEdit1
            // 
            repositoryItemCheckEdit1.AutoHeight = false;
            repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // repositoryItemDateEdit1
            // 
            repositoryItemDateEdit1.AutoHeight = false;
            repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // repositoryItemDateEdit2
            // 
            repositoryItemDateEdit2.AutoHeight = false;
            repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            // 
            // FrmPurchase
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1230, 608);
            Controls.Add(grdControlOrders);
            Controls.Add(groupControl1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPurchase";
            Text = "Danh sách Phiếu Nhập hàng";
            Load += FrmOrder_Load;
            Shown += FrmOrder_Shown;
            ((System.ComponentModel.ISupportInitialize)groupControl1).EndInit();
            groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)layoutControlTop).EndInit();
            layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)layoutControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkCancel.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkFinish.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkDraft.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)dateEdit1.Properties.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)dateEdit1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit3.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)lkupBranch.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem8).EndInit();
            ((System.ComponentModel.ISupportInitialize)grdControlOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)grdViewOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemCheckEdit1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit2.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)repositoryItemDateEdit2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControlTop;
        private DevExpress.XtraEditors.LookUpEdit lkBranch;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.DateEdit dateEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraGrid.GridControl grdControlOrders;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewOrders;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmCode;
        private DevExpress.XtraGrid.Columns.GridColumn purchaseDate;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmSupplierName;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmTotal;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmStatusValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraEditors.CheckEdit chkCancel;
        private DevExpress.XtraEditors.CheckEdit chkFinish;
        private DevExpress.XtraEditors.CheckEdit chkDraft;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.LookUpEdit lkupBranch;
        private DevExpress.XtraGrid.Columns.GridColumn clmQuantity;
        private DevExpress.XtraEditors.SimpleButton btnReloadPurchase;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraGrid.Columns.GridColumn clmId;
    }
}