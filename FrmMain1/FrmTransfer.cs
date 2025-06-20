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
using System.Threading.Tasks;
using System.Windows.Forms;
using FrmMain.App;
using static FrmMain.FrmMainF;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmTransfer : XtraForm
    {
        private readonly FrmMainF _mainForm;
        private readonly IKiotVietService _kiotVietService;
        private const string TransferUrl = "https://public.kiotapi.com/transfers";
        private List<int> _statusList;
        private readonly IBranchService _branchService;
        private int _currentBranchId;
        private DateTime _searchFromTransferDate;
        private DateTime _searchToTransferDate;

        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 1;
        public FrmTransfer(FrmMainF mainForm, IKiotVietService kiotVietService, IBranchService branchService)
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

        private async Task LoadData()
        {
            try
            {
                SetControlEnable(false);
                var request = new SearchTranferRequest()
                {
                    FromBranchIds = [_currentBranchId],
                    Status = _statusList.ToArray(),
                    PageSize = 100,
                    CurrentItem = 0,
                    FromTransferDate = _searchFromTransferDate,
                    ToTransferDate = _searchToTransferDate
                };

                var (success, content) = await _kiotVietService.CallApiAsync(TransferUrl, request, "GET");

                if (!success || string.IsNullOrWhiteSpace(content))
                {
                    grdControlOrders.DataSource = null;
                    return;
                }
                var transferPagedResponse = JsonConvert.DeserializeObject<TranferPagedResponse>(content);
                if (transferPagedResponse?.Data == null)
                {
                    grdControlOrders.DataSource = null;
                    return;
                }
                var branches = await _branchService.GetAllBranches();
                var branchDict = branches.ToDictionary(b => b.BranchId, b => b.BranchName);
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
                    if (FormHelper.OpenedForm(nameof(FrmTransferProcess), WuserControl.Order, out var openForm))
                    {
                        if (openForm is FrmTransferProcess processForm)
                        {
                            processForm.ReloadData(code.ToString(), Convert.ToInt64(id), true);
                        }
                    }
                    else
                    {
                        FrmTransferProcess.CurrentCode = code.ToString();
                        FrmTransferProcess.CurrentId = Convert.ToInt64(id);
                        FrmTransferProcess.Transfer = true;
                        var frmOrderInstance = _mainForm.ServiceProvider.GetRequiredService<FrmTransferProcess>();
                        Form frmOrder = frmOrderInstance;
                        FormHelper.NewFormNew(_mainForm, frmOrder, WuserControl.Order, nameof(FrmTransferProcess));
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
                                SetTextEditHeight(c, height);
                            }
                            break;
                        }
                }
            }
        }

        private async void FrmOrder_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Lấy thông tin setting
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
                // 2. Lấy thông tin chi nhánh từ service
                var branch = await _branchService.GetBranchById(branchId);
                _currentBranchId = branch?.BranchId ?? 0;

                // 3. Set UI hiển thị thông tin branch
                txtBranchName.Text = branch?.BranchName ?? "Chưa chọn chi nhánh";
                txtBranchName.ReadOnly = true;

                // 4. Thiết lập giao diện
                SetTextEditHeight(this, 25);
                SetStatusCheckboxStyle();

                // 5. Cài ngày mặc định (nên trước khi load data)
                SetDefaultDatePurchase();
                // 6. Load dữ liệu
                await LoadData();
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình tải dữ liệu: {exception}", MsgType.Error_);
            }
        }

        private void Handler_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not CheckEdit checkEdit) return;
            checkEdit.CheckStateChanged -= Handler_CheckedChanged;
            chkDraft.Checked = chkCancel.Checked = chkTransfer.Checked = false;

            var statusValue = checkEdit.Name switch
            {
                "chkDraft" => 1,
                "chkTransfer" => 2,
                "chkFinish" => 3,
                "chkCancel" => 4,
                _ => 0
            };

            if (statusValue == 0) return;

            //Trong COmmnet
            //if (checkEdit.Checked)
            //{
            //    _statusList = _statusList.RemoveAll();
            //    _statusList.Add(statusValue);
            //}
            else
            {
                if (_statusList.Count == 0)
                {
                    chkDraft.Checked = true;
                    _statusList.Add(1);
                    chkDraft.CheckedChanged += Handler_CheckedChanged;
                }
                _statusList.Remove(statusValue);
                
            }
            btnReload_Click(btnReloadPurchase, EventArgs.Empty);
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
            //set searchFromPurchaseDate, searchToPurchaseDate

            // Lấy ngày hiện tại
            var today = DateTime.Today;

            // Tính ngày đầu tháng
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            // Tính ngày cuối tháng: thêm giờ 23:59:59 vào ngày cuối cùng của tháng

            DateTime lastDayOfMonth = new(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            lastDayOfMonth = lastDayOfMonth.AddDays(1).AddTicks(-1);

            // Gán giá trị cho 2 DateEdit
            fromTransferDate.DateTime = firstDayOfMonth;
            toTransferDate.DateTime = lastDayOfMonth;

            // Optional: Giới hạn Min/Max cho chọn ngày
            fromTransferDate.Properties.MaxValue = lastDayOfMonth;
            toTransferDate.Properties.MinValue = firstDayOfMonth;
            _searchFromTransferDate = firstDayOfMonth;
            _searchToTransferDate = lastDayOfMonth;
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
                    btnReloadPurchase.Text = "Tải dữ liệu...";
                    await LoadData();
                    _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                    _reloadTimer.Start();
                }
                else
                {
                    btnReloadPurchase.Text = $"Tải lại sau: {remaining.Minutes:D2}:{remaining.Seconds:D2}";
                }
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình tải dữ liệu: {exception}", MsgType.Error_);
            }
        }

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
                await LoadData();
                btnReloadPurchase.Text = "Tải dữ liệu...";
                _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                _reloadTimer?.Start();
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình tải dữ liệu: {exception}", MsgType.Error_);
            }
        }

        private void SetStatusCheckboxStyle()
        {
            SetCheckboxColor(chkDraft, Color.Green, Color.White);
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
            layoutControlTop.Enabled = enable;
            grdControlOrders.Enabled = enable;
        }
    }
}