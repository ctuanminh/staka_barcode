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

                await FormHelper.OpenFormWithScope<FrmOrder>(this, ServiceProvider, "", 0, "OpenFrmOrder",
                    WuserControl.Order);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá tải dữ liệu: {ex}", MsgType.Error);
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
                var settings = await _systemService.GetAppSettingBuyComputer(Environment.MachineName);
                AppGlobals.AppSetting = settings;

                var branchSetting = AppGlobals.AppSetting.FirstOrDefault(s =>
                    s.ComputerName == Environment.MachineName &&
                    s.ModuleName == "Branch" &&
                    s.SettingKey == "BranchId" &&
                    !string.IsNullOrWhiteSpace(s.SettingValue));
                rpLkpBranch.DataSource = branches;
                rpLkpBranch.ReadOnly = true;
                if (branchSetting != null && int.TryParse(branchSetting.SettingValue, out var branchId))
                {
                    AppGlobals.BranchId = branchId;
                    barBranch.EditValue = branchSetting.SettingValue;
                }
                else
                {
                    MessageHelper.MsgBox(this, "Kiểm tra lại dữ liệu mặc định Chi nhánh/Tài khoản", MsgType.Error);
                }

                bLblComputerName.Caption = $"Máy: {Environment.MachineName}";
                var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                bLblVersion.Caption = $"Ver: {version} - Build: 26/07/2025";
            }
            catch (Exception e)
            {
                MessageHelper.MsgBox(this,"Có lỗi trong quá trình tải dữ liệu", MsgType.Error);
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
                    await FormHelper.OpenFormWithScope<FrmOrder>(
                        this, ServiceProvider,
                        "", 0,
                        "OpenFrmOrder", WuserControl.FrmPurchase);
                    break;
                case nameof(mbtnSystem):
                    if (!FormHelper.OpenedForm(nameof(FrmSystem), WuserControl.FrmSystem, out _))
                    {
                        var frmAdmin = new FrmAdmin();
                        if (frmAdmin.ShowDialog() == DialogResult.OK)
                        {
                            await FormHelper.OpenFormWithScope<FrmSystem>(
                                this, ServiceProvider,
                                "", 0,
                                "OpenFrmSystem", WuserControl.FrmPurchase);
                        }
                    }
                    break;

                case nameof(mbtcPurchase):
                    await FormHelper.OpenFormWithScope<FrmPurchase>(
                        this, ServiceProvider,
                        "", 0,
                        "OpenFrmPurchase", WuserControl.FrmPurchase);
                    break;
                case nameof(mbtnTranfer):
                    await FormHelper.OpenFormWithScope<FrmTransfer>(
                        this, ServiceProvider,
                        "", 0,
                        "OpenFrmTransfer", WuserControl.FrmTransfer);
                    break;
                case nameof(mbtnReceiver):
                    await FormHelper.OpenFormWithScope<FrmReceiverList>(
                        this, ServiceProvider,
                        "", 0,
                        "OpenFrmReceiverList", WuserControl.FrmReceiverList);
                    break;
                case nameof(mbtnProduct):
                    await FormHelper.OpenFormWithScope<FrmProduct>(
                        this, ServiceProvider, "", 0, "OpenFrmProduct", WuserControl.FrmProduct);
                    break;

                case nameof(mbtnInvoiceList):
                    await FormHelper.OpenFormWithScope<FrmInvoiceList>(
                        this, ServiceProvider, "", 0, "OpenFrmInvoiceList", WuserControl.FrmReceiverList
                        );
                    break;

                case nameof(mbtnLogout):
                    if (MessageHelper.MsgBox(this, "Bạn muốn thoát tài khoản?", MsgType.YesNo) == DialogResult.Yes)
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
                        await FormHelper.OpenFormWithScope<FrmOrder>(this, ServiceProvider, "", 0, "OpenFrmOrder",
                            WuserControl.Order);
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