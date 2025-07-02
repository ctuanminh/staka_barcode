using Be.Common.Dtos.Identity;
using Be.Common.System;
using Be.Services.Identity;
using Be.Services.Pos;
using Be.Services.System;
using DevExpress.XtraEditors;
using FrmMain.App;
using FrmMain.Utils;
using System;
using System.Configuration;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrmMain
{
    public partial class FrmLogin : FrmBasePos
    {
        private readonly IUserService _userService;
        private readonly IBranchService _branchService;
        private readonly ISystemService _systemService;
        private AppSettingDto _appSetting;

        public FrmLogin(IUserService userService, IBranchService branchService, ISystemService systemService) : base(
            branchService, systemService)
        {
            _userService = userService;
            _branchService = branchService;
            _systemService = systemService;
            InitializeComponent();
        }

        private async void FrmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                txtUserName.Text = ConfigurationManager.AppSettings["LastUserName"] ?? "";
                txtPassword.Text = ConfigurationManager.AppSettings["LastPassword"] ?? "";
                SetTextEditHeight(this, 25);
                var branches = await _branchService.GetAllBranches();
                lkpBranchInit.Properties.DataSource = branches;
                _appSetting = await
                    _systemService.GetAppSetting(Environment.MachineName, "Branch", "BranchId");
                if (_appSetting == null) return;
                lkpBranchInit.EditValue = _appSetting.SettingValue;
                lkpBranchInit.ReadOnly = true;
                lkpBranchInit.BackColor = Color.White;
                lkpBranchInit.ForeColor = Color.OrangeRed;
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình lưu cài đặt: {ex}", MsgType.Error);
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageHelper.MsgBox(this,"Tên đăng nhập/Mật khẩu không được trống", MsgType.Error);
                    return;
                }

                if (lkpBranchInit.EditValue == null)
                {
                    MessageHelper.MsgBox(this,"Chọn Chi nhánh làm việc", MsgType.Error);
                    return;
                }

                var userLogin = new UserLoginRequest()
                {
                    UserName = txtUserName.Text,
                    Password = txtPassword.Text
                };
                var loginEntity = await _userService.Login(userLogin);
                if (!loginEntity.success) return;
                AppGlobals.UserInfo.FullName = loginEntity.userLoginDto.FullName;
                AppGlobals.UserInfo.UserName = loginEntity.userLoginDto.UserName;

                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LastUserName"].Value = txtUserName.Text;
                config.AppSettings.Settings["LastPassword"].Value = txtPassword.Text;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                if (_appSetting == null)
                {
                    await SaveSetting();
                }
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình lưu cài đặt: {ex}", MsgType.Error);
            }
        }

        private async Task SaveSetting()
        {
            try
            {
                if (_appSetting == null)
                {
                    var appSetting = new AppSettingDto()
                    {
                        ComputerName = Environment.MachineName,
                        ModuleName = "Branch",
                        SettingKey = "BranchId",
                        SettingValue = lkpBranchInit.EditValue.ToString()
                    };
                    var result = await _systemService.AddAppSetting(appSetting);
                    if (result == null)
                    {
                        MessageHelper.MsgBox(this,"Lưu cài đặt thất bại", MsgType.Error);
                    }
                }
                else
                {
                    var appSettingExist = await
                        _systemService.GetAppSetting(Environment.MachineName, "Branch", "BranchId");
                    if (appSettingExist != null)
                    {
                        appSettingExist.SettingValue = lkpBranchInit.EditValue.ToString();
                        var result = await _systemService.UpdateAppSetting(appSettingExist);
                        if (!result)
                        {
                            MessageHelper.MsgBox(this,"Cập nhật cài đặt thất bại", MsgType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình lưu cài đặt: {ex}", MsgType.Error);
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            btnLogin_Click(btnLogin, EventArgs.Empty);
        }

        private static void SetTextEditHeight(Control control, int height)
        {
            foreach (Control c in control.Controls)
            {
                switch (c)
                {
                    case TextEdit textEdit:
                        textEdit.Properties.AutoHeight = false;
                        textEdit.MinimumSize = new Size(0, height);
                        textEdit.MaximumSize = new Size(0, height);
                        break;
                    case SimpleButton button:
                        button.MinimumSize = new Size(0, height);
                        button.MaximumSize = new Size(0, height);
                        break;
                    case CheckEdit checkEdit:
                        checkEdit.MinimumSize = new Size(0, height);
                        checkEdit.MaximumSize = new Size(0, height);
                        break;
                    default:
                    {
                        if (c.HasChildren)
                        {
                            SetTextEditHeight(c, height); // Đệ quy
                        }
                        break;
                    }
                }
            }
        }
    }
}