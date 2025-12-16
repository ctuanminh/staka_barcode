namespace FrmMain
{
    partial class FrmProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProduct));
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
            clmAction = new DevExpress.XtraGrid.Columns.GridColumn();
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
            grdControlOrders.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            grdControlOrders.Location = new System.Drawing.Point(2, 23);
            grdControlOrders.MainView = grdViewOrders;
            grdControlOrders.Name = "grdControlOrders";
            grdControlOrders.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpAction });
            grdControlOrders.Size = new System.Drawing.Size(810, 540);
            grdControlOrders.TabIndex = 1;
            grdControlOrders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grdViewOrders });
            // 
            // grdViewOrders
            // 
            grdViewOrders.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            grdViewOrders.Appearance.HeaderPanel.Options.UseFont = true;
            grdViewOrders.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { grdClmCode, purchaseDate, grdClmSupplierName, grdClmTotal, grdClmStatusValue, clmQuantity, clmId, clmStatus, clmAction });
            grdViewOrders.DetailHeight = 325;
            grdViewOrders.GridControl = grdControlOrders;
            grdViewOrders.Name = "grdViewOrders";
            grdViewOrders.OptionsEditForm.PopupEditFormWidth = 686;
            grdViewOrders.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            grdViewOrders.OptionsView.ShowGroupPanel = false;
            // 
            // grdClmCode
            // 
            grdClmCode.Caption = "Mã nhập hàng";
            grdClmCode.FieldName = "Code";
            grdClmCode.MinWidth = 17;
            grdClmCode.Name = "grdClmCode";
            grdClmCode.OptionsColumn.AllowEdit = false;
            grdClmCode.Visible = true;
            grdClmCode.VisibleIndex = 1;
            // 
            // purchaseDate
            // 
            purchaseDate.Caption = "Thời gian";
            purchaseDate.FieldName = "PurchaseDate";
            purchaseDate.MinWidth = 17;
            purchaseDate.Name = "purchaseDate";
            purchaseDate.OptionsColumn.AllowEdit = false;
            purchaseDate.Visible = true;
            purchaseDate.VisibleIndex = 2;
            purchaseDate.Width = 87;
            // 
            // grdClmSupplierName
            // 
            grdClmSupplierName.Caption = "Nhà cung cấp";
            grdClmSupplierName.FieldName = "SupplierName";
            grdClmSupplierName.MinWidth = 17;
            grdClmSupplierName.Name = "grdClmSupplierName";
            grdClmSupplierName.OptionsColumn.AllowEdit = false;
            grdClmSupplierName.Visible = true;
            grdClmSupplierName.VisibleIndex = 3;
            grdClmSupplierName.Width = 105;
            // 
            // grdClmTotal
            // 
            grdClmTotal.Caption = "Cần trả NCC";
            grdClmTotal.DisplayFormat.FormatString = "n0";
            grdClmTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            grdClmTotal.FieldName = "Total";
            grdClmTotal.MinWidth = 17;
            grdClmTotal.Name = "grdClmTotal";
            grdClmTotal.OptionsColumn.AllowEdit = false;
            grdClmTotal.Visible = true;
            grdClmTotal.VisibleIndex = 5;
            grdClmTotal.Width = 102;
            // 
            // grdClmStatusValue
            // 
            grdClmStatusValue.Caption = "Trạng thái";
            grdClmStatusValue.FieldName = "StatusValue";
            grdClmStatusValue.MinWidth = 17;
            grdClmStatusValue.Name = "grdClmStatusValue";
            grdClmStatusValue.OptionsColumn.AllowEdit = false;
            grdClmStatusValue.Visible = true;
            grdClmStatusValue.VisibleIndex = 6;
            grdClmStatusValue.Width = 89;
            // 
            // clmQuantity
            // 
            clmQuantity.Caption = "Tổng số mặt hàng";
            clmQuantity.FieldName = "Quantity";
            clmQuantity.MinWidth = 17;
            clmQuantity.Name = "clmQuantity";
            clmQuantity.OptionsColumn.AllowEdit = false;
            clmQuantity.Visible = true;
            clmQuantity.VisibleIndex = 4;
            clmQuantity.Width = 192;
            // 
            // clmId
            // 
            clmId.Caption = "ID";
            clmId.FieldName = "Id";
            clmId.MinWidth = 17;
            clmId.Name = "clmId";
            clmId.OptionsColumn.AllowEdit = false;
            clmId.Width = 64;
            // 
            // clmStatus
            // 
            clmStatus.Caption = "clmStatus";
            clmStatus.FieldName = "Status";
            clmStatus.MinWidth = 17;
            clmStatus.Name = "clmStatus";
            clmStatus.OptionsColumn.AllowEdit = false;
            clmStatus.Width = 64;
            // 
            // clmAction
            // 
            clmAction.Caption = "Action";
            clmAction.ColumnEdit = rpAction;
            clmAction.FieldName = "Action";
            clmAction.MinWidth = 17;
            clmAction.Name = "clmAction";
            clmAction.OptionsColumn.ShowCaption = false;
            clmAction.Visible = true;
            clmAction.VisibleIndex = 0;
            clmAction.Width = 64;
            // 
            // rpAction
            // 
            rpAction.AutoHeight = false;
            editorButtonImageOptions1.Image = (System.Drawing.Image)resources.GetObject("editorButtonImageOptions1.Image");
            rpAction.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Sửa", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "Sửa phiếu nhận hàng", "Sửa", null, DevExpress.Utils.ToolTipAnchor.Default) });
            rpAction.Name = "rpAction";
            rpAction.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            rpAction.ButtonClick += rpBtnAction_ButtonClick;
            // 
            // groupControl1
            // 
            groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            groupControl1.AppearanceCaption.Options.UseFont = true;
            groupControl1.Controls.Add(layoutControlTop);
            groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            groupControl1.Location = new System.Drawing.Point(0, 0);
            groupControl1.Name = "groupControl1";
            groupControl1.Size = new System.Drawing.Size(300, 565);
            groupControl1.TabIndex = 2;
            groupControl1.Text = "Lọc Sản phẩm";
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
            layoutControlTop.Location = new System.Drawing.Point(2, 23);
            layoutControlTop.Name = "layoutControlTop";
            layoutControlTop.Root = Root;
            layoutControlTop.Size = new System.Drawing.Size(296, 540);
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
            btnAddPurchase.Location = new System.Drawing.Point(11, 131);
            btnAddPurchase.Name = "btnAddPurchase";
            btnAddPurchase.Size = new System.Drawing.Size(135, 22);
            btnAddPurchase.StyleController = layoutControlTop;
            btnAddPurchase.TabIndex = 11;
            btnAddPurchase.Text = "Nhập hàng";
            btnAddPurchase.Click += btnAddPurchase_Click;
            // 
            // txtBranchName
            // 
            txtBranchName.Location = new System.Drawing.Point(69, 11);
            txtBranchName.Name = "txtBranchName";
            txtBranchName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            txtBranchName.Properties.Appearance.Options.UseFont = true;
            txtBranchName.Size = new System.Drawing.Size(216, 20);
            txtBranchName.StyleController = layoutControlTop;
            txtBranchName.TabIndex = 0;
            // 
            // toPurchaseDate
            // 
            toPurchaseDate.EditValue = null;
            toPurchaseDate.Location = new System.Drawing.Point(69, 83);
            toPurchaseDate.Name = "toPurchaseDate";
            toPurchaseDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            toPurchaseDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            toPurchaseDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            toPurchaseDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            toPurchaseDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            toPurchaseDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            toPurchaseDate.Properties.MaskSettings.Set("mask", "dd/MM/yyyy");
            toPurchaseDate.Size = new System.Drawing.Size(216, 20);
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
            btnReloadPurchase.Location = new System.Drawing.Point(150, 131);
            btnReloadPurchase.Name = "btnReloadPurchase";
            btnReloadPurchase.Size = new System.Drawing.Size(135, 22);
            btnReloadPurchase.StyleController = layoutControlTop;
            btnReloadPurchase.TabIndex = 9;
            btnReloadPurchase.Text = "Tải lại dữ liệu";
            btnReloadPurchase.Click += btnReloadOrder_Click;
            // 
            // chkCancel
            // 
            chkCancel.Location = new System.Drawing.Point(203, 107);
            chkCancel.Name = "chkCancel";
            chkCancel.Properties.Caption = "Đã huỷ";
            chkCancel.Size = new System.Drawing.Size(82, 20);
            chkCancel.StyleController = layoutControlTop;
            chkCancel.TabIndex = 7;
            chkCancel.CheckedChanged += Handler_CheckedChanged;
            // 
            // chkFinish
            // 
            chkFinish.Location = new System.Drawing.Point(99, 107);
            chkFinish.Name = "chkFinish";
            chkFinish.Properties.Caption = "Đã nhận hàng";
            chkFinish.Size = new System.Drawing.Size(100, 20);
            chkFinish.StyleController = layoutControlTop;
            chkFinish.TabIndex = 6;
            chkFinish.CheckedChanged += Handler_CheckedChanged;
            // 
            // chkDraft
            // 
            chkDraft.EditValue = true;
            chkDraft.Location = new System.Drawing.Point(11, 107);
            chkDraft.Name = "chkDraft";
            chkDraft.Properties.Caption = "Phiếu tạm";
            chkDraft.Size = new System.Drawing.Size(84, 20);
            chkDraft.StyleController = layoutControlTop;
            chkDraft.TabIndex = 5;
            chkDraft.CheckedChanged += Handler_CheckedChanged;
            // 
            // fromPurchaseDate
            // 
            fromPurchaseDate.EditValue = null;
            fromPurchaseDate.Location = new System.Drawing.Point(69, 59);
            fromPurchaseDate.Name = "fromPurchaseDate";
            fromPurchaseDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            fromPurchaseDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            fromPurchaseDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            fromPurchaseDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            fromPurchaseDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            fromPurchaseDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            fromPurchaseDate.Properties.MaskSettings.Set("mask", "dd/MM/yyyy");
            fromPurchaseDate.Size = new System.Drawing.Size(216, 20);
            fromPurchaseDate.StyleController = layoutControlTop;
            fromPurchaseDate.TabIndex = 4;
            fromPurchaseDate.EditValueChanged += fromPurchaseDate_EditValueChanged;
            // 
            // lookUpEdit3
            // 
            lookUpEdit3.EditValue = "<Null>";
            lookUpEdit3.Location = new System.Drawing.Point(69, 35);
            lookUpEdit3.Name = "lookUpEdit3";
            lookUpEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            lookUpEdit3.Properties.NullText = "Chọn Người Tạo";
            lookUpEdit3.Size = new System.Drawing.Size(216, 20);
            lookUpEdit3.StyleController = layoutControlTop;
            lookUpEdit3.TabIndex = 2;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem3, layoutControlItem10, layoutControlItem5, layoutControlItem6, layoutControlItem9, layoutControlItem7, layoutControlItem4, layoutControlItem2, layoutControlItem8 });
            Root.Name = "Root";
            Root.Size = new System.Drawing.Size(296, 540);
            Root.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = lookUpEdit3;
            layoutControlItem3.Location = new System.Drawing.Point(0, 24);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new System.Drawing.Size(278, 24);
            layoutControlItem3.Text = "Người tạo";
            layoutControlItem3.TextSize = new System.Drawing.Size(48, 13);
            // 
            // layoutControlItem10
            // 
            layoutControlItem10.Control = txtBranchName;
            layoutControlItem10.Location = new System.Drawing.Point(0, 0);
            layoutControlItem10.Name = "layoutControlItem10";
            layoutControlItem10.Size = new System.Drawing.Size(278, 24);
            layoutControlItem10.Text = "Chi nhánh";
            layoutControlItem10.TextSize = new System.Drawing.Size(48, 13);
            // 
            // layoutControlItem5
            // 
            layoutControlItem5.Control = chkDraft;
            layoutControlItem5.Location = new System.Drawing.Point(0, 96);
            layoutControlItem5.Name = "layoutControlItem5";
            layoutControlItem5.Size = new System.Drawing.Size(88, 24);
            layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            layoutControlItem6.Control = chkFinish;
            layoutControlItem6.Location = new System.Drawing.Point(88, 96);
            layoutControlItem6.Name = "layoutControlItem6";
            layoutControlItem6.Size = new System.Drawing.Size(104, 24);
            layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            layoutControlItem9.Control = toPurchaseDate;
            layoutControlItem9.Location = new System.Drawing.Point(0, 72);
            layoutControlItem9.Name = "layoutControlItem9";
            layoutControlItem9.Size = new System.Drawing.Size(278, 24);
            layoutControlItem9.Text = "Đến ngày";
            layoutControlItem9.TextSize = new System.Drawing.Size(48, 13);
            // 
            // layoutControlItem7
            // 
            layoutControlItem7.Control = chkCancel;
            layoutControlItem7.Location = new System.Drawing.Point(192, 96);
            layoutControlItem7.Name = "layoutControlItem7";
            layoutControlItem7.Size = new System.Drawing.Size(86, 24);
            layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = fromPurchaseDate;
            layoutControlItem4.Location = new System.Drawing.Point(0, 48);
            layoutControlItem4.Name = "layoutControlItem4";
            layoutControlItem4.Size = new System.Drawing.Size(278, 24);
            layoutControlItem4.Text = "Từ ngày";
            layoutControlItem4.TextSize = new System.Drawing.Size(48, 13);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = btnAddPurchase;
            layoutControlItem2.Location = new System.Drawing.Point(0, 120);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new System.Drawing.Size(139, 402);
            layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            layoutControlItem8.Control = btnReloadPurchase;
            layoutControlItem8.Location = new System.Drawing.Point(139, 120);
            layoutControlItem8.Name = "layoutControlItem8";
            layoutControlItem8.Size = new System.Drawing.Size(139, 402);
            layoutControlItem8.TextVisible = false;
            // 
            // groupControl2
            // 
            groupControl2.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            groupControl2.AppearanceCaption.Options.UseFont = true;
            groupControl2.Controls.Add(grdControlOrders);
            groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupControl2.Location = new System.Drawing.Point(300, 0);
            groupControl2.Name = "groupControl2";
            groupControl2.Size = new System.Drawing.Size(814, 565);
            groupControl2.TabIndex = 3;
            groupControl2.Text = "Danh sách Sản phẩm";
            // 
            // FrmProduct
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1114, 565);
            Controls.Add(groupControl2);
            Controls.Add(groupControl1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmProduct";
            Text = "Danh sách Sản phẩm";
            Load += FrmProduct_Load;
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