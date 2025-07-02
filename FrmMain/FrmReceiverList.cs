using Be.Common.Tranfer.Request;
using Be.Common.Tranfer.Response;
using Be.Services.KiotViet;
using Be.Services.Pos;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using FrmMain.App;
using FrmMain.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Services.System;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmReceiverList : FrmBasePos, IReloadableForm
    {
        private readonly FrmMainF _mainForm;
        private readonly IKiotVietService _kiotVietService;
        private const string TransferUrl = "https://public.kiotapi.com/transfers";
        private int _statusFilter;
        private readonly IBranchService _branchService;
        private int _currentBranchId;

        private DateTime _searchFromTransferDate;
        private DateTime _searchToTransferDate;

        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 5;

        public FrmReceiverList(FrmMainF mainForm, IKiotVietService kiotVietService, IBranchService branchService,
            ISystemService systemService) : base(branchService, systemService)
        {
            _mainForm = mainForm;
            _kiotVietService = kiotVietService;
            _branchService = branchService;
            InitializeComponent();
        }

        private void FrmOrder_Shown(object sender, EventArgs e)
        {
            _statusFilter = 2;
        }

        public async Task ReLoadData(string code, long id)
        {
            await LoadData(code, id);
        }

        private async Task LoadData(string code, long id)
        {
            try
            {
                SetControlEnable(false);
                var request = new SearchTranferRequest()
                {
                    FromBranchIds = null,
                    ToBranchIds = [_currentBranchId],
                    Status = [_statusFilter],
                    PageSize = 100,
                    CurrentItem = 0,
                    FromTransferDate = _statusFilter ==2? _searchFromTransferDate : null,
                    ToTransferDate = _statusFilter ==2? _searchToTransferDate : null,
                    FromReceivedDate = _statusFilter ==3? _searchFromTransferDate : null,
                    ToReceivedDate = _statusFilter ==3? _searchToTransferDate : null,
                };

                var (success, content) = await _kiotVietService.CallApiAsync(TransferUrl, request, "GET");

                if (!success || string.IsNullOrWhiteSpace(content))
                {
                    MessageHelper.MsgBox(this,$"Có lỗi trong quá trình tải dữ liệu: {content}", MsgType.Error);
                    grdControlOrders.DataSource = null;
                    return;
                }

                var branches = await _branchService.GetAllBranches();
                var branchDict = branches.ToDictionary(b => b.BranchId, b => b.BranchName);
                
                var transferPagedResponse = JsonConvert.DeserializeObject<TranferPagedResponse>(content);
                if (transferPagedResponse?.Data == null)
                {
                    grdControlOrders.DataSource = null;
                    MessageHelper.MsgBox(this,"Không có phiếu nhận nào.", MsgType.Information);
                    return;
                }

                foreach (var transfer in transferPagedResponse.Data)
                {
                    transfer.FromBranchName = branchDict.GetValueOrDefault(transfer.FromBranchId, "");
                    transfer.ToBranchName = branchDict.GetValueOrDefault(transfer.ToBranchId, "");
                }
                grdControlOrders.DataSource = transferPagedResponse.Data;
                grdViewOrders.BestFitColumns();
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình tải dữ liệu: {exception}", MsgType.Error);
            }
            finally
            {
                SetControlEnable(true);
            }
        }

        private void grdViewOrders_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (sender is not GridView { FocusedRowHandle: >= 0 } view) return;
                var code = view.GetRowCellValue(view.FocusedRowHandle, "Code");
                var id = view.GetRowCellValue(view.FocusedRowHandle, "Id");
                if (code == null) return;
                {
                    if (FormHelper.OpenedForm(nameof(FrmTransferProcess), WuserControl.Order, out var openForm))
                    {
                        if (openForm is FrmTransferProcess processForm)
                        {
                            processForm.ReloadData(code.ToString(), Convert.ToInt64(id), false);
                        }
                    }
                    else
                    {
                        FrmTransferProcess.CurrentCode = code.ToString();
                        FrmTransferProcess.CurrentId = Convert.ToInt64(id);
                        FrmTransferProcess.Transfer = false;
                        var frmOrderInstance = _mainForm.ServiceProvider.GetRequiredService<FrmTransferProcess>();
                        Form frmOrder = frmOrderInstance;
                        FormHelper.NewFormNew(_mainForm, frmOrder, WuserControl.Order, nameof(FrmTransferProcess));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,"Lỗi khi chuyển dữ liệu", MsgType.Error);
            }
        }

        private async void FrmOrder_Load(object sender, EventArgs e)
        {
            try
            {
                var setting = AppGlobals.AppSetting.FirstOrDefault(s =>
                    s.ComputerName == Environment.MachineName &&
                    s.ModuleName == "Branch" &&
                    s.SettingKey == "BranchId");

                if (setting == null || string.IsNullOrWhiteSpace(setting.SettingValue))
                {
                    MessageHelper.MsgBox(this,"Không tìm thấy thông tin chi nhánh trên máy này.", MsgType.Error);
                    return;
                }

                if (!long.TryParse(setting.SettingValue, out var branchId))
                {
                    MessageHelper.MsgBox(this,"Mã chi nhánh không hợp lệ.", MsgType.Error);
                    return;
                }
                var branch = await _branchService.GetBranchById(branchId);
                _currentBranchId = branch?.BranchId ?? 0;
                txtBranchName.Text = branch?.BranchName ?? "Chưa chọn chi nhánh";
                txtBranchName.ReadOnly = true;
                SetStatusCheckboxStyle();
                StartCountdownTimer();
                SetDefaultDatePurchase();
                await LoadData("", 0);
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình tải dữ liệu: {exception}", MsgType.Error);
            }
        }
        private async void Handler_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not CheckEdit checkEdit) return;
            var checkedValue = checkEdit.Checked;

            var statusValue = checkEdit.Name switch
            {
                "chkDraft" => 1,
                "chkTransfer" => 2,
                "chkFinish" => 3,
                "chkCancel" => 4,
                _ => 0
            };

            if (statusValue == 0)
                return;

            chkTransfer.CheckedChanged -= Handler_CheckedChanged;
            chkFinish.CheckedChanged -= Handler_CheckedChanged;
            chkCancel.CheckedChanged -= Handler_CheckedChanged;

            chkTransfer.Checked = chkFinish.Checked = chkCancel.Checked = false;

            if (checkedValue)
            {
                switch (checkEdit.Name)
                {
                    case "chkTransfer":
                        chkTransfer.Checked = true;
                        break;
                    case "chkFinish":
                        chkFinish.Checked = true;
                        break;
                    case "chkCancel":
                        chkCancel.Checked = true;
                        break;
                }
                _statusFilter = statusValue;
            }
            else
            {
                chkTransfer.Checked = true;
                _statusFilter = 2;
            }

            chkTransfer.CheckedChanged += Handler_CheckedChanged;
            chkFinish.CheckedChanged += Handler_CheckedChanged;
            chkCancel.CheckedChanged += Handler_CheckedChanged;
            await LoadData("", 0);
        }

        private void fromPurchaseDate_EditValueChanged(object sender, EventArgs e)
        {
            if (fromTransferDate.EditValue == null || fromTransferDate.EditValue == DBNull.Value)
            {
                SetDefaultDatePurchase();
                return;
            }

            DateTime? fromPurchaseDate = fromTransferDate.DateTime;
            toTransferDate.Properties.MinValue = fromPurchaseDate.Value;
            if (toTransferDate.DateTime < fromPurchaseDate.Value)
            {
                toTransferDate.DateTime = fromPurchaseDate.Value;
            }

            _searchFromTransferDate = fromPurchaseDate.Value;
        }

        private void toPurchaseDate_EditValueChanged(object sender, EventArgs e)
        {
            if (toTransferDate.EditValue == null || toTransferDate.EditValue == DBNull.Value)
            {
                SetDefaultDatePurchase();
                return;
            }
            DateTime? toPurchaseDate = toTransferDate.DateTime;

            fromTransferDate.Properties.MaxValue = toPurchaseDate.Value;
            if (fromTransferDate.DateTime > toPurchaseDate.Value)
            {
                fromTransferDate.DateTime = toPurchaseDate.Value;
            }

            _searchToTransferDate = toPurchaseDate.Value;
        }

        private void SetDefaultDatePurchase()
        {
            // Lấy ngày hiện tại
            var today = DateTime.Today;

            // Tính ngày đầu tháng
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            // Tính ngày cuối tháng
            DateTime lastDayOfMonth = new(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            // Gán giá trị cho 2 DateEdit
            fromTransferDate.DateTime = firstDayOfMonth;
            toTransferDate.DateTime = lastDayOfMonth;

            // Optional: Giới hạn Min/Max cho chọn ngày
            fromTransferDate.Properties.MaxValue = lastDayOfMonth;
            toTransferDate.Properties.MinValue = firstDayOfMonth;
            _searchFromTransferDate = firstDayOfMonth;
            _searchToTransferDate = lastDayOfMonth;
        }

        private void SetStatusCheckboxStyle()
        {
            SetCheckboxColor(chkFinish, Color.LightGreen, Color.Black);
            SetCheckboxColor(chkCancel, Color.OrangeRed, Color.White);
            SetCheckboxColor(chkTransfer, Color.Cyan, Color.Black);
            txtBranchName.BackColor = Color.White;
            txtBranchName.ForeColor = Color.OrangeRed;
        }

        private static void SetCheckboxColor(CheckEdit checkEdit, Color backColor, Color foreColor)
        {
            checkEdit.BackColor = backColor;
            checkEdit.ForeColor = foreColor;
        }
        private void SetControlEnable(bool enable)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetControlEnable(enable)));
            }
            else
            {
                layoutControlTop.Enabled = enable;
                grdControlOrders.Enabled = enable;
            }
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

        // Tick mỗi giây
        private async void ReloadTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var remaining = _nextReloadTime - DateTime.Now;

                if (remaining <= TimeSpan.Zero)
                {
                    _reloadTimer.Stop();
                    btnReload.Text = "Tải dữ liệu...";
                    await LoadData("", 0);
                    // Khởi động lại đếm ngược
                    _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                    _reloadTimer.Start();
                }
                else
                {
                    btnReload.Text = $"Tải lại sau: {remaining.Minutes:D2}:{remaining.Seconds:D2}";
                }
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình tải dữ liệu: {exception}", MsgType.Error);
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

        private async void btnReload_Click(object sender, EventArgs e)
        {
            try
            {
                _reloadTimer?.Stop();
                await LoadData("", 0);
                btnReload.Text = "Tải dữ liệu...";
                _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                _reloadTimer?.Start();
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình tải dữ liệu: {exception}", MsgType.Error);
            }
        }
    }
}