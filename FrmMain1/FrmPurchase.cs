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
using Be.Common.Purchase_Order.Request;
using Be.Common.Purchase_Order.Response;
using Be.Services.KiotViet;
using Be.Services.Pos;
using static FrmMain.FrmMainF;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmPurchase : XtraForm
    {
        private readonly FrmMainF _mainForm;
        private readonly IKiotVietService _kiotVietService;
        private const string PurchaseOrderUrl = "https://public.kiotapi.com/purchaseorders";
        private List<int> _PurchaseStatusList;
        private readonly IBranchService _branchService;
        private int _branchId = 631782;
        public FrmPurchase(FrmMainF mainForm, IKiotVietService kiotVietService, IBranchService branchService)
        {
            _mainForm = mainForm;
            _kiotVietService = kiotVietService;
            _branchService = branchService;
            InitializeComponent();
        }

        private async void FrmOrder_Shown(object sender, EventArgs e)
        {
            _PurchaseStatusList = [1];
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
                var request = new SearchPurchaseOrderRequest()
                {
                    BranchIds = [_branchId],
                    Status = _PurchaseStatusList.ToArray(),
                    PageSize = 100,
                    OrderBy = "purchaseDate",
                    OrderDirection = "Desc"
                };

                var (success, content) = await _kiotVietService.CallApiAsync(PurchaseOrderUrl, request, "GET");

                if (!success || content == null) return;
                var purchaseOrderPagedData = JsonConvert.DeserializeObject<PurchaseOrderPagedData>(content);
                grdViewOrders.OptionsDetail.EnableMasterViewMode = false;
                grdControlOrders.DataSource = purchaseOrderPagedData.Data;
                grdViewOrders.Columns["PurchaseDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                grdViewOrders.Columns["PurchaseDate"].DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
                grdViewOrders.BestFitColumns();
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
                var purchaseOrderId = Convert.ToInt64(view.GetRowCellValue(view.FocusedRowHandle, "Id"));
                var purchaseOrderCode = view.GetRowCellValue(view.FocusedRowHandle, "Code");
                if (purchaseOrderId <=0) return;
                if (FormHelper.OpenedForm(nameof(FrmPurchaseProcess), WuserControl.Order, out var openForm))
                {
                    if (openForm is FrmPurchaseProcess processForm)
                    {
                        processForm.ReloadData(purchaseOrderId);
                    }
                }
                else
                {
                    FrmPurchaseProcess.CurrentCode = purchaseOrderCode.ToString();
                    FrmPurchaseProcess.CurrentId = purchaseOrderId;
                    var frmPurchaseInstance = _mainForm.ServiceProvider.GetRequiredService<FrmPurchaseProcess>();
                    Form frmPurchase = frmPurchaseInstance;
                    FormHelper.NewFormNew(_mainForm, frmPurchase, WuserControl.Order, nameof(FrmPurchaseProcess));
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Lỗi khi chuyển dữ liệu", MsgType.Error_);
            }
        }

        private void SetTextEditHeight(Control control, int height)
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

        private async void FrmOrder_Load(object sender, EventArgs e)
        {
            SetTextEditHeight(this, 25);
            var branches = await _branchService.GetPagedBranches();
            lkupBranch.Properties.DataSource = branches.Data;
            chkFinish.BackColor = Color.LightGreen;
            chkDraft.BackColor = Color.Green;
            chkDraft.ForeColor = Color.White;
            chkCancel.BackColor = Color.OrangeRed;
            chkCancel.ForeColor = Color.White;
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
                if (!_PurchaseStatusList.Contains(statusValue))
                    _PurchaseStatusList.Add(statusValue);
            }
            else
            {
                _PurchaseStatusList.Remove(statusValue);
                if (_PurchaseStatusList.Count == 0)
                {
                    chkDraft.CheckedChanged -= Handler_CheckedChanged;
                    chkDraft.Checked = true;
                    _PurchaseStatusList.Add(1);
                    chkDraft.CheckedChanged += Handler_CheckedChanged;
                }
            }
            LoadData();
        }

        private void lkupBranch_EditValueChanged(object sender, EventArgs e)
        {
            var branchId = (int)lkupBranch.EditValue;
            _branchId = branchId;
            LoadData();
        }
    }
}