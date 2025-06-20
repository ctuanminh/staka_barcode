namespace FrmMain
{
    partial class FrmReceiverList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReceiverList));
            groupControl1 = new DevExpress.XtraEditors.GroupControl();
            layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            btnReload = new DevExpress.XtraEditors.SimpleButton();
            txtBranchName = new DevExpress.XtraEditors.TextEdit();
            chkCancel = new DevExpress.XtraEditors.CheckEdit();
            chkTransfer = new DevExpress.XtraEditors.CheckEdit();
            chkFinish = new DevExpress.XtraEditors.CheckEdit();
            toTransferDate = new DevExpress.XtraEditors.DateEdit();
            fromTransferDate = new DevExpress.XtraEditors.DateEdit();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            groupControl2 = new DevExpress.XtraEditors.GroupControl();
            grdControlOrders = new DevExpress.XtraGrid.GridControl();
            grdViewOrders = new DevExpress.XtraGrid.Views.Grid.GridView();
            grdClmCode = new DevExpress.XtraGrid.Columns.GridColumn();
            clmDispatchedDate = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmFromBranchName = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmToBranchName = new DevExpress.XtraGrid.Columns.GridColumn();
            grdClmStatusValue = new DevExpress.XtraGrid.Columns.GridColumn();
            clmSum = new DevExpress.XtraGrid.Columns.GridColumn();
            gridClmId = new DevExpress.XtraGrid.Columns.GridColumn();
            gridReceivedDate = new DevExpress.XtraGrid.Columns.GridColumn();
            repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            ((System.ComponentModel.ISupportInitialize)groupControl1).BeginInit();
            groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)layoutControlTop).BeginInit();
            layoutControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)layoutControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtBranchName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkCancel.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkTransfer.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkFinish.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toTransferDate.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toTransferDate.Properties.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fromTransferDate.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fromTransferDate.Properties.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)groupControl2).BeginInit();
            groupControl2.SuspendLayout();
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
            groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            groupControl1.Location = new System.Drawing.Point(0, 0);
            groupControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupControl1.Name = "groupControl1";
            groupControl1.Size = new System.Drawing.Size(346, 608);
            groupControl1.TabIndex = 0;
            groupControl1.Text = "Lọc phiếu Nhận hàng";
            // 
            // layoutControlTop
            // 
            layoutControlTop.AutoScroll = false;
            layoutControlTop.Controls.Add(layoutControl1);
            layoutControlTop.Controls.Add(btnReload);
            layoutControlTop.Controls.Add(txtBranchName);
            layoutControlTop.Controls.Add(chkCancel);
            layoutControlTop.Controls.Add(chkTransfer);
            layoutControlTop.Controls.Add(chkFinish);
            layoutControlTop.Controls.Add(toTransferDate);
            layoutControlTop.Controls.Add(fromTransferDate);
            layoutControlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            layoutControlTop.Location = new System.Drawing.Point(2, 22);
            layoutControlTop.Name = "layoutControlTop";
            layoutControlTop.Root = Root;
            layoutControlTop.Size = new System.Drawing.Size(342, 584);
            layoutControlTop.TabIndex = 0;
            layoutControlTop.Text = "layoutControl1";
            // 
            // layoutControl1
            // 
            layoutControl1.Location = new System.Drawing.Point(12, 107);
            layoutControl1.Name = "layoutControl1";
            layoutControl1.Root = layoutControlGroup1;
            layoutControl1.Size = new System.Drawing.Size(138, 465);
            layoutControl1.TabIndex = 19;
            layoutControl1.Text = "layoutControl1";
            // 
            // layoutControlGroup1
            // 
            layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            layoutControlGroup1.GroupBordersVisible = false;
            layoutControlGroup1.Name = "layoutControlGroup1";
            layoutControlGroup1.Size = new System.Drawing.Size(138, 465);
            layoutControlGroup1.TextVisible = false;
            // 
            // btnReload
            // 
            btnReload.Appearance.BackColor = System.Drawing.Color.Green;
            btnReload.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            btnReload.Appearance.Options.UseBackColor = true;
            btnReload.Appearance.Options.UseFont = true;
            btnReload.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("btnReload.ImageOptions.Image");
            btnReload.Location = new System.Drawing.Point(154, 107);
            btnReload.Name = "btnReload";
            btnReload.Size = new System.Drawing.Size(176, 22);
            btnReload.StyleController = layoutControlTop;
            btnReload.TabIndex = 18;
            btnReload.Text = "Tải dữ liệu";
            btnReload.Click += btnReload_Click;
            // 
            // txtBranchName
            // 
            txtBranchName.Location = new System.Drawing.Point(117, 12);
            txtBranchName.Name = "txtBranchName";
            txtBranchName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            txtBranchName.Properties.Appearance.ForeColor = System.Drawing.Color.OrangeRed;
            txtBranchName.Properties.Appearance.Options.UseFont = true;
            txtBranchName.Properties.Appearance.Options.UseForeColor = true;
            txtBranchName.Size = new System.Drawing.Size(213, 20);
            txtBranchName.StyleController = layoutControlTop;
            txtBranchName.TabIndex = 17;
            // 
            // chkCancel
            // 
            chkCancel.Location = new System.Drawing.Point(220, 84);
            chkCancel.Name = "chkCancel";
            chkCancel.Properties.Caption = "Đã huỷ";
            chkCancel.Size = new System.Drawing.Size(110, 19);
            chkCancel.StyleController = layoutControlTop;
            chkCancel.TabIndex = 7;
            chkCancel.CheckedChanged += Handler_CheckedChanged;
            // 
            // chkTransfer
            // 
            chkTransfer.EditValue = true;
            chkTransfer.Location = new System.Drawing.Point(12, 84);
            chkTransfer.Name = "chkTransfer";
            chkTransfer.Properties.Caption = "Đang chuyển";
            chkTransfer.Size = new System.Drawing.Size(105, 19);
            chkTransfer.StyleController = layoutControlTop;
            chkTransfer.TabIndex = 10;
            chkTransfer.CheckedChanged += Handler_CheckedChanged;
            // 
            // chkFinish
            // 
            chkFinish.Location = new System.Drawing.Point(121, 84);
            chkFinish.Name = "chkFinish";
            chkFinish.Properties.Caption = "Đã nhận";
            chkFinish.Size = new System.Drawing.Size(95, 19);
            chkFinish.StyleController = layoutControlTop;
            chkFinish.TabIndex = 6;
            chkFinish.CheckedChanged += Handler_CheckedChanged;
            // 
            // toTransferDate
            // 
            toTransferDate.EditValue = null;
            toTransferDate.Location = new System.Drawing.Point(117, 60);
            toTransferDate.Name = "toTransferDate";
            toTransferDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            toTransferDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            toTransferDate.Size = new System.Drawing.Size(213, 20);
            toTransferDate.StyleController = layoutControlTop;
            toTransferDate.TabIndex = 14;
            toTransferDate.EditValueChanged += toPurchaseDate_EditValueChanged;
            // 
            // fromTransferDate
            // 
            fromTransferDate.EditValue = null;
            fromTransferDate.Location = new System.Drawing.Point(117, 36);
            fromTransferDate.Name = "fromTransferDate";
            fromTransferDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            fromTransferDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            fromTransferDate.Size = new System.Drawing.Size(213, 20);
            fromTransferDate.StyleController = layoutControlTop;
            fromTransferDate.TabIndex = 4;
            fromTransferDate.EditValueChanged += fromPurchaseDate_EditValueChanged;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem11, layoutControlItem6, layoutControlItem7, layoutControlItem1, layoutControlItem4, layoutControlItem2, layoutControlItem8, layoutControlItem5 });
            Root.Name = "Root";
            Root.Size = new System.Drawing.Size(342, 584);
            Root.TextVisible = false;
            // 
            // layoutControlItem11
            // 
            layoutControlItem11.Control = toTransferDate;
            layoutControlItem11.Location = new System.Drawing.Point(0, 48);
            layoutControlItem11.Name = "layoutControlItem11";
            layoutControlItem11.Size = new System.Drawing.Size(322, 24);
            layoutControlItem11.Text = "Đến ngày";
            layoutControlItem11.TextSize = new System.Drawing.Size(101, 14);
            // 
            // layoutControlItem6
            // 
            layoutControlItem6.Control = chkTransfer;
            layoutControlItem6.Location = new System.Drawing.Point(0, 72);
            layoutControlItem6.Name = "layoutControlItem6";
            layoutControlItem6.Size = new System.Drawing.Size(109, 23);
            layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            layoutControlItem7.Control = chkCancel;
            layoutControlItem7.Location = new System.Drawing.Point(208, 72);
            layoutControlItem7.Name = "layoutControlItem7";
            layoutControlItem7.Size = new System.Drawing.Size(114, 23);
            layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = txtBranchName;
            layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new System.Drawing.Size(322, 24);
            layoutControlItem1.Text = "Chi nhánh làm việc";
            layoutControlItem1.TextSize = new System.Drawing.Size(101, 14);
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = fromTransferDate;
            layoutControlItem4.Location = new System.Drawing.Point(0, 24);
            layoutControlItem4.Name = "layoutControlItem4";
            layoutControlItem4.Size = new System.Drawing.Size(322, 24);
            layoutControlItem4.Text = "Từ ngày";
            layoutControlItem4.TextSize = new System.Drawing.Size(101, 14);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = btnReload;
            layoutControlItem2.Location = new System.Drawing.Point(142, 95);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new System.Drawing.Size(180, 469);
            layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            layoutControlItem8.Control = layoutControl1;
            layoutControlItem8.Location = new System.Drawing.Point(0, 95);
            layoutControlItem8.Name = "layoutControlItem8";
            layoutControlItem8.Size = new System.Drawing.Size(142, 469);
            layoutControlItem8.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            layoutControlItem5.Control = chkFinish;
            layoutControlItem5.Location = new System.Drawing.Point(109, 72);
            layoutControlItem5.Name = "layoutControlItem5";
            layoutControlItem5.Size = new System.Drawing.Size(99, 23);
            layoutControlItem5.TextVisible = false;
            // 
            // groupControl2
            // 
            groupControl2.Controls.Add(grdControlOrders);
            groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupControl2.Location = new System.Drawing.Point(346, 0);
            groupControl2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupControl2.Name = "groupControl2";
            groupControl2.Size = new System.Drawing.Size(804, 608);
            groupControl2.TabIndex = 2;
            groupControl2.Text = "Danh sách Phiếu nhận";
            // 
            // grdControlOrders
            // 
            grdControlOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            grdControlOrders.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grdControlOrders.Location = new System.Drawing.Point(2, 22);
            grdControlOrders.MainView = grdViewOrders;
            grdControlOrders.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            grdControlOrders.Name = "grdControlOrders";
            grdControlOrders.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repositoryItemCheckEdit1, repositoryItemDateEdit1, repositoryItemDateEdit2 });
            grdControlOrders.Size = new System.Drawing.Size(800, 584);
            grdControlOrders.TabIndex = 2;
            grdControlOrders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { grdViewOrders });
            // 
            // grdViewOrders
            // 
            grdViewOrders.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { grdClmCode, clmDispatchedDate, grdClmFromBranchName, grdClmTotal, grdClmToBranchName, grdClmStatusValue, clmSum, gridClmId, gridReceivedDate });
            grdViewOrders.GridControl = grdControlOrders;
            grdViewOrders.Name = "grdViewOrders";
            grdViewOrders.OptionsBehavior.Editable = false;
            grdViewOrders.OptionsDetail.EnableMasterViewMode = false;
            grdViewOrders.OptionsView.ShowGroupPanel = false;
            grdViewOrders.DoubleClick += grdViewOrders_DoubleClick;
            // 
            // grdClmCode
            // 
            grdClmCode.Caption = "Mã chuyển hàng";
            grdClmCode.FieldName = "Code";
            grdClmCode.Name = "grdClmCode";
            grdClmCode.Visible = true;
            grdClmCode.VisibleIndex = 0;
            grdClmCode.Width = 69;
            // 
            // clmDispatchedDate
            // 
            clmDispatchedDate.Caption = "Ngày chuyển";
            clmDispatchedDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
            clmDispatchedDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            clmDispatchedDate.FieldName = "DispatchedDate";
            clmDispatchedDate.Name = "clmDispatchedDate";
            clmDispatchedDate.Visible = true;
            clmDispatchedDate.VisibleIndex = 2;
            // 
            // grdClmFromBranchName
            // 
            grdClmFromBranchName.Caption = "Chi nhánh chuyển";
            grdClmFromBranchName.FieldName = "FromBranchName";
            grdClmFromBranchName.Name = "grdClmFromBranchName";
            grdClmFromBranchName.Visible = true;
            grdClmFromBranchName.VisibleIndex = 4;
            grdClmFromBranchName.Width = 140;
            // 
            // grdClmTotal
            // 
            grdClmTotal.Caption = "Người tạo";
            grdClmTotal.FieldName = "Total";
            grdClmTotal.Name = "grdClmTotal";
            grdClmTotal.Visible = true;
            grdClmTotal.VisibleIndex = 1;
            grdClmTotal.Width = 149;
            // 
            // grdClmToBranchName
            // 
            grdClmToBranchName.AppearanceCell.ForeColor = System.Drawing.Color.OrangeRed;
            grdClmToBranchName.AppearanceCell.Options.UseForeColor = true;
            grdClmToBranchName.Caption = "Chi nhánh nhận";
            grdClmToBranchName.FieldName = "ToBranchName";
            grdClmToBranchName.Name = "grdClmToBranchName";
            grdClmToBranchName.Visible = true;
            grdClmToBranchName.VisibleIndex = 5;
            grdClmToBranchName.Width = 150;
            // 
            // grdClmStatusValue
            // 
            grdClmStatusValue.Caption = "Trạng thái";
            grdClmStatusValue.FieldName = "StatusValue";
            grdClmStatusValue.Name = "grdClmStatusValue";
            grdClmStatusValue.Visible = true;
            grdClmStatusValue.VisibleIndex = 6;
            grdClmStatusValue.Width = 98;
            // 
            // clmSum
            // 
            clmSum.Caption = "Tổng số mặt hàng";
            clmSum.Name = "clmSum";
            clmSum.Visible = true;
            clmSum.VisibleIndex = 7;
            clmSum.Width = 58;
            // 
            // gridClmId
            // 
            gridClmId.Caption = "ID";
            gridClmId.FieldName = "Id";
            gridClmId.Name = "gridClmId";
            gridClmId.Width = 100;
            // 
            // gridReceivedDate
            // 
            gridReceivedDate.Caption = "Ngày nhận";
            gridReceivedDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
            gridReceivedDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            gridReceivedDate.FieldName = "ReceivedDate";
            gridReceivedDate.Name = "gridReceivedDate";
            gridReceivedDate.Visible = true;
            gridReceivedDate.VisibleIndex = 3;
            gridReceivedDate.Width = 49;
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
            // FrmReceiverList
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1150, 608);
            Controls.Add(groupControl2);
            Controls.Add(groupControl1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmReceiverList";
            Text = "Danh sách Phiếu nhận hàng";
            Load += FrmOrder_Load;
            Shown += FrmOrder_Shown;
            ((System.ComponentModel.ISupportInitialize)groupControl1).EndInit();
            groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)layoutControlTop).EndInit();
            layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)layoutControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtBranchName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkCancel.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkTransfer.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkFinish.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)toTransferDate.Properties.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)toTransferDate.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)fromTransferDate.Properties.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)fromTransferDate.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem11).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem8).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).EndInit();
            ((System.ComponentModel.ISupportInitialize)groupControl2).EndInit();
            groupControl2.ResumeLayout(false);
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
        private DevExpress.XtraEditors.DateEdit fromTransferDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.CheckEdit chkCancel;
        private DevExpress.XtraEditors.CheckEdit chkFinish;
        private DevExpress.XtraEditors.CheckEdit chkTransfer;
        private DevExpress.XtraEditors.CheckEdit chkDateReceived;
        private DevExpress.XtraEditors.CheckEdit chkDateTranfer;
        private DevExpress.XtraEditors.DateEdit toTransferDate;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl grdControlOrders;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewOrders;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmCode;
        private DevExpress.XtraGrid.Columns.GridColumn clmDispatchedDate;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmFromBranchName;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmTotal;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmToBranchName;
        private DevExpress.XtraGrid.Columns.GridColumn grdClmStatusValue;
        private DevExpress.XtraGrid.Columns.GridColumn clmSum;
        private DevExpress.XtraGrid.Columns.GridColumn gridClmId;
        private DevExpress.XtraGrid.Columns.GridColumn gridReceivedDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.TextEdit txtBranchName;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton btnReload;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
    }
}