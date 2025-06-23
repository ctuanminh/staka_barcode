namespace FrmMain
{
    partial class FrmMainF
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMainF));
            ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            mbtnOrder = new DevExpress.XtraBars.BarButtonItem();
            lblTimer = new DevExpress.XtraBars.BarStaticItem();
            barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            mbtnSystem = new DevExpress.XtraBars.BarButtonItem();
            mbtcPurchase = new DevExpress.XtraBars.BarButtonItem();
            mbtnTranfer = new DevExpress.XtraBars.BarButtonItem();
            barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            barBranch = new DevExpress.XtraBars.BarEditItem();
            rpLkpBranch = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            mbtnLogout = new DevExpress.XtraBars.BarButtonItem();
            mbtnReceiver = new DevExpress.XtraBars.BarButtonItem();
            bLblVersion = new DevExpress.XtraBars.BarStaticItem();
            bLblComputerName = new DevExpress.XtraBars.BarStaticItem();
            rbOrder = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            TabMdiManager = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(components);
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rpLkpBranch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TabMdiManager).BeginInit();
            SuspendLayout();
            // 
            // ribbonControl1
            // 
            ribbonControl1.ExpandCollapseItem.Id = 0;
            ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { ribbonControl1.ExpandCollapseItem, mbtnOrder, lblTimer, barStaticItem2, mbtnSystem, mbtcPurchase, mbtnTranfer, barSubItem1, barBranch, mbtnLogout, mbtnReceiver, bLblVersion, bLblComputerName });
            ribbonControl1.Location = new System.Drawing.Point(0, 0);
            ribbonControl1.MaxItemId = 22;
            ribbonControl1.Name = "ribbonControl1";
            ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { rbOrder });
            ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpLkpBranch });
            ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.OfficeUniversal;
            ribbonControl1.Size = new System.Drawing.Size(890, 89);
            ribbonControl1.StatusBar = ribbonStatusBar1;
            // 
            // mbtnOrder
            // 
            mbtnOrder.Caption = "Đơn hàng";
            mbtnOrder.Id = 1;
            mbtnOrder.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("mbtnOrder.ImageOptions.Image");
            mbtnOrder.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("mbtnOrder.ImageOptions.LargeImage");
            mbtnOrder.Name = "mbtnOrder";
            mbtnOrder.ItemClick += mButtonItem_ItemClick;
            // 
            // lblTimer
            // 
            lblTimer.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            lblTimer.Caption = "lblTime";
            lblTimer.Id = 4;
            lblTimer.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("lblTimer.ImageOptions.Image");
            lblTimer.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("lblTimer.ImageOptions.LargeImage");
            lblTimer.Name = "lblTimer";
            lblTimer.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // barStaticItem2
            // 
            barStaticItem2.Caption = "Công ty Cổ phần Staka";
            barStaticItem2.Id = 5;
            barStaticItem2.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("barStaticItem2.ImageOptions.Image");
            barStaticItem2.Name = "barStaticItem2";
            barStaticItem2.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // mbtnSystem
            // 
            mbtnSystem.Caption = "Hệ thống";
            mbtnSystem.Id = 6;
            mbtnSystem.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("mbtnSystem.ImageOptions.Image");
            mbtnSystem.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("mbtnSystem.ImageOptions.LargeImage");
            mbtnSystem.Name = "mbtnSystem";
            mbtnSystem.ItemClick += mButtonItem_ItemClick;
            // 
            // mbtcPurchase
            // 
            mbtcPurchase.Caption = "Nhập hàng";
            mbtcPurchase.Id = 7;
            mbtcPurchase.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("mbtcPurchase.ImageOptions.Image");
            mbtcPurchase.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("mbtcPurchase.ImageOptions.LargeImage");
            mbtcPurchase.Name = "mbtcPurchase";
            mbtcPurchase.ItemClick += mButtonItem_ItemClick;
            // 
            // mbtnTranfer
            // 
            mbtnTranfer.Caption = "Chuyển hàng";
            mbtnTranfer.Id = 8;
            mbtnTranfer.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("mbtnTranfer.ImageOptions.Image");
            mbtnTranfer.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("mbtnTranfer.ImageOptions.LargeImage");
            mbtnTranfer.Name = "mbtnTranfer";
            mbtnTranfer.ItemClick += mButtonItem_ItemClick;
            // 
            // barSubItem1
            // 
            barSubItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barSubItem1.Caption = "barSubItem1";
            barSubItem1.Id = 10;
            barSubItem1.Name = "barSubItem1";
            // 
            // barBranch
            // 
            barBranch.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barBranch.Caption = "Chi nhánh làm việc";
            barBranch.Edit = rpLkpBranch;
            barBranch.EditWidth = 250;
            barBranch.Id = 17;
            barBranch.Name = "barBranch";
            // 
            // rpLkpBranch
            // 
            rpLkpBranch.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            rpLkpBranch.Appearance.Options.UseFont = true;
            rpLkpBranch.AutoHeight = false;
            rpLkpBranch.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            rpLkpBranch.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] { new DevExpress.XtraEditors.Controls.LookUpColumnInfo("BranchName", "") });
            rpLkpBranch.DisplayMember = "BranchName";
            rpLkpBranch.Name = "rpLkpBranch";
            rpLkpBranch.NullText = "Chọn Chi Nhánh";
            rpLkpBranch.ShowFooter = false;
            rpLkpBranch.ShowHeader = false;
            rpLkpBranch.ValueMember = "BranchId";
            // 
            // mbtnLogout
            // 
            mbtnLogout.Caption = "Đăng xuất";
            mbtnLogout.Id = 18;
            mbtnLogout.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("mbtnLogout.ImageOptions.Image");
            mbtnLogout.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("mbtnLogout.ImageOptions.LargeImage");
            mbtnLogout.Name = "mbtnLogout";
            mbtnLogout.ItemClick += mButtonItem_ItemClick;
            // 
            // mbtnReceiver
            // 
            mbtnReceiver.Caption = "Nhận hàng chuyển";
            mbtnReceiver.Id = 19;
            mbtnReceiver.ImageOptions.Image = (System.Drawing.Image)resources.GetObject("mbtnReceiver.ImageOptions.Image");
            mbtnReceiver.ImageOptions.LargeImage = (System.Drawing.Image)resources.GetObject("mbtnReceiver.ImageOptions.LargeImage");
            mbtnReceiver.Name = "mbtnReceiver";
            mbtnReceiver.ItemClick += mButtonItem_ItemClick;
            // 
            // bLblVersion
            // 
            bLblVersion.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            bLblVersion.Caption = "barStaticItem1";
            bLblVersion.Id = 20;
            bLblVersion.Name = "bLblVersion";
            // 
            // bLblComputerName
            // 
            bLblComputerName.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            bLblComputerName.Caption = "bLblComputerName";
            bLblComputerName.Id = 21;
            bLblComputerName.Name = "bLblComputerName";
            // 
            // rbOrder
            // 
            rbOrder.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroup1 });
            rbOrder.Name = "rbOrder";
            rbOrder.Text = "Home";
            // 
            // ribbonPageGroup1
            // 
            ribbonPageGroup1.ItemLinks.Add(mbtnOrder);
            ribbonPageGroup1.ItemLinks.Add(mbtcPurchase);
            ribbonPageGroup1.ItemLinks.Add(mbtnTranfer);
            ribbonPageGroup1.ItemLinks.Add(mbtnReceiver);
            ribbonPageGroup1.ItemLinks.Add(mbtnSystem);
            ribbonPageGroup1.ItemLinks.Add(mbtnLogout);
            ribbonPageGroup1.Name = "ribbonPageGroup1";
            ribbonPageGroup1.Text = "Đơn hàng";
            // 
            // ribbonStatusBar1
            // 
            ribbonStatusBar1.ItemLinks.Add(barBranch);
            ribbonStatusBar1.ItemLinks.Add(bLblComputerName);
            ribbonStatusBar1.ItemLinks.Add(bLblVersion);
            ribbonStatusBar1.ItemLinks.Add(lblTimer);
            ribbonStatusBar1.ItemLinks.Add(barStaticItem2);
            ribbonStatusBar1.Location = new System.Drawing.Point(0, 650);
            ribbonStatusBar1.Name = "ribbonStatusBar1";
            ribbonStatusBar1.Ribbon = ribbonControl1;
            ribbonStatusBar1.Size = new System.Drawing.Size(890, 24);
            // 
            // TabMdiManager
            // 
            TabMdiManager.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageAndTabControlHeader;
            TabMdiManager.MdiParent = this;
            // 
            // FrmMainF
            // 
            AllowFormGlass = DevExpress.Utils.DefaultBoolean.False;
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(890, 674);
            Controls.Add(ribbonStatusBar1);
            Controls.Add(ribbonControl1);
            IconOptions.Icon = (System.Drawing.Icon)resources.GetObject("FrmMainF.IconOptions.Icon");
            IsMdiContainer = true;
            Name = "FrmMainF";
            Ribbon = ribbonControl1;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            StatusBar = ribbonStatusBar1;
            Text = "Staka 2025";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            Load += FrmMainF_Load;
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)rpLkpBranch).EndInit();
            ((System.ComponentModel.ISupportInitialize)TabMdiManager).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbOrder;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem mbtnOrder;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager TabMdiManager;
        private DevExpress.XtraBars.BarStaticItem lblTimer;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarButtonItem mbtnSystem;
        private DevExpress.XtraBars.BarButtonItem mbtcPurchase;
        private DevExpress.XtraBars.BarButtonItem mbtnTranfer;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarEditItem barBranch;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpLkpBranch;
        private DevExpress.XtraBars.BarButtonItem mbtnLogout;
        private DevExpress.XtraBars.BarButtonItem mbtnReceiver;
        private DevExpress.XtraBars.BarStaticItem bLblVersion;
        private DevExpress.XtraBars.BarStaticItem bLblComputerName;
    }
}