using Be.Common.System;
using Be.Services.Pos;
using Be.Services.System;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using FrmMain.App;
using FrmMain.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrmMain
{
    public partial class FrmMainF : RibbonForm
    {
        private Timer _clockTimer;
        public IServiceProvider ServiceProvider { get; }
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
        private async void FrmMainF_Load(object sender, EventArgs e)
        {
            try
            {
                _clockTimer = new Timer();
                _clockTimer.Interval = 1000; // 1 giây
                _clockTimer.Tick += ClockTimer_Tick;
                _clockTimer.Start();
                if (AppGlobals.UserInfo == null || string.IsNullOrEmpty(AppGlobals.UserInfo.UserName))
                {
                    AppGlobals.UserInfo = new UserInfo();
                    var frmLogin = ServiceProvider.GetRequiredService<FrmLogin>();
                    if (frmLogin.ShowDialog() != DialogResult.OK)
                    {
                        Close();
                        return;
                    }
                }

                await LoadSetting();

                var isOrderFormOpened = MdiChildren.Any(f => f is FrmOrder);
                if (isOrderFormOpened) return;
                var scope = ServiceProvider.CreateScope();
                var frmOrder = scope.ServiceProvider.GetRequiredService<FrmOrder>();
                frmOrder.MdiParent = this;
                frmOrder.Tag = scope;
                frmOrder.FormClosed += (_, _) => scope.Dispose();
                frmOrder.Show();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá tải dữ liệu: {ex}", MsgType.Error_);
            }
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            lblTimer.Caption = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }

        private async Task LoadSetting()
        {
            try
            {
                var branches = await _branchService.GetAllBranches();
                rpLkpBranch.DataSource = branches;
                rpLkpBranch.ReadOnly = true; 

                var setting = await _systemService.GetAppSettingBuyComputer(Environment.MachineName);
                AppGlobals.AppSetting = setting;
                var branchSetting = AppGlobals.AppSetting.FirstOrDefault(s =>
                    s.ComputerName == Environment.MachineName && 
                    s.ModuleName == "Branch" && 
                    s.SettingKey == "BranchId" &&
                    !string.IsNullOrWhiteSpace(s.SettingValue));
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes<AssemblyMetadataAttribute>();
                
                var buildDate = attributes
                    .FirstOrDefault(attr => attr.Key == "BuildDate")?.Value ?? "Unknown";

                bLblComputerName.Caption = $"Máy: {Environment.MachineName}";
                var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                bLblVersion.Caption = $"Ver: {version} - Build: 27/06/2025";
                if (branchSetting != null && int.TryParse(branchSetting.SettingValue, out var branchId))
                {
                    AppGlobals.BranchId = branchId;
                    barBranch.EditValue = branchSetting.SettingValue;
                }
                else
                {
                    MessageHelper.MsgBox("Kiểm tra lại dữ liệu mặc định Chi nhánh/Tài khoản", MsgType.Error_);
                }
            }
            catch (Exception e)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình tải dữ liệu", MsgType.Error_);
            }
        }

        private async void mButtonItem_ItemClick(object sender, ItemClickEventArgs e)
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
                    if (!FormHelper.OpenedForm(nameof(FrmOrder), WuserControl.Order, out _))
                    {
                        await FormHelper.OpenFormWithScope<FrmOrder>(
                            this, ServiceProvider,
                            "", 0,
                            "OpenFrmOrder", WuserControl.FrmPurchase);
                    }
                    break;
                case nameof(mbtnSystem):
                    if (!FormHelper.OpenedForm(nameof(FrmSystem), WuserControl.FrmSystem, out _))
                    {
                        var frmAdmin = new FrmAdmin();
                        if (frmAdmin.ShowDialog() == DialogResult.OK)
                        {
                            var frmSystem = ServiceProvider.GetRequiredService<FrmSystem>();
                            FormHelper.NewFormNew(this, frmSystem, WuserControl.FrmSystem);
                        }
                    }
                    break;
                case nameof(mbtcPurchase):
                    if (!FormHelper.OpenedForm(nameof(FrmPurchase), WuserControl.FrmPurchase, out _))
                    {
                        var frmSystem = ServiceProvider.GetRequiredService<FrmPurchase>();
                        FormHelper.NewFormNew(this,frmSystem, WuserControl.FrmPurchase);
                    }
                    break;
                case nameof(mbtnTranfer):
                    if (!FormHelper.OpenedForm(nameof(FrmTransfer), WuserControl.FrmTransfer, out _))
                    {
                        var frmSystem = ServiceProvider.GetRequiredService<FrmTransfer>();
                        FormHelper.NewFormNew(this, frmSystem, WuserControl.FrmTransfer);
                    }
                    break;
                case nameof(mbtnReceiver):
                    if (!FormHelper.OpenedForm(nameof(FrmReceiverList), WuserControl.FrmReceiverList, out _))
                    {
                        var frmReceiverList = ServiceProvider.GetRequiredService<FrmReceiverList>();
                        FormHelper.NewFormNew(this,frmReceiverList, WuserControl.FrmReceiverList);
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
                        if (frmLogin.ShowDialog() != DialogResult.OK)
                        {
                            Close();
                            return;
                        }
                        var isOrderFormOpened = this.MdiChildren.Any(f => f is FrmOrder);
                        if (isOrderFormOpened) return;
                        var frmOrder = ServiceProvider.GetRequiredService<FrmOrder>();
                        frmOrder.MdiParent = this;
                        frmOrder.Show();
                    }
                    break;
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