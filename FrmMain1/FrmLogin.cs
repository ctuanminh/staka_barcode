using System;
using System.Configuration;
using System.Windows.Forms;
using Be.Common.Dtos.Identity;
using Be.Services.Identity;
using FrmMain.App;
using FrmMain.Utils;

namespace FrmMain
{
    public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
    {
        private readonly IUserService _userService;
        public FrmLogin(IUserService userService)
        {
            _userService = userService;
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageHelper.MsgBox("Tên đăng nhập/Mật khẩu không được trống", MsgType.Error_);
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
            DialogResult = DialogResult.OK;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            txtUserName.Text = ConfigurationManager.AppSettings["LastUserName"] ?? "";
            txtPassword.Text = ConfigurationManager.AppSettings["LastPassword"] ?? "";
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            btnLogin_Click(btnLogin, EventArgs.Empty);
        }
    }
}