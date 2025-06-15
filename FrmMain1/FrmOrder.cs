using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using FrmMain.Dto.Request;
using FrmMain.Dto.Response;
using FrmMain.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Be.Services.KiotViet;
using Be.Services.Pos;
using static FrmMain.FrmMainF;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmOrder : XtraForm
    {
        private readonly FrmMainF _mainForm;
        private readonly IKiotVietService _kiotVietService;
        private const string OrderUrl = " https://public.kiotapi.com/orders/code/";
        private List<int> _orderStatusList;
        private readonly IBranchService _branchService;
        public FrmOrder(FrmMainF mainForm, IKiotVietService kiotVietService, IBranchService branchService)
        {
            _mainForm = mainForm;
            _kiotVietService = kiotVietService;
            _branchService = branchService;
            InitializeComponent();
        }

        private async void FrmOrder_Shown(object sender, EventArgs e)
        {
            _orderStatusList = [1];
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(LoadingForm), true, true);
                SplashScreenManager.Default.SetWaitFormCaption("Đang lấy Đơn hàng");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng đợi...");
                layoutControlTop.Enabled = false;
                grdControlOrders.Enabled = false;
                const string orderUrl = $"https://public.kiotapi.com/orders";
                var request = new SearchOrderRequest()
                {
                    BranchIds = [631782, 635192],
                    Status = _orderStatusList.ToArray(),
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
                MessageHelper.MsgBox("Lỗi gọi API: " + exception, MsgType.Error_);
            }
            finally
            {
                // Ẩn màn hình chờ
                SplashScreenManager.CloseForm();
                layoutControlTop.Enabled = true;
                grdControlOrders.Enabled = true;
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
                    if (FormHelper.OpenedForm(nameof(FrmOrderProcess), WuserControl.Order, out var openForm))
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
                        FormHelper.NewFormNew(_mainForm, frmOrder, WuserControl.Order, nameof(FrmOrderProcess));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Lỗi khi chuyển dữ liệu", MsgType.Error_);
            }
        }

        private static void SetTextEditHeight(Control control, int height)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextEdit textEdit)
                {
                    textEdit.Properties.AutoHeight = false;
                    textEdit.MinimumSize = new Size(0, height);
                    textEdit.MaximumSize = new Size(0, height);
                }
                else if (c.HasChildren)
                {
                    SetTextEditHeight(c, height); // Đệ quy
                }
            }
        }

        private async void FrmOrder_Load(object sender, EventArgs e)
        {
            SetTextEditHeight(this, 25);
            var branches = await _branchService.GetAllBranches();
            lkupBranch.Properties.DataSource = branches.Data;
            chkFinish.BackColor = Color.LightGreen;
            chkDraft.BackColor = Color.Green;
            chkDraft.ForeColor = Color.White;
            chkCancel.BackColor = Color.OrangeRed;
        }

        private void Handler_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not CheckEdit checkEdit) return;

            var statusValue = checkEdit.Name switch
            {
                "chkDraft" => 1,
                "chkFinish" => 3,
                "chkCancel" => 4,
                _ => 0
            };

            if (statusValue == 0) return;

            if (checkEdit.Checked)
            {
                if (!_orderStatusList.Contains(statusValue))
                    _orderStatusList.Add(statusValue);
            }
            else
            {
                _orderStatusList.Remove(statusValue);
                if (_orderStatusList.Count == 0)
                {
                    chkDraft.CheckedChanged -= Handler_CheckedChanged;
                    chkDraft.Checked = true;
                    _orderStatusList.Add(1);
                    chkDraft.CheckedChanged += Handler_CheckedChanged;
                }
            }
            LoadData();
        }
    }
}