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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPurchase));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            grdControlOrders = new DevExpress.XtraGrid.GridControl();
            grdViewOrders = new DevExpress.XtraGrid.Views.Grid.GridView();
            grdClmCode = new DevExpress.XtraGrid.Columns.GridColumn();
            purchaseDate = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmSupplierName = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmStatusValue = new DevExpress.XtraGrid.Columns.GridColumn();
            clmQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            clmId = new DevExpress.XtraGrid.Columns.GridColumn();
            clmStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            rpAction = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            groupControl1 = new DevExpress.XtraEditors.GroupControl();
            layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            btnAddPurchase = new DevExpress.XtraEditors.SimpleButton();
            txtBranchName = new DevExpress.XtraEditors.TextEdit();
            toPurchaseDate = new DevExpress.XtraEditors.DateEdit();
            btnReloadPurchase = new DevExpress.XtraEditors.SimpleButton();
            chkCancel = new DevExpress.XtraEditors.CheckEdit();
            chkFinish = new DevExpress.XtraEditors.CheckEdit();
            chkDraft = new DevExpress.XtraEditors.CheckEdit();
            fromPurchaseDate = new DevExpress.XtraEditors.DateEdit();
            lookUpEdit3 = new DevExpress.XtraEditors.LookUpEdit();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            groupControl2 = new DevExpress.XtraEditors.GroupControl();
            clmAction = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)grdControlOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grdViewOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rpAction).BeginInit();
            ((System.ComponentModel.ISupportInitialize)groupControl1).BeginInit();
            groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)layoutControlTop).BeginInit();
            layoutControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtBranchName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toPurchaseDate.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toPurchaseDate.Properties.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkCancel.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkFinish.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkDraft.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fromPurchaseDate.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fromPurchaseDate.Properties.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit3.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)groupControl2).BeginInit();
            groupControl2.SuspendLayout();
            SuspendLayout();
            // 
            // grdControlOrders
            // 
            grdControlOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            grdControlOrders.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grdControlOrders.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            grdControlOrders.Location = new System.Drawing.Point(2, 22);
            grdControlOrders.MainView = grdViewOrders;
            grdControlOrders.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grdControlOrders.Name = "grdControlOrders";
            grdControlOrders.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpAction });
            grdControlOrders.Size = new System.Drawing.Size(946, 584);
            grdControlOrders.TabIndex = 1;
            grdControlOrders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grdViewOrders });
            // 
            // grdViewOrders
            // 
            grdViewOrders.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            grdViewOrders.Appearance.HeaderPanel.Options.UseFont = true;
            grdViewOrders.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { grdClmCode, purchaseDate, grdClmSupplierName, grdClmTotal, grdClmStatusValue, clmQuantity, clmId, clmStatus, clmAction });
            grdViewOrders.GridControl = grdControlOrders;
            grdViewOrders.Name = "grdViewOrders";
            grdViewOrders.OptionsBehavior.Editable = false;
            grdViewOrders.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            grdViewOrders.OptionsView.ShowGroupPanel = false;
            grdViewOrders.DoubleClick += grdViewOrders_DoubleClick;
            // 
            // grdClmCode
            // 
            grdClmCode.Caption = "Mã nhập hàng";
            grdClmCode.FieldName = "Code";
            grdClmCode.Name = "grdClmCode";
            grdClmCode.Visible = true;
            grdClmCode.VisibleIndex = 1;
            grdClmCode.Width = 87;
            // 
            // purchaseDate
            // 
            purchaseDate.Caption = "Thời gian";
            purchaseDate.FieldName = "PurchaseDate";
            purchaseDate.Name = "purchaseDate";
            purchaseDate.Visible = true;
            purchaseDate.VisibleIndex = 2;
            purchaseDate.Width = 101;
            // 
            // grdClmSupplierName
            // 
            grdClmSupplierName.Caption = "Nhà cung cấp";
            grdClmSupplierName.FieldName = "SupplierName";
            grdClmSupplierName.Name = "grdClmSupplierName";
            grdClmSupplierName.Visible = true;
            grdClmSupplierName.VisibleIndex = 3;
            grdClmSupplierName.Width = 122;
            // 
            // grdClmTotal
            // 
            grdClmTotal.Caption = "Cần trả NCC";
            grdClmTotal.DisplayFormat.FormatString = "n0";
            grdClmTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdClmTotal.FieldName = "Total";
            grdClmTotal.Name = "grdClmTotal";
            grdClmTotal.Visible = true;
            grdClmTotal.VisibleIndex = 5;
            grdClmTotal.Width = 119;
            // 
            // grdClmStatusValue
            // 
            grdClmStatusValue.Caption = "Trạng thái";
            grdClmStatusValue.FieldName = "StatusValue";
            grdClmStatusValue.Name = "grdClmStatusValue";
            grdClmStatusValue.Visible = true;
            grdClmStatusValue.VisibleIndex = 6;
            grdClmStatusValue.Width = 104;
            // 
            // clmQuantity
            // 
            clmQuantity.Caption = "Tổng số mặt hàng";
            clmQuantity.FieldName = "Quantity";
            clmQuantity.Name = "clmQuantity";
            clmQuantity.Visible = true;
            clmQuantity.VisibleIndex = 4;
            clmQuantity.Width = 224;
            // 
            // clmId
            // 
            clmId.Caption = "ID";
            clmId.FieldName = "Id";
            clmId.Name = "clmId";
            // 
            // clmStatus
            // 
            clmStatus.Caption = "clmStatus";
            clmStatus.FieldName = "Status";
            clmStatus.Name = "clmStatus";
            // 
            // rpAction
            // 
            rpAction.AutoHeight = false;
            editorButtonImageOptions1.Image = (System.Drawing.Image)resources.GetObject("editorButtonImageOptions1.Image");
            rpAction.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Sửa", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "Sửa phiếu nhận hàng", "Sửa", null, DevExpress.Utils.ToolTipAnchor.Default) });
            rpAction.Name = "rpAction";
            rpAction.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // groupControl1
            // 
            groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            groupControl1.AppearanceCaption.Options.UseFont = true;
            groupControl1.Controls.Add(layoutControlTop);
            groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            groupControl1.Location = new System.Drawing.Point(0, 0);
            groupControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupControl1.Name = "groupControl1";
            groupControl1.Size = new System.Drawing.Size(350, 608);
            groupControl1.TabIndex = 2;
            groupControl1.Text = "Lọc phiếu Nhập hàng";
            // 
            // layoutControlTop
            // 
            layoutControlTop.AutoScroll = false;
            layoutControlTop.Controls.Add(btnAddPurchase);
            layoutControlTop.Controls.Add(txtBranchName);
            layoutControlTop.Controls.Add(toPurchaseDate);
            layoutControlTop.Controls.Add(btnReloadPurchase);
            layoutControlTop.Controls.Add(chkCancel);
            layoutControlTop.Controls.Add(chkFinish);
            layoutControlTop.Controls.Add(chkDraft);
            layoutControlTop.Controls.Add(fromPurchaseDate);
            layoutControlTop.Controls.Add(lookUpEdit3);
            layoutControlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            layoutControlTop.Location = new System.Drawing.Point(2, 22);
            layoutControlTop.Name = "layoutControlTop";
            layoutControlTop.Root = Root;
            layoutControlTop.Size = new System.Drawing.Size(346, 584);
            layoutControlTop.TabIndex = 0;
            layoutControlTop.Text = "layoutControl1";
            // 
            // btnAddPurchase
            // 
            btnAddPurchase.Appearance.BackColor = System.Drawing.Color.LightGreen;
            btnAddPurchase.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            btnAddPurchase.Appearance.Options.UseBackColor = true;
            btnAddPurchase.Appearance.Options.UseFont = true;
            btnAddPurchase.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("btnAddPurchase.ImageOptions.Image");
            btnAddPurchase.Location = new System.Drawing.Point(12, 131);
            btnAddPurchase.Name = "btnAddPurchase";
            btnAddPurchase.Size = new System.Drawing.Size(159, 22);
            btnAddPurchase.StyleController = layoutControlTop;
            btnAddPurchase.TabIndex = 11;
            btnAddPurchase.Text = "Nhập hàng";
            btnAddPurchase.Click += btnAddPurchase_Click;
            // 
            // txtBranchName
            // 
            txtBranchName.Location = new System.Drawing.Point(70, 12);
            txtBranchName.Name = "txtBranchName";
            txtBranchName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            txtBranchName.Properties.Appearance.Options.UseFont = true;
            txtBranchName.Size = new System.Drawing.Size(264, 20);
            txtBranchName.StyleController = layoutControlTop;
            txtBranchName.TabIndex = 0;
            // 
            // toPurchaseDate
            // 
            toPurchaseDate.EditValue = null;
            toPurchaseDate.Location = new System.Drawing.Point(70, 84);
            toPurchaseDate.Name = "toPurchaseDate";
            toPurchaseDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            toPurchaseDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            toPurchaseDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            toPurchaseDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            toPurchaseDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            toPurchaseDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            toPurchaseDate.Properties.MaskSettings.Set("mask", "dd/MM/yyyy");
            toPurchaseDate.Size = new System.Drawing.Size(264, 20);
            toPurchaseDate.StyleController = layoutControlTop;
            toPurchaseDate.TabIndex = 3;
            toPurchaseDate.EditValueChanged += toPurchaseDate_EditValueChanged;
            // 
            // btnReloadPurchase
            // 
            btnReloadPurchase.Appearance.BackColor = System.Drawing.Color.Green;
            btnReloadPurchase.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            btnReloadPurchase.Appearance.Options.UseBackColor = true;
            btnReloadPurchase.Appearance.Options.UseFont = true;
            btnReloadPurchase.Cursor = System.Windows.Forms.Cursors.Hand;
            btnReloadPurchase.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("btnReloadPurchase.ImageOptions.Image");
            btnReloadPurchase.Location = new System.Drawing.Point(175, 131);
            btnReloadPurchase.Name = "btnReloadPurchase";
            btnReloadPurchase.Size = new System.Drawing.Size(159, 22);
            btnReloadPurchase.StyleController = layoutControlTop;
            btnReloadPurchase.TabIndex = 9;
            btnReloadPurchase.Text = "Tải lại dữ liệu";
            btnReloadPurchase.Click += btnReloadOrder_Click;
            // 
            // chkCancel
            // 
            chkCancel.Location = new System.Drawing.Point(237, 108);
            chkCancel.Name = "chkCancel";
            chkCancel.Properties.Caption = "Đã huỷ";
            chkCancel.Size = new System.Drawing.Size(97, 19);
            chkCancel.StyleController = layoutControlTop;
            chkCancel.TabIndex = 7;
            chkCancel.CheckedChanged += Handler_CheckedChanged;
            // 
            // chkFinish
            // 
            chkFinish.Location = new System.Drawing.Point(115, 108);
            chkFinish.Name = "chkFinish";
            chkFinish.Properties.Caption = "Đã nhận hàng";
            chkFinish.Size = new System.Drawing.Size(118, 19);
            chkFinish.StyleController = layoutControlTop;
            chkFinish.TabIndex = 6;
            chkFinish.CheckedChanged += Handler_CheckedChanged;
            // 
            // chkDraft
            // 
            chkDraft.EditValue = true;
            chkDraft.Location = new System.Drawing.Point(12, 108);
            chkDraft.Name = "chkDraft";
            chkDraft.Properties.Caption = "Phiếu tạm";
            chkDraft.Size = new System.Drawing.Size(99, 19);
            chkDraft.StyleController = layoutControlTop;
            chkDraft.TabIndex = 5;
            chkDraft.CheckedChanged += Handler_CheckedChanged;
            // 
            // fromPurchaseDate
            // 
            fromPurchaseDate.EditValue = null;
            fromPurchaseDate.Location = new System.Drawing.Point(70, 60);
            fromPurchaseDate.Name = "fromPurchaseDate";
            fromPurchaseDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            fromPurchaseDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            fromPurchaseDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            fromPurchaseDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            fromPurchaseDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            fromPurchaseDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            fromPurchaseDate.Properties.MaskSettings.Set("mask", "dd/MM/yyyy");
            fromPurchaseDate.Size = new System.Drawing.Size(264, 20);
            fromPurchaseDate.StyleController = layoutControlTop;
            fromPurchaseDate.TabIndex = 4;
            fromPurchaseDate.EditValueChanged += fromPurchaseDate_EditValueChanged;
            // 
            // lookUpEdit3
            // 
            lookUpEdit3.EditValue = "<Null>";
            lookUpEdit3.Location = new System.Drawing.Point(70, 36);
            lookUpEdit3.Name = "lookUpEdit3";
            lookUpEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            lookUpEdit3.Properties.NullText = "Chọn Người Tạo";
            lookUpEdit3.Size = new System.Drawing.Size(264, 20);
            lookUpEdit3.StyleController = layoutControlTop;
            lookUpEdit3.TabIndex = 2;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem3, layoutControlItem10, layoutControlItem5, layoutControlItem6, layoutControlItem9, layoutControlItem7, layoutControlItem4, layoutControlItem2, layoutControlItem8 });
            Root.Name = "Root";
            Root.Size = new System.Drawing.Size(346, 584);
            Root.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = lookUpEdit3;
            layoutControlItem3.Location = new System.Drawing.Point(0, 24);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new System.Drawing.Size(326, 24);
            layoutControlItem3.Text = "Người tạo";
            layoutControlItem3.TextSize = new System.Drawing.Size(54, 14);
            // 
            // layoutControlItem10
            // 
            layoutControlItem10.Control = txtBranchName;
            layoutControlItem10.Location = new System.Drawing.Point(0, 0);
            layoutControlItem10.Name = "layoutControlItem10";
            layoutControlItem10.Size = new System.Drawing.Size(326, 24);
            layoutControlItem10.Text = "Chi nhánh";
            layoutControlItem10.TextSize = new System.Drawing.Size(54, 14);
            // 
            // layoutControlItem5
            // 
            layoutControlItem5.Control = chkDraft;
            layoutControlItem5.Location = new System.Drawing.Point(0, 96);
            layoutControlItem5.Name = "layoutControlItem5";
            layoutControlItem5.Size = new System.Drawing.Size(103, 23);
            layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            layoutControlItem6.Control = chkFinish;
            layoutControlItem6.Location = new System.Drawing.Point(103, 96);
            layoutControlItem6.Name = "layoutControlItem6";
            layoutControlItem6.Size = new System.Drawing.Size(122, 23);
            layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            layoutControlItem9.Control = toPurchaseDate;
            layoutControlItem9.Location = new System.Drawing.Point(0, 72);
            layoutControlItem9.Name = "layoutControlItem9";
            layoutControlItem9.Size = new System.Drawing.Size(326, 24);
            layoutControlItem9.Text = "Đến ngày";
            layoutControlItem9.TextSize = new System.Drawing.Size(54, 14);
            // 
            // layoutControlItem7
            // 
            layoutControlItem7.Control = chkCancel;
            layoutControlItem7.Location = new System.Drawing.Point(225, 96);
            layoutControlItem7.Name = "layoutControlItem7";
            layoutControlItem7.Size = new System.Drawing.Size(101, 23);
            layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = fromPurchaseDate;
            layoutControlItem4.Location = new System.Drawing.Point(0, 48);
            layoutControlItem4.Name = "layoutControlItem4";
            layoutControlItem4.Size = new System.Drawing.Size(326, 24);
            layoutControlItem4.Text = "Từ ngày";
            layoutControlItem4.TextSize = new System.Drawing.Size(54, 14);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = btnAddPurchase;
            layoutControlItem2.Location = new System.Drawing.Point(0, 119);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new System.Drawing.Size(163, 445);
            layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            layoutControlItem8.Control = btnReloadPurchase;
            layoutControlItem8.Location = new System.Drawing.Point(163, 119);
            layoutControlItem8.Name = "layoutControlItem8";
            layoutControlItem8.Size = new System.Drawing.Size(163, 445);
            layoutControlItem8.TextVisible = false;
            // 
            // groupControl2
            // 
            groupControl2.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            groupControl2.AppearanceCaption.Options.UseFont = true;
            groupControl2.Controls.Add(grdControlOrders);
            groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupControl2.Location = new System.Drawing.Point(350, 0);
            groupControl2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupControl2.Name = "groupControl2";
            groupControl2.Size = new System.Drawing.Size(950, 608);
            groupControl2.TabIndex = 3;
            groupControl2.Text = "Danh sách phiếu Nhập hàng";
            // 
            // clmAction
            // 
            clmAction.Caption = "Action";
            clmAction.ColumnEdit = rpAction;
            clmAction.FieldName = "Action";
            clmAction.Name = "clmAction";
            clmAction.OptionsColumn.ShowCaption = false;
            clmAction.Visible = true;
            clmAction.VisibleIndex = 0;
            // 
            // FrmPurchase
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1300, 608);
            Controls.Add(groupControl2);
            Controls.Add(groupControl1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPurchase";
            Text = "Danh sách Phiếu Nhập hàng";
            Load += FrmOrder_Load;
            ((System.ComponentModel.ISupportInitialize)grdControlOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)grdViewOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)rpAction).EndInit();
            ((System.ComponentModel.ISupportInitialize)groupControl1).EndInit();
            groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)layoutControlTop).EndInit();
            layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)txtBranchName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)toPurchaseDate.Properties.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)toPurchaseDate.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkCancel.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkFinish.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkDraft.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)fromPurchaseDate.Properties.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)fromPurchaseDate.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit3.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem10).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem9).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem8).EndInit();
            ((System.ComponentModel.ISupportInitialize)groupControl2).EndInit();
            groupControl2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private DevExpress.XtraEditors.LookUpEdit lkBranch;
        private DevExpress.XtraGrid.GridControl grdControlOrders;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewOrders;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmCode;
        private DevExpress.XtraGrid.Columns.GridColumn purchaseDate;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmSupplierName;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmTotal;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmStatusValue;
        private DevExpress.XtraGrid.Columns.GridColumn clmQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn clmId;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControlTop;
        private DevExpress.XtraEditors.TextEdit txtBranchName;
        private DevExpress.XtraEditors.DateEdit toPurchaseDate;
        private DevExpress.XtraEditors.SimpleButton btnReloadPurchase;
        private DevExpress.XtraEditors.CheckEdit chkCancel;
        private DevExpress.XtraEditors.CheckEdit chkFinish;
        private DevExpress.XtraEditors.CheckEdit chkDraft;
        private DevExpress.XtraEditors.DateEdit fromPurchaseDate;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit3;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton btnAddPurchase;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit rpAction;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraGrid.Columns.GridColumn clmStatus;
        private DevExpress.XtraGrid.Columns.GridColumn clmAction;
    }
}