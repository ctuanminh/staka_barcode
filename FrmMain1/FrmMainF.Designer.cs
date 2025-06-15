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
            rbOrder = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(components);
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabbedMdiManager1).BeginInit();
            SuspendLayout();
            // 
            // ribbonControl1
            // 
            ribbonControl1.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(35, 32, 35, 32);
            ribbonControl1.ExpandCollapseItem.Id = 0;
            ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { ribbonControl1.ExpandCollapseItem, mbtnOrder, lblTimer, barStaticItem2, mbtnSystem });
            ribbonControl1.Location = new System.Drawing.Point(0, 0);
            ribbonControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ribbonControl1.MaxItemId = 7;
            ribbonControl1.Name = "ribbonControl1";
            ribbonControl1.OptionsMenuMinWidth = 385;
            ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { rbOrder });
            ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.OfficeUniversal;
            ribbonControl1.Size = new System.Drawing.Size(1560, 86);
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
            barStaticItem2.Name = "barStaticItem2";
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
            // rbOrder
            // 
            rbOrder.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroup1 });
            rbOrder.Name = "rbOrder";
            rbOrder.Text = "Home";
            // 
            // ribbonPageGroup1
            // 
            ribbonPageGroup1.ItemLinks.Add(mbtnOrder);
            ribbonPageGroup1.ItemLinks.Add(mbtnSystem);
            ribbonPageGroup1.Name = "ribbonPageGroup1";
            ribbonPageGroup1.Text = "Đơn hàng";
            // 
            // ribbonStatusBar1
            // 
            ribbonStatusBar1.ItemLinks.Add(lblTimer);
            ribbonStatusBar1.ItemLinks.Add(barStaticItem2);
            ribbonStatusBar1.Location = new System.Drawing.Point(0, 703);
            ribbonStatusBar1.Name = "ribbonStatusBar1";
            ribbonStatusBar1.Ribbon = ribbonControl1;
            ribbonStatusBar1.Size = new System.Drawing.Size(1560, 23);
            // 
            // xtraTabbedMdiManager1
            // 
            xtraTabbedMdiManager1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageAndTabControlHeader;
            xtraTabbedMdiManager1.MdiParent = this;
            // 
            // FrmMainF
            // 
            AllowFormGlass = DevExpress.Utils.DefaultBoolean.False;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1560, 726);
            Controls.Add(ribbonStatusBar1);
            Controls.Add(ribbonControl1);
            IsMdiContainer = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FrmMainF";
            Ribbon = ribbonControl1;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            StatusBar = ribbonStatusBar1;
            Text = "Staka 2025";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            Load += FrmMainF_Load;
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabbedMdiManager1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbOrder;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem mbtnOrder;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.BarStaticItem lblTimer;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarButtonItem mbtnSystem;
    }
}