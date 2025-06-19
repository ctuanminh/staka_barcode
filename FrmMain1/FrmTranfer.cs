using Be.Common.Tranfer.Request;
using Be.Common.Tranfer.Response;
using Be.Services.KiotViet;
using Be.Services.Pos;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using FrmMain.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FrmMain.App;
using static FrmMain.FrmMainF;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmTranfer : XtraForm
    {
        private readonly FrmMainF _mainForm;
        private readonly IKiotVietService _kiotVietService;
        private const string TranferUrl = "https://public.kiotapi.com/transfers";
        private List<int> _statusList;
        private readonly IBranchService _branchService;
        private int _branchIdTranfer;
        private int _branchIdReceiver;
        private int currentBranchId;
        private DateTime searchFromTransferDate;
        private DateTime searchToTransferDate;

        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 1;
        public FrmTranfer(FrmMainF mainForm, IKiotVietService kiotVietService, IBranchService branchService)
        {
            _mainForm = mainForm;
            _kiotVietService = kiotVietService;
            _branchService = branchService;
            InitializeComponent();
            StartCountdownTimer();
        }

        private void FrmOrder_Shown(object sender, EventArgs e)
        {
            _statusList = [1];
        }

        private async void LoadData()
        {
            try
            {
                SetControlEnable(false);
                var request = new SearchTranferRequest()
                {
                    FromBranchIds = [currentBranchId],
                    ToBranchIds = null,
                    Status = _statusList.ToArray(),
                    PageSize = 100,
                    CurrentItem = 1,
                    FromTransferDate = searchFromTransferDate,
                    ToTransferDate = searchToTransferDate
                };

                var (success, content) = await _kiotVietService.CallApiAsync(TranferUrl, request, "GET");

                if (!success || content == null) return;
                var tranferPagedResponse = JsonConvert.DeserializeObject<TranferPagedResponse>(content);

                var branches = await _branchService.GetAllBranches();
                foreach (var transfer in tranferPagedResponse.Data)
                {
                    var fromBranch = branches.FirstOrDefault(b => b.BranchId == transfer.FromBranchId);
                    var toBranch = branches.FirstOrDefault(b => b.BranchId == transfer.ToBranchId);

                    transfer.FromBranchName = fromBranch != null ? fromBranch.BranchName : "";
                    transfer.ToBranchName = toBranch != null ? toBranch.BranchName : "";
                }
                grdControlOrders.DataSource = tranferPagedResponse.Data;
                grdViewOrders.BestFitColumns();
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox("Lỗi gọi API: " + exception, MsgType.Error_);
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
                    if (FormHelper.OpenedForm(nameof(FrmTranferProcess), WuserControl.Order, out var openForm))
                    {
                        if (openForm is FrmTranferProcess processForm)
                        {
                            processForm.ReloadData(code.ToString(), Convert.ToInt64(id), true);
                        }
                    }
                    else
                    {
                        FrmTranferProcess.CurrentCode = code.ToString();
                        FrmTranferProcess.CurrentId = Convert.ToInt64(id);
                        FrmTranferProcess.Tranfer = true;
                        var frmOrderInstance = _mainForm.ServiceProvider.GetRequiredService<FrmTranferProcess>();
                        Form frmOrder = frmOrderInstance;
                        FormHelper.NewFormNew(_mainForm, frmOrder, WuserControl.Order, nameof(FrmTranferProcess));
                    }
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
            chkTranfer.BackColor = Color.Cyan;
            chkTranfer.ForeColor = Color.Black;

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
            currentBranchId = branch?.BranchId ?? 0;
            txtBranchName.Text = branch?.BranchName ?? "Chưa chọn chi nhánh";
            txtBranchName.ReadOnly = true;
            SetDefaultDatePurchase();
            LoadData();
        }

        private void Handler_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not CheckEdit checkEdit) return;

            var statusValue = checkEdit.Name switch
            {
                "chkDraft" => 1,
                "chkTranfer" => 2,
                "chkFinish" => 3,
                "chkCancel" => 4,
                _ => 0
            };

            if (statusValue == 0) return;

            if (checkEdit.Checked)
            {
                if (!_statusList.Contains(statusValue))
                    _statusList.Add(statusValue);
            }
            else
            {
                _statusList.Remove(statusValue);
                if (_statusList.Count == 0)
                {
                    chkDraft.CheckedChanged -= Handler_CheckedChanged;
                    chkDraft.Checked = true;
                    _statusList.Add(1);
                    chkDraft.CheckedChanged += Handler_CheckedChanged;
                }
            }

            btnReloadTranfer_Click(btnReloadPurchase, EventArgs.Empty);
        }

        private void SetControlEnable(bool enable)
        {
            layoutControlTop.Enabled = enable;
            grdControlOrders.Enabled = enable;
        }

        private void fromPurchaseDate_EditValueChanged(object sender, EventArgs e)
        {
            if (fromTransferDate.EditValue == null || fromTransferDate.EditValue == DBNull.Value)
            {
                SetDefaultDatePurchase();
                return;
            }

            DateTime? _fromPurchaseDate = fromTransferDate.DateTime;
            toTransferDate.Properties.MinValue = _fromPurchaseDate.Value;
            if (toTransferDate.DateTime < _fromPurchaseDate.Value)
            {
                toTransferDate.DateTime = _fromPurchaseDate.Value;
            }

            searchFromTransferDate = _fromPurchaseDate.Value;
        }

        private void toPurchaseDate_EditValueChanged(object sender, EventArgs e)
        {
            if (toTransferDate.EditValue == null || toTransferDate.EditValue == DBNull.Value)
            {
                SetDefaultDatePurchase();
                return;
            }
            DateTime? _toPurchaseDate = toTransferDate.DateTime;

            fromTransferDate.Properties.MaxValue = _toPurchaseDate.Value;
            if (fromTransferDate.DateTime > _toPurchaseDate.Value)
            {
                fromTransferDate.DateTime = _toPurchaseDate.Value;
            }

            searchToTransferDate = _toPurchaseDate.Value;
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
            fromTransferDate.DateTime = firstDayOfMonth;
            toTransferDate.DateTime = lastDayOfMonth;

            // Optional: Giới hạn Min/Max cho chọn ngày
            fromTransferDate.Properties.MaxValue = lastDayOfMonth;
            toTransferDate.Properties.MinValue = firstDayOfMonth;
            searchFromTransferDate = firstDayOfMonth;
            searchToTransferDate = lastDayOfMonth;
        }

        // Tick mỗi giây
        private void ReloadTimer_Tick(object sender, EventArgs e)
        {
            var remaining = _nextReloadTime - DateTime.Now;

            if (remaining <= TimeSpan.Zero)
            {
                _reloadTimer.Stop();
                btnReloadPurchase.Text = "Tải dữ liệu...";
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

        private void btnReloadTranfer_Click(object sender, EventArgs e)
        {
            _reloadTimer?.Stop();
            LoadData();
            btnReloadPurchase.Text = "Tải dữ liệu...";
            _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
            _reloadTimer?.Start();
        }
    }
}