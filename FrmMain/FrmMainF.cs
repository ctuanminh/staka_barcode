using Be.Common.System;
using Be.Services.Pos;
using Be.Services.System;
using DevExpress.Map.Kml;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using FrmMain.App;
using FrmMain.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.IO;
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
                var frmOrder = ServiceProvider.GetRequiredService<FrmOrder>();
                frmOrder.MdiParent = this;
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
                bLblVersion.Caption = $"Ver: {version} - Build: 21/06/2025";
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
            FrmTransfer = 5,
            FrmTransferProcess = 6,
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
                    if (!OpenedForm(nameof(FrmTransfer), WuserControl.FrmTransfer))
                    {
                        var frmSystem = ServiceProvider.GetRequiredService<FrmTransfer>();
                        NewFormNew(frmSystem, WuserControl.FrmTransfer);
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

        public void NewFormNew(Form f, WuserControl wuser, string fName = "")
        {
            if (!string.IsNullOrEmpty(fName)) f.Name = fName;
            f.AccessibleDescription = wuser.ToString();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.BringToFront();
            f.Show();
        }

        private void CustomizeTabControl()
        {
            TabMdiManager.Appearance.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            TabMdiManager.Appearance.Options.UseFont = true;
            TabMdiManager.AppearancePage.Header.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            TabMdiManager.AppearancePage.Header.Options.UseFont = true;
        }

        public static DateTime RetrieveLinkerTimestamp()
        {
            var filePath = Assembly.GetCallingAssembly().Location;
            const int cPeHeaderOffset = 60;
            const int cLinkerTimestampOffset = 8;
            var b = new byte[2048];
            using (FileStream s = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                s.Read(b, 0, 2048);
            }
            var i = BitConverter.ToInt32(b, cPeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(b, i + cLinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);
            return linkTimeUtc.ToLocalTime();
        }
    }
}