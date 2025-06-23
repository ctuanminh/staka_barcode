using System;
using Be.Services.Pos;
using Be.Services.System;
using FrmMain.Utils;

namespace FrmMain
{
    public partial class FrmSetup : FrmBase
    {
        private readonly ISystemService _systemService;
        public FrmSetup(IBranchService branchService, ISystemService systemService) : base(branchService)
        {
            _systemService = systemService;
            InitializeComponent();
        }

        private async void FrmSetup_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadDefaultSetting();
                if (_branchId != 0) return;
                MessageHelper.MsgBox("Không tìm thấy thông tin chi nhánh trên máy này.", MsgType.Error_);
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox("Không tìm thấy thông tin chi nhánh trên máy này.", MsgType.Error_);
            }
        }

    }
}