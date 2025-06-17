namespace FrmMain
{
    partial class FrmSystem
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
            groupControlTop = new DevExpress.XtraEditors.GroupControl();
            layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            btnSyncProduct = new DevExpress.XtraEditors.SimpleButton();
            btnSynBranch = new DevExpress.XtraEditors.SimpleButton();
            btnSyncRole = new DevExpress.XtraEditors.SimpleButton();
            btnSyncCustomer = new DevExpress.XtraEditors.SimpleButton();
            btnSyncUsers = new DevExpress.XtraEditors.SimpleButton();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            lookUpEdit1 = new DevExpress.XtraEditors.LookUpEdit();
            layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)groupControlTop).BeginInit();
            groupControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)layoutControl1).BeginInit();
            layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).BeginInit();
            SuspendLayout();
            // 
            // groupControlTop
            // 
            groupControlTop.Controls.Add(layoutControl1);
            groupControlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            groupControlTop.Location = new System.Drawing.Point(0, 0);
            groupControlTop.Name = "groupControlTop";
            groupControlTop.ShowCaption = false;
            groupControlTop.Size = new System.Drawing.Size(1181, 462);
            groupControlTop.TabIndex = 0;
            groupControlTop.Text = "groupControlTop";
            // 
            // layoutControl1
            // 
            layoutControl1.Controls.Add(simpleButton1);
            layoutControl1.Controls.Add(lookUpEdit1);
            layoutControl1.Controls.Add(btnSyncProduct);
            layoutControl1.Controls.Add(btnSynBranch);
            layoutControl1.Controls.Add(btnSyncRole);
            layoutControl1.Controls.Add(btnSyncCustomer);
            layoutControl1.Controls.Add(btnSyncUsers);
            layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            layoutControl1.Location = new System.Drawing.Point(2, 2);
            layoutControl1.Name = "layoutControl1";
            layoutControl1.Root = Root;
            layoutControl1.Size = new System.Drawing.Size(1177, 458);
            layoutControl1.TabIndex = 0;
            layoutControl1.Text = "layoutControl1";
            // 
            // btnSyncProduct
            // 
            btnSyncProduct.Location = new System.Drawing.Point(1053, 12);
            btnSyncProduct.Name = "btnSyncProduct";
            btnSyncProduct.Size = new System.Drawing.Size(112, 22);
            btnSyncProduct.StyleController = layoutControl1;
            btnSyncProduct.TabIndex = 6;
            btnSyncProduct.Text = "Đồng bộ sản phẩm";
            btnSyncProduct.Click += btnSyncProduct_Click;
            // 
            // btnSynBranch
            // 
            btnSynBranch.Location = new System.Drawing.Point(935, 12);
            btnSynBranch.Name = "btnSynBranch";
            btnSynBranch.Size = new System.Drawing.Size(114, 22);
            btnSynBranch.StyleController = layoutControl1;
            btnSynBranch.TabIndex = 5;
            btnSynBranch.Text = "Đồng bộ Chi nhánh";
            btnSynBranch.Click += btnSynBranch_Click;
            // 
            // btnSyncRole
            // 
            btnSyncRole.Location = new System.Drawing.Point(848, 12);
            btnSyncRole.Name = "btnSyncRole";
            btnSyncRole.Size = new System.Drawing.Size(83, 22);
            btnSyncRole.StyleController = layoutControl1;
            btnSyncRole.TabIndex = 4;
            btnSyncRole.Text = "Đồng bộ Role";
            btnSyncRole.Click += btnSyncRole_Click;
            // 
            // btnSyncCustomer
            // 
            btnSyncCustomer.Location = new System.Drawing.Point(732, 12);
            btnSyncCustomer.Name = "btnSyncCustomer";
            btnSyncCustomer.Size = new System.Drawing.Size(112, 22);
            btnSyncCustomer.StyleController = layoutControl1;
            btnSyncCustomer.TabIndex = 3;
            btnSyncCustomer.Text = "Đồng bộ Customer";
            btnSyncCustomer.Click += btnSyncCustomer_Click;
            // 
            // btnSyncUsers
            // 
            btnSyncUsers.Location = new System.Drawing.Point(644, 12);
            btnSyncUsers.Name = "btnSyncUsers";
            btnSyncUsers.Size = new System.Drawing.Size(84, 22);
            btnSyncUsers.StyleController = layoutControl1;
            btnSyncUsers.TabIndex = 2;
            btnSyncUsers.Text = "Đồng bộ User";
            btnSyncUsers.Click += btnSyncUsers_Click;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem1, emptySpaceItem1, layoutControlItem2, layoutControlItem3, layoutControlItem4, layoutControlItem5, layoutControlItem6, layoutControlItem7 });
            Root.Name = "Root";
            Root.Size = new System.Drawing.Size(1177, 458);
            Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = btnSyncUsers;
            layoutControlItem1.Location = new System.Drawing.Point(632, 0);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new System.Drawing.Size(88, 26);
            layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            emptySpaceItem1.Location = new System.Drawing.Point(0, 26);
            emptySpaceItem1.Name = "emptySpaceItem1";
            emptySpaceItem1.Size = new System.Drawing.Size(1157, 412);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = btnSyncCustomer;
            layoutControlItem2.Location = new System.Drawing.Point(720, 0);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new System.Drawing.Size(116, 26);
            layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = btnSyncRole;
            layoutControlItem3.Location = new System.Drawing.Point(836, 0);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new System.Drawing.Size(87, 26);
            layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = btnSynBranch;
            layoutControlItem4.Location = new System.Drawing.Point(923, 0);
            layoutControlItem4.Name = "layoutControlItem4";
            layoutControlItem4.Size = new System.Drawing.Size(118, 26);
            layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            layoutControlItem5.Control = btnSyncProduct;
            layoutControlItem5.Location = new System.Drawing.Point(1041, 0);
            layoutControlItem5.Name = "layoutControlItem5";
            layoutControlItem5.Size = new System.Drawing.Size(116, 26);
            layoutControlItem5.TextVisible = false;
            // 
            // lookUpEdit1
            // 
            lookUpEdit1.Location = new System.Drawing.Point(117, 12);
            lookUpEdit1.Name = "lookUpEdit1";
            lookUpEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            lookUpEdit1.Properties.NullText = "Chọn Chi nhánh làm việc";
            lookUpEdit1.Size = new System.Drawing.Size(338, 20);
            lookUpEdit1.StyleController = layoutControl1;
            lookUpEdit1.TabIndex = 0;
            // 
            // layoutControlItem6
            // 
            layoutControlItem6.Control = lookUpEdit1;
            layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            layoutControlItem6.Name = "layoutControlItem6";
            layoutControlItem6.Size = new System.Drawing.Size(447, 26);
            layoutControlItem6.Text = "Chi nhánh làm việc";
            layoutControlItem6.TextSize = new System.Drawing.Size(101, 14);
            // 
            // simpleButton1
            // 
            simpleButton1.Location = new System.Drawing.Point(459, 12);
            simpleButton1.Name = "simpleButton1";
            simpleButton1.Size = new System.Drawing.Size(181, 22);
            simpleButton1.StyleController = layoutControl1;
            simpleButton1.TabIndex = 7;
            simpleButton1.Text = "Lưu cài đặt";
            // 
            // layoutControlItem7
            // 
            layoutControlItem7.Control = simpleButton1;
            layoutControlItem7.Location = new System.Drawing.Point(447, 0);
            layoutControlItem7.Name = "layoutControlItem7";
            layoutControlItem7.Size = new System.Drawing.Size(185, 26);
            layoutControlItem7.TextVisible = false;
            // 
            // FrmSystem
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1181, 462);
            Controls.Add(groupControlTop);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FrmSystem";
            Text = "Hệ thống";
            Load += FrmSystem_Load;
            ((System.ComponentModel.ISupportInitialize)groupControlTop).EndInit();
            groupControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)layoutControl1).EndInit();
            layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).EndInit();
            ((System.ComponentModel.ISupportInitialize)lookUpEdit1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControlTop;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton btnSyncRole;
        private DevExpress.XtraEditors.SimpleButton btnSyncCustomer;
        private DevExpress.XtraEditors.SimpleButton btnSyncUsers;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton btnSyncProduct;
        private DevExpress.XtraEditors.SimpleButton btnSynBranch;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
    }
}