using System;
using Be.Common.Dtos.Identity;
using Be.Services.Identity;
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
            if (loginEntity != null)
            {
                MessageHelper.MsgBox("Đăng nhập thành công", MsgType.Information);
            }
        }
    }
}