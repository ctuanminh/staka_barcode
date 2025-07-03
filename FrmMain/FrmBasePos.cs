using Be.Services.Pos;
using Be.Services.System;
using DevExpress.XtraEditors;
using FrmMain.App;
using FrmMain.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FrmMain
{
    public partial class FrmBasePos : XtraForm
    {
        public FrmBasePos()
        {
            InitializeComponent();
        }
        protected int BranchId;
        protected string BranchName;
        protected readonly IBranchService BranchService;
        protected readonly ISystemService SystemService;
        protected FrmBasePos(IBranchService branchService, ISystemService systemService)
        {
            BranchService = branchService;
            SystemService = systemService;
            InitializeComponent();
        }

        protected async Task LoadDefaultSetting()
        {

            var settings = await SystemService.GetAppSettingBuyComputer(Environment.MachineName);
            AppGlobals.AppSetting = settings;

            var branchSetting = AppGlobals.AppSetting.FirstOrDefault(s =>
                s.ComputerName == Environment.MachineName &&
                s.ModuleName == "Branch" &&
                s.SettingKey == "BranchId" &&
                !string.IsNullOrWhiteSpace(s.SettingValue));

            if (branchSetting == null || string.IsNullOrWhiteSpace(branchSetting.SettingValue))
            {
                MessageHelper.MsgBox(this, "Không tìm thấy thông tin chi nhánh trên máy này.", MsgType.Error);
                return;
            }

            if (!long.TryParse(branchSetting.SettingValue, out var branchId))
            {
                MessageHelper.MsgBox(this, "Mã chi nhánh không hợp lệ.", MsgType.Error);
                return;
            }

            var branch = await BranchService.GetBranchById(branchId);
            BranchId = branch?.BranchId ?? 0;
            BranchName = branch?.BranchName ?? "";
        }
    }
}