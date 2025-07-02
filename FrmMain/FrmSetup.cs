using System;
using System.Threading.Tasks;
using Be.Services.Pos;
using Be.Services.System;
using FrmMain.Utils;

namespace FrmMain
{
    public partial class FrmSetup : FrmBasePos
    {
        public FrmSetup(IBranchService branchService, ISystemService systemService) : base(branchService, systemService)
        {
            InitializeComponent();
        }

        private async void FrmSetup_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadDefaultSetting();
                if (BranchId != 0) return;
                MessageHelper.MsgBox(this,"Không tìm thấy thông tin chi nhánh trên máy này.", MsgType.Error);
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox(this,"Không tìm thấy thông tin chi nhánh trên máy này.", MsgType.Error);
            }
        }
    }
}