using Be.Common.Purchase_Order.Request;
using Be.Common.Purchase_Order.Response;
using Be.Services.KiotViet;
using Be.Services.Pos;
using DevExpress.XtraCharts.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using FrmMain.App;
using FrmMain.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
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
        private DateTime searchFromPurchase;
        private DateTime searchToPurchase;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 15;
        public FrmPurchase(FrmMainF mainForm, IKiotVietService kiotVietService, IBranchService branchService)
        {
            _mainForm = mainForm;
            _kiotVietService = kiotVietService;
            _branchService = branchService;
            InitializeComponent();
            StartCountdownTimer();
        }

        private void FrmOrder_Shown(object sender, EventArgs e)
        {
            _PurchaseStatusList = [1];
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var setting = AppGlobals.AppSetting.FirstOrDefault(s =>
                    s.ComputerName == Environment.MachineName &&
                    s.ModuleName == "Branch" &&
                    s.SettingKey == "BranchId");

                if (setting == null || string.IsNullOrWhiteSpace(setting.SettingValue))
                {
                    MessageHelper.MsgBox("Không tìm thấy thông tin chi nhánh trên máy này.", MsgType.Error_);
                    return;
                }

                if (!long.TryParse(setting.SettingValue, out var branchId))
                {
                    MessageHelper.MsgBox("Mã chi nhánh không hợp lệ.", MsgType.Error_);
                    return;
                }

                var branch = await _branchService.GetBranchById(branchId);
                branchId = branch?.Id ?? 0;
                txtBranch.Text = branch?.BranchName ?? "Chưa chọn chi nhánh";
                txtBranch.ReadOnly = true;
                txtBranch.BackColor = Color.White;
                txtBranch.ForeColor = Color.OrangeRed;

                layoutControlTop.Enabled = false;
                grdControlOrders.Enabled = false;
                var request = new SearchPurchaseOrderRequest()
                {
                    BranchIds = [AppGlobals.BranchId],
                    Status = _PurchaseStatusList.ToArray(),
                    PageSize = 100,
                    OrderBy = "purchaseDate",
                    OrderDirection = "Desc",
                    FromPurchaseDate = searchFromPurchase,
                    ToPurchaseDate = searchToPurchase,
                };

                var (success, content) = await _kiotVietService.CallApiAsync(PurchaseOrderUrl, request, "GET");

                if (!success || content == null) return;
                var purchaseOrderPagedData = JsonConvert.DeserializeObject<PurchaseOrderPagedData>(content);
                grdViewOrders.OptionsDetail.EnableMasterViewMode = false;
                //Sort Data: Sort theo PurchaseDate
                purchaseOrderPagedData.Data.Sort((x, y) => DateTime.Compare(y.PurchaseDate, x.PurchaseDate));
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
                if (purchaseOrderId <= 0) return;
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
            chkFinish.BackColor = Color.LightGreen;
            chkDraft.BackColor = Color.Green;
            chkDraft.ForeColor = Color.White;
            chkCancel.BackColor = Color.OrangeRed;
            chkCancel.ForeColor = Color.White;
            SetDefaultDatePurchase();
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

        private void fromPurchaseDate_EditValueChanged(object sender, EventArgs e)
        {
            if (fromPurchaseDate.EditValue == null || fromPurchaseDate.EditValue == DBNull.Value)
            {
                SetDefaultDatePurchase();
                return;
            }

            DateTime? _fromPurchaseDate = fromPurchaseDate.DateTime;
            toPurchaseDate.Properties.MinValue = _fromPurchaseDate.Value;
            if (toPurchaseDate.DateTime < _fromPurchaseDate.Value)
            {
                toPurchaseDate.DateTime = _fromPurchaseDate.Value;
            }

            searchFromPurchase = _fromPurchaseDate.Value;
        }

        private void toPurchaseDate_EditValueChanged(object sender, EventArgs e)
        {
            if (toPurchaseDate.EditValue == null || toPurchaseDate.EditValue == DBNull.Value)
            {
                SetDefaultDatePurchase();
                return;
            }
            DateTime? _toPurchaseDate = toPurchaseDate.DateTime;

            fromPurchaseDate.Properties.MaxValue = _toPurchaseDate.Value;
            if (fromPurchaseDate.DateTime > _toPurchaseDate.Value)
            {
                fromPurchaseDate.DateTime = _toPurchaseDate.Value;
            }

            searchToPurchase = _toPurchaseDate.Value;
        }

        private void SetDefaultDatePurchase()
        {
            //set searchFromPurchaseDate, searchToPurchaseDate

            // Lấy ngày hiện tại
            var today = DateTime.Today;

            // Tính ngày đầu tháng
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            // Tính ngày cuối tháng
            DateTime lastDayOfMonth = new(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            // Gán giá trị cho 2 DateEdit
            fromPurchaseDate.DateTime = firstDayOfMonth;
            toPurchaseDate.DateTime = lastDayOfMonth;

            // Optional: Giới hạn Min/Max cho chọn ngày
            fromPurchaseDate.Properties.MaxValue = lastDayOfMonth;
            toPurchaseDate.Properties.MinValue = firstDayOfMonth;
            searchFromPurchase = firstDayOfMonth;
            searchToPurchase = lastDayOfMonth;
        }

        // Tick mỗi giây
        private void ReloadTimer_Tick(object sender, EventArgs e)
        {
            var remaining = _nextReloadTime - DateTime.Now;

            if (remaining <= TimeSpan.Zero)
            {
                _reloadTimer.Stop();
                btnReloadPurchase.Text = "Loading...";
                LoadData();
                // Khởi động lại đếm ngược
                _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                _reloadTimer.Start();
            }
            else
            {
                btnReloadPurchase.Text = $"Tải lại sau: {remaining.Minutes:D2}:{remaining.Seconds:D2}";
            }
        }

        // Hàm khởi động Timer đếm ngược
        private void StartCountdownTimer()
        {
            _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);

            _reloadTimer = new Timer();
            _reloadTimer.Interval = 1000; // mỗi 1 giây
            _reloadTimer.Tick += ReloadTimer_Tick;
            _reloadTimer.Start();
        }

        private void btnReloadOrder_Click(object sender, EventArgs e)
        {
            _reloadTimer?.Stop();
            LoadData();
            btnReloadPurchase.Text = "Loading...";
            _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
            _reloadTimer?.Start();
        }
    }
}