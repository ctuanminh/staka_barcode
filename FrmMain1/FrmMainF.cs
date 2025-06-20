using Be.Services.System;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;
using FrmMain.App;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Common.System;
using Be.Services.Pos;
using DevExpress.Mvvm.POCO;
using DevExpress.XtraEditors.Controls;
using FrmMain.Utils;

namespace FrmMain
{
    public partial class FrmMainF : RibbonForm
    {
        private Timer _clockTimer;
        public IServiceProvider ServiceProvider { get; }
        public bool login = false;
        private readonly ISystemService _systemService;
        private readonly IBranchService _branchService;
        public FrmMainF(IServiceProvider serviceProvider, ISystemService systemService, IBranchService branchService)
        {
            ServiceProvider = serviceProvider;
            _systemService = systemService;
            _branchService = branchService;
            InitializeComponent();
            CustomizeTabControl();
        }

        private static bool OpenedForm(string fName, WuserControl parent)
        {
            var openForm = Application.OpenForms[fName];
            if (openForm == null)
            {
                return false;
            }

            if (parent == WuserControl.None || openForm.AccessibleDescription == parent.ToString())
            {
                openForm.BringToFront();
                return true;
            }

            openForm.Close();
            return false;
        }
        public enum WuserControl
        {
            None = 0,
            Order = 1,
            OrderProcess = 2,
            FrmSystem = 3,
            FrmPurchase = 4,
            FrmPurchaseProcess = 5,
            FrmTranfer = 5,
            FrmTranferProcess = 6,
            FrmReceiverList = 7,
        }

        private void mButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AppGlobals.UserInfo.FullName == null)
            {
                var frmLogin = ServiceProvider.GetRequiredService<FrmLogin>();
                frmLogin.ShowDialog();
                if (AppGlobals.UserInfo.UserName == null) return;
            }

            switch (e.Item.Name)
            {
                case nameof(mbtnOrder):
                    if (!OpenedForm(nameof(FrmOrder), WuserControl.Order))
                    {
                        var frmOrder = ServiceProvider.GetRequiredService<FrmOrder>();
                        NewFormNew(frmOrder, WuserControl.Order);
                    }
                    break;
                case nameof(mbtnSystem):
                    if (!OpenedForm(nameof(FrmSystem), WuserControl.FrmSystem))
                    {
                        var frmAdmin = new FrmAdmin();
                        if (frmAdmin.ShowDialog() == DialogResult.OK)
                        {
                            var frmSystem = ServiceProvider.GetRequiredService<FrmSystem>();
                            NewFormNew(frmSystem, WuserControl.FrmSystem);
                        }
                    }
                    break;
                case nameof(mbtcPurchase):
                    if (!OpenedForm(nameof(FrmPurchase), WuserControl.FrmPurchase))
                    {
                        var frmSystem = ServiceProvider.GetRequiredService<FrmPurchase>();
                        NewFormNew(frmSystem, WuserControl.FrmPurchase);
                    }
                    break;
                case nameof(mbtnTranfer):
                    if (!OpenedForm(nameof(FrmTransfer), WuserControl.FrmTranfer))
                    {
                        var frmSystem = ServiceProvider.GetRequiredService<FrmTransfer>();
                        NewFormNew(frmSystem, WuserControl.FrmTranfer);
                    }
                    break;
                case nameof(mbtnReceiver):
                    if (!OpenedForm(nameof(FrmReceiverList), WuserControl.FrmReceiverList))
                    {
                        var frmReceiverList = ServiceProvider.GetRequiredService<FrmReceiverList>();
                        NewFormNew(frmReceiverList, WuserControl.FrmReceiverList);
                    }
                    break;
                case nameof(mbtnLogout):
                    if (MessageHelper.MsgBox("Bạn muốn thoát tài khoản?", MsgType.YesNo) == DialogResult.Yes)
                    {
                        AppGlobals.UserInfo = new UserInfo();
                        
                        foreach (var form in MdiChildren)
                        {
                            form.Close();
                        }
                        var frmLogin = ServiceProvider.GetRequiredService<FrmLogin>();
                        frmLogin.ShowDialog();
                        var isOrderFormOpened = this.MdiChildren.Any(f => f is FrmOrder);
                        if (isOrderFormOpened) return;
                        var frmOrder = ServiceProvider.GetRequiredService<FrmOrder>();
                        frmOrder.MdiParent = this;
                        frmOrder.Show();
                    }
                    break;
            }
        }

        public void NewFormNew(Form f, WuserControl wuser, string fName = "")
        {
            if (!string.IsNullOrEmpty(fName)) f.Name = fName;
            f.AccessibleDescription = wuser.ToString();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.BringToFront();
            f.Show();
        }

        private async void FrmMainF_Load(object sender, EventArgs e)
        {
            _clockTimer = new Timer();
            _clockTimer.Interval = 1000; // 1 giây
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();
            LoadSetting();
            if (AppGlobals.UserInfo != null) return;
            AppGlobals.UserInfo = new UserInfo();
            var frmLogin = ServiceProvider.GetRequiredService<FrmLogin>();
            frmLogin.ShowDialog();
            var isOrderFormOpened = this.MdiChildren.Any(f => f is FrmOrder);
            if (isOrderFormOpened) return;
            var frmOrder = ServiceProvider.GetRequiredService<FrmOrder>();
            frmOrder.MdiParent = this;
            frmOrder.Show();
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            lblTimer.Caption = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }

        private async void LoadSetting()
        {
            try
            {
                var branches = await _branchService.GetAllBranches();
                rpLkpBranch.DataSource = branches;
                rpLkpBranch.ReadOnly = true; // Set ReadOnly to true to prevent editing

                var setting = await _systemService.GetAppSettingBuyComputer(Environment.MachineName);
                AppGlobals.AppSetting = setting;
                var branchId = AppGlobals.AppSetting.FirstOrDefault(s =>
                    s.ComputerName == Environment.MachineName && s.ModuleName == "Branch" && s.SettingKey == "BranchId");

                if (branchId != null)
                {
                    AppGlobals.BranchId = Convert.ToInt32(branchId.SettingValue);
                    barBranch.EditValue = branchId?.SettingValue; // Corrected assignment
                }
                else
                {
                    MessageHelper.MsgBox("Kiểm tra lại dữ liệu mặc đinh Chi nhánh/Tài khoản", MsgType.Error_);
                }
            }
            catch (Exception e)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình tải dữ liệu", MsgType.Error_);
                return;
            }
        }

        private void CustomizeTabControl()
        {
            TabMdiManager.Appearance.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            TabMdiManager.Appearance.Options.UseFont = true;
            TabMdiManager.AppearancePage.Header.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            TabMdiManager.AppearancePage.Header.Options.UseFont = true;
        }
    }
}