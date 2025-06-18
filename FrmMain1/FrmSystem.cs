using Be.Common.Branch.Request;
using Be.Common.Dtos.Identity;
using Be.Services.customer;
using Be.Services.Identity;
using Be.Services.Pos;
using DevExpress.XtraSplashScreen;
using FrmMain.Utils;
using System.Drawing;
using System;
using System.Windows.Forms;
using Be.Common.Dtos.Product;
using Be.Common.System;
using Be.Core.Entities;
using Be.Services.Catalog;
using Be.Services.System;
using DevExpress.XtraEditors;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmSystem : XtraForm
    {
        private readonly IBranchService _branchService;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly ISystemService _systemService;
        private string _branchId;

        public FrmSystem(ICustomerService customerService, IBranchService branchService, IUserService userService,
            IProductService productService, ISystemService systemService)
        {
            _customerService = customerService;
            _branchService = branchService;
            _userService = userService;
            _productService = productService;
            _systemService = systemService;
            InitializeComponent();
        }

        private async void btnSyncUsers_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(LoadingForm), true, true);
                SplashScreenManager.Default.SetWaitFormCaption("Đang đồng bộ Người dùng");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng đợi...");
                var request = new SyncUserRequest()
                {
                    PageSize = 200,
                    CurrentItem = 0,
                };
                var syncUserExist = await _userService.SyncUser(request);
                if (syncUserExist)
                {
                    MessageHelper.MsgBox($"Đồng bộ người dùng thành công", MsgType.Information);
                }
                else
                {
                    MessageHelper.MsgBox("Không có người dùng nào để đồng bộ", MsgType.Error_);
                }
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình đồng bộ dữ liệu: {exception}", MsgType.Error_);
            }
            finally
            {
                SplashScreenManager.CloseForm();
            }
        }

        private async void btnSyncCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(LoadingForm), true, true);
                SplashScreenManager.Default.SetWaitFormCaption("Đang đồng Khách hàng");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng đợi...");
                var request = new SyncUserRequest()
                {
                    PageSize = 200,
                    CurrentItem = 0,
                };
                var success = await _customerService.SyncCustomer();
                if (success)
                {
                    MessageHelper.MsgBox($"Đồng bộ Khách hàng thành công", MsgType.Information);
                }
                else
                {
                    MessageHelper.MsgBox("Không có Khách hàng nào để đồng bộ", MsgType.Error_);
                }
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình đồng bộ dữ liệu: {exception}", MsgType.Error_);
            }
            finally
            {
                SplashScreenManager.CloseForm();
            }
        }

        private async void btnSyncRole_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(LoadingForm), true, true);
                SplashScreenManager.Default.SetWaitFormCaption("Đang lấy Đơn hàng");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng đợi...");
                var request = new SyncRoleRequest()
                {

                };
                var result = await _userService.SyncRole(request);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình đồng bộ dữ liệu: {ex}", MsgType.Error_);
            }
            finally
            {
                SplashScreenManager.CloseForm();
            }
        }

        private async void btnSynBranch_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(LoadingForm), true, true);
                SplashScreenManager.Default.SetWaitFormCaption("Đang lấy Đơn hàng");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng đợi...");
                var branchRequest = new BranchRequest();
                var result = await _branchService.SyncBranch(branchRequest);
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình đồng bộ dữ liệu: {exception}", MsgType.Error_);
            }
            finally
            {
                SplashScreenManager.CloseForm();
            }
        }

        private async void btnSyncProduct_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(LoadingForm), true, true);
                SplashScreenManager.Default.SetWaitFormCaption("Đang lấy Đơn hàng");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng đợi...");
                var request = new SearchProductRequestKiot()
                {
                    IsActive = true,
                };
                var result = await _productService.SyncProduct(request);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình đồng bộ dữ liệu: {ex}", MsgType.Error_);
            }
            finally
            {
                SplashScreenManager.CloseForm();
            }
        }

        private async void FrmSystem_Load(object sender, EventArgs e)
        {
            var computerName = Environment.MachineName;
            MessageHelper.MsgBox(computerName, MsgType.Information);
            SetTextEditHeight(this, 25);
            var branches = await _branchService.GetAllBranches();
            lkpBranch.Properties.DataSource = branches;
        }

        private void SetTextEditHeight(Control control, int height)
        {
            foreach (Control c in control.Controls)
            {
                switch (c)
                {
                    case TextEdit textEdit:
                        textEdit.Properties.AutoHeight = false;
                        textEdit.MinimumSize = new System.Drawing.Size(0, height);
                        textEdit.MaximumSize = new System.Drawing.Size(0, height);
                        break;
                    case SimpleButton button:
                        button.MinimumSize = new System.Drawing.Size(0, height);
                        button.MaximumSize = new System.Drawing.Size(0, height);
                        break;
                    case CheckEdit checkEdit:
                        checkEdit.MinimumSize = new System.Drawing.Size(0, height);
                        checkEdit.MaximumSize = new System.Drawing.Size(0, height);
                        break;
                    default:
                        {
                            if (c.HasChildren)
                            {
                                SetTextEditHeight(c, height); // Recursive call
                            }

                            break;
                        }
                }
            }
        }

        private async void DefautlSetting()
        {
            try
            {
                var appSetting = new AppSettingDto()
                {
                    ComputerName = Environment.MachineName,
                    ModuleName = "Branch",
                    SettingKey = "BranchId",
                    SettingValue = "631782",
                };
                var result = await _systemService.AddAppSetting(appSetting);
                if (result != null)
                {
                    MessageHelper.MsgBox("Lưu cài đặt thành công", MsgType.Information);
                }
                else
                {
                    MessageHelper.MsgBox("Lưu cài đặt thất bại", MsgType.Error_);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lưu cài đặt: {ex}", MsgType.Error_);
            }
        }

        private void lkpBranch_EditValueChanged(object sender, EventArgs e)
        {
            _branchId = lkpBranch.EditValue?.ToString();
        }

        private async void saveSetting_Click(object sender, EventArgs e)
        {
            try
            {
                var appSettingExist = await 
                    _systemService.GetAppSetting(Environment.MachineName, "Branch", "BranchId");
                if (appSettingExist != null)
                {
                    appSettingExist.SettingValue = _branchId;
                    var result = await _systemService.UpdateAppSetting(appSettingExist);
                    if (result)
                    {
                        MessageHelper.MsgBox("Cập nhật cài đặt thành công", MsgType.Information);
                    }
                    else
                    {
                        MessageHelper.MsgBox("Cập nhật cài đặt thất bại", MsgType.Error_);
                    }
                }
                else
                {
                    var appSetting = new AppSettingDto()
                    {
                        ComputerName = Environment.MachineName,
                        ModuleName = "Branch",
                        SettingKey = "BranchId",
                        SettingValue = _branchId
                    };
                    var result = await _systemService.AddAppSetting(appSetting);
                    if (result != null)
                    {
                        MessageHelper.MsgBox("Lưu cài đặt thành công", MsgType.Information);
                    }
                    else
                    {
                        MessageHelper.MsgBox("Lưu cài đặt thất bại", MsgType.Error_);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lưu cài đặt: {ex}", MsgType.Error_);
            }
        }
    }

}