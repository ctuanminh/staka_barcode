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
            layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
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
            grdControlOrders = new DevExpress.XtraGrid.GridControl();
            grdViewOrders = new DevExpress.XtraGrid.Views.Grid.GridView();
            grdClmCode = new DevExpress.XtraGrid.Columns.GridColumn();
            purchaseDate = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmCustomerName = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmTotalPayment = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmStatus = new DevExpress.XtraGrid.Columns.GridColumn();
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
            groupControl1.Size = new System.Drawing.Size(1238, 128);
            groupControl1.TabIndex = 0;
            groupControl1.Text = "Lọc Đơn hàng";
            // 
            // layoutControlTop
            // 
            layoutControlTop.AutoScroll = false;
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
            layoutControlTop.Size = new System.Drawing.Size(1234, 104);
            layoutControlTop.TabIndex = 0;
            layoutControlTop.Text = "layoutControl1";
            // 
            // layoutControl1
            // 
            layoutControl1.Location = new System.Drawing.Point(487, 12);
            layoutControl1.Name = "layoutControl1";
            layoutControl1.Root = layoutControlGroup1;
            layoutControl1.Size = new System.Drawing.Size(735, 80);
            layoutControl1.TabIndex = 8;
            layoutControl1.Text = "layoutControl1";
            // 
            // layoutControlGroup1
            // 
            layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            layoutControlGroup1.GroupBordersVisible = false;
            layoutControlGroup1.Name = "layoutControlGroup1";
            layoutControlGroup1.Size = new System.Drawing.Size(735, 80);
            layoutControlGroup1.TextVisible = false;
            // 
            // chkCancel
            // 
            chkCancel.Location = new System.Drawing.Point(424, 12);
            chkCancel.Name = "chkCancel";
            chkCancel.Properties.Caption = "Đã huỷ";
            chkCancel.Size = new System.Drawing.Size(59, 19);
            chkCancel.StyleController = layoutControlTop;
            chkCancel.TabIndex = 7;
            chkCancel.CheckedChanged += Handler_CheckedChanged;
            // 
            // chkFinish
            // 
            chkFinish.Location = new System.Drawing.Point(332, 12);
            chkFinish.Name = "chkFinish";
            chkFinish.Properties.Caption = "Hoàn thành";
            chkFinish.Size = new System.Drawing.Size(88, 19);
            chkFinish.StyleController = layoutControlTop;
            chkFinish.TabIndex = 6;
            chkFinish.CheckedChanged += Handler_CheckedChanged;
            // 
            // chkDraft
            // 
            chkDraft.EditValue = true;
            chkDraft.Location = new System.Drawing.Point(252, 12);
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
            dateEdit1.Location = new System.Drawing.Point(70, 60);
            dateEdit1.Name = "dateEdit1";
            dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            dateEdit1.Size = new System.Drawing.Size(178, 20);
            dateEdit1.StyleController = layoutControlTop;
            dateEdit1.TabIndex = 4;
            // 
            // lookUpEdit3
            // 
            lookUpEdit3.EditValue = "<Null>";
            lookUpEdit3.Location = new System.Drawing.Point(70, 36);
            lookUpEdit3.Name = "lookUpEdit3";
            lookUpEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            lookUpEdit3.Properties.NullText = "Chọn Người Tạo";
            lookUpEdit3.Size = new System.Drawing.Size(178, 20);
            lookUpEdit3.StyleController = layoutControlTop;
            lookUpEdit3.TabIndex = 3;
            // 
            // lkupBranch
            // 
            lkupBranch.Location = new System.Drawing.Point(70, 12);
            lkupBranch.Name = "lkupBranch";
            lkupBranch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            lkupBranch.Properties.NullText = "Chọn Chi Nhánh";
            lkupBranch.Size = new System.Drawing.Size(178, 20);
            lkupBranch.StyleController = layoutControlTop;
            lkupBranch.TabIndex = 0;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem1, layoutControlItem3, layoutControlItem5, layoutControlItem6, layoutControlItem7, layoutControlItem4, layoutControlItem2 });
            Root.Name = "Root";
            Root.Size = new System.Drawing.Size(1234, 104);
            Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = lkupBranch;
            layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new System.Drawing.Size(240, 24);
            layoutControlItem1.Text = "Chi nhánh";
            layoutControlItem1.TextSize = new System.Drawing.Size(54, 14);
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = lookUpEdit3;
            layoutControlItem3.Location = new System.Drawing.Point(0, 24);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new System.Drawing.Size(240, 24);
            layoutControlItem3.Text = "Người tạo";
            layoutControlItem3.TextSize = new System.Drawing.Size(54, 14);
            // 
            // layoutControlItem5
            // 
            layoutControlItem5.Control = chkDraft;
            layoutControlItem5.Location = new System.Drawing.Point(240, 0);
            layoutControlItem5.Name = "layoutControlItem5";
            layoutControlItem5.Size = new System.Drawing.Size(80, 84);
            layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            layoutControlItem6.Control = chkFinish;
            layoutControlItem6.Location = new System.Drawing.Point(320, 0);
            layoutControlItem6.Name = "layoutControlItem6";
            layoutControlItem6.Size = new System.Drawing.Size(92, 84);
            layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            layoutControlItem7.Control = chkCancel;
            layoutControlItem7.Location = new System.Drawing.Point(412, 0);
            layoutControlItem7.Name = "layoutControlItem7";
            layoutControlItem7.Size = new System.Drawing.Size(63, 84);
            layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = dateEdit1;
            layoutControlItem4.Location = new System.Drawing.Point(0, 48);
            layoutControlItem4.Name = "layoutControlItem4";
            layoutControlItem4.Size = new System.Drawing.Size(240, 36);
            layoutControlItem4.Text = "Thời gian";
            layoutControlItem4.TextSize = new System.Drawing.Size(54, 14);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = layoutControl1;
            layoutControlItem2.Location = new System.Drawing.Point(475, 0);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new System.Drawing.Size(739, 84);
            layoutControlItem2.TextVisible = false;
            // 
            // grdControlOrders
            // 
            grdControlOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            grdControlOrders.Location = new System.Drawing.Point(0, 128);
            grdControlOrders.MainView = grdViewOrders;
            grdControlOrders.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grdControlOrders.Name = "grdControlOrders";
            grdControlOrders.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemCheckEdit1, repositoryItemDateEdit1, repositoryItemDateEdit2 });
            grdControlOrders.Size = new System.Drawing.Size(1238, 480);
            grdControlOrders.TabIndex = 1;
            grdControlOrders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grdViewOrders });
            // 
            // grdViewOrders
            // 
            grdViewOrders.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { grdClmCode, purchaseDate, grdClmCustomerName, grdClmTotal, grdClmTotalPayment, grdClmStatus });
            grdViewOrders.GridControl = grdControlOrders;
            grdViewOrders.Name = "grdViewOrders";
            grdViewOrders.OptionsBehavior.Editable = false;
            grdViewOrders.OptionsView.ShowGroupPanel = false;
            grdViewOrders.DoubleClick += grdViewOrders_DoubleClick;
            // 
            // grdClmCode
            // 
            grdClmCode.Caption = "Mã đặt hàng";
            grdClmCode.FieldName = "Code";
            grdClmCode.Name = "grdClmCode";
            grdClmCode.Visible = true;
            grdClmCode.VisibleIndex = 0;
            grdClmCode.Width = 122;
            // 
            // purchaseDate
            // 
            purchaseDate.Caption = "Ngày đạt hàng";
            purchaseDate.FieldName = "PurchaseDate";
            purchaseDate.Name = "purchaseDate";
            purchaseDate.Visible = true;
            purchaseDate.VisibleIndex = 1;
            purchaseDate.Width = 201;
            // 
            // grdClmCustomerName
            // 
            grdClmCustomerName.Caption = "Khách hàng";
            grdClmCustomerName.FieldName = "CustomerName";
            grdClmCustomerName.Name = "grdClmCustomerName";
            grdClmCustomerName.Visible = true;
            grdClmCustomerName.VisibleIndex = 2;
            grdClmCustomerName.Width = 317;
            // 
            // grdClmTotal
            // 
            grdClmTotal.Caption = "Khách cần trả";
            grdClmTotal.FieldName = "Total";
            grdClmTotal.Name = "grdClmTotal";
            grdClmTotal.Visible = true;
            grdClmTotal.VisibleIndex = 3;
            grdClmTotal.Width = 261;
            // 
            // grdClmTotalPayment
            // 
            grdClmTotalPayment.Caption = "Khách đã trả";
            grdClmTotalPayment.FieldName = "TotalPayment";
            grdClmTotalPayment.Name = "grdClmTotalPayment";
            grdClmTotalPayment.Visible = true;
            grdClmTotalPayment.VisibleIndex = 4;
            grdClmTotalPayment.Width = 213;
            // 
            // grdClmStatus
            // 
            grdClmStatus.Caption = "Trạng thái";
            grdClmStatus.FieldName = "StatusValue";
            grdClmStatus.Name = "grdClmStatus";
            grdClmStatus.Visible = true;
            grdClmStatus.VisibleIndex = 5;
            grdClmStatus.Width = 141;
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
            // FrmOrder
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1238, 608);
            Controls.Add(grdControlOrders);
            Controls.Add(groupControl1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmOrder";
            Text = "Danh sách Đơn hàng";
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
        private DevExpress.XtraGrid.Columns.GridColumn grdClmCustomerName;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmTotal;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmTotalPayment;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmStatus;
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
    }
}