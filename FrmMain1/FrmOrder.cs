using DevExpress.Mvvm.POCO;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraScheduler.Outlook.Interop;
using FrmMain.Dto.Request;
using FrmMain.Dto.Response;
using FrmMain.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FrmMain.FrmMainF;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmOrder : XtraForm
    {
        private readonly FrmMainF _mainForm;
        private readonly IKiotVietService _kiotVietService;
        private const string OrderUrl = " https://public.kiotapi.com/orders/code/";

        public FrmOrder(FrmMainF mainForm, IKiotVietService kiotVietService)
        {
            _mainForm = mainForm;
            _kiotVietService = kiotVietService;
            InitializeComponent();
        }

        private async void FrmOrder_Shown(object sender, EventArgs e)
        {
            try
            {
                const string orderUrl = $"https://public.kiotapi.com/orders";
                var request = new SearchOrderRequest()
                {
                    BranchIds = [631782, 635192],
                    Status = [1],
                    PageSize = 200,
                    OrderBy = "purchaseDate",
                    OrderDirection = "Desc"
                };

                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, request, "GET");

                if (!success || content == null) return;
                var orderPagedResponse = JsonConvert.DeserializeObject<OrderPagedResponse>(content);

                grdControlOrders.DataSource = orderPagedResponse.Data;
                grdViewOrders.Columns["PurchaseDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                grdViewOrders.Columns["PurchaseDate"].DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
            }
            catch (Exception exception)
            {
                MessageBox.Show("Lỗi gọi API: " + exception);
            }
        }

        private void grdViewOrders_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (sender is not GridView { FocusedRowHandle: >= 0 } view) return;
                var code = view.GetRowCellValue(view.FocusedRowHandle, "Code");
                if (code == null) return;
                {
                    if (FormHelper.OpenedForm(nameof(FrmOrderProcess), WuserControl.order, out var openForm))
                    {
                        if (openForm is FrmOrderProcess processForm)
                        {
                            processForm.ReloadData(code.ToString());
                        }
                    }
                    else
                    {
                        FrmOrderProcess.CurrentCode = code.ToString();
                        var frmOrderInstance = _mainForm.ServiceProvider.GetRequiredService<FrmOrderProcess>();
                        Form frmOrder = frmOrderInstance;
                        FormHelper.NewFormNew(_mainForm, frmOrder, WuserControl.order, nameof(FrmOrderProcess));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chuyển dữ liệu: " + ex);
            }
        }
    }
}