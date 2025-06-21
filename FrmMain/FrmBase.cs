using System;
using System.Linq;
using System.Threading.Tasks;
using Be.Services.Pos;
using DevExpress.XtraEditors;
using FrmMain.App;
using FrmMain.Utils;

namespace FrmMain
{
    public partial class FrmBase : XtraForm
    {
        protected int _branchId;
        protected readonly IBranchService _branchService;
        public FrmBase(IBranchService branchService)
        {
            _branchService = branchService;
            InitializeComponent();
        }
        protected async Task LoadDefaultSetting()
        {
            var setting = AppGlobals.AppSetting.FirstOrDefault(s =>
                s.ComputerName == Environment.MachineName &&
                s.ModuleName == "Branch" &&
                s.SettingKey == "BranchId");

            if (setting == null || string.IsNullOrWhiteSpace(setting.SettingValue))
            {
                MessageHelper.MsgBox("Không tìm thấy thông tin chi nhánh trên máy này.", MsgType.Error_);
                return;
            }

            if (!long.TryParse(setting.SettingValue, out var branchId))
            {
                MessageHelper.MsgBox("Mã chi nhánh không hợp lệ.", MsgType.Error_);
                return;
            }

            var branch = await _branchService.GetBranchById(branchId);
            _branchId = branch?.BranchId ?? 0;
        }
    }
}