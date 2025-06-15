using Be.Common.Branch.Request;
using Be.Common.Dtos.Identity;
using Be.Services.customer;
using Be.Services.Identity;
using Be.Services.Pos;
using DevExpress.XtraScheduler.Outlook.Interop;
using DevExpress.XtraSplashScreen;
using FrmMain.Utils;
using System;
using Be.Common.Dtos.Product;
using Be.Services.Catalog;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmSystem : DevExpress.XtraEditors.XtraForm
    {
        private readonly IBranchService _branchService;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        
        public FrmSystem(ICustomerService customerService, IBranchService branchService, IUserService userService, IProductService productService)
        {
            _customerService = customerService;
            _branchService = branchService;
            _userService = userService;
            _productService = productService;
            InitializeComponent();
        }

        private void btnSyncUsers_Click(object sender, EventArgs e)
        {

        }

        private void btnSyncCustomer_Click(object sender, EventArgs e)
        {
            
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
                    IsActive= true,
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
    }
}