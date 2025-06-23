using Be.Services.KiotViet;
using Be.Services.Pos;
using Be.Services.System;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using FrmMain.App;
using FrmMain.Dto.Request;
using FrmMain.Dto.Response;
using FrmMain.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Core.Entities;
using static FrmMain.FrmMainF;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmOrder : XtraForm
    {
        private readonly FrmMainF _mainForm;
        private readonly IKiotVietService _kiotVietService;
        private List<int> _orderStatusList;
        private int _branchId;
        private readonly IBranchService _branchService;
        private readonly ISystemService _systemService;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 5;
        public FrmOrder(FrmMainF mainForm, IKiotVietService kiotVietService, IBranchService branchService, 
            ISystemService systemService)
        {
            _mainForm = mainForm;
            _kiotVietService = kiotVietService;
            _branchService = branchService;
            _systemService = systemService;
            InitializeComponent();
            StartCountdownTimer();
        }

        private void FrmOrder_Shown(object sender, EventArgs e)
        {
        }

        private async Task LoadData()
        {
            try
            {
                SetControlEnable(false);
                const string orderUrl = $"https://public.kiotapi.com/orders";
                var request = new SearchOrderRequest()
                {
                    BranchIds = [AppGlobals.BranchId],
                    Status = _orderStatusList.ToArray(),
                    PageSize = 200,
                    OrderBy = "purchaseDate",
                    OrderDirection = "Desc"
                };
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, request, "GET");
                //Log request để the dõi số lượng gọi API lên kiotviet
                await _systemService.AddRequest(new RequestEntity()
                {
                    Module = Name,
                    Url = orderUrl,
                    IsSuccess = success,
                    BranchId = _branchId
                });
                if (!success || string.IsNullOrWhiteSpace(content)) return;
                var orderPagedResponse = JsonConvert.DeserializeObject<OrderPagedResponse>(content);
                foreach (var order in orderPagedResponse.Data)
                {
                    if(string.IsNullOrWhiteSpace(order.CustomerName))
                        order.CustomerName = "Khách lẻ";
                }
                grdControlOrders.DataSource = orderPagedResponse.Data;
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox("Lỗi trong quá trình lấy dữ liệu: " + exception, MsgType.Error_);
            }
            finally
            {
                if (!IsDisposed && !Disposing)
                {
                    SetControlEnable(true);
                }
            }
            
        }

        private void grdViewOrders_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (sender is not GridView { FocusedRowHandle: >= 0 } view) return;
                var code = view.GetRowCellValue(view.FocusedRowHandle, "Code");
                var orderId = view.GetRowCellValue(view.FocusedRowHandle, "Id");

                if (code == null || orderId == null)
                {
                    MessageHelper.MsgBox("Không tìm thấy đơn hàng", MsgType.Error_);
                    return;
                }
                if (FormHelper.OpenedForm(nameof(FrmOrderProcess), WuserControl.Order, out var openForm))
                {
                    if (openForm is FrmOrderProcess processForm)
                    {
                        processForm.ReloadData(code.ToString(), orderId.ToString());
                    }
                }
                else
                {
                    FrmOrderProcess.CurrentCode = code.ToString();
                    FrmOrderProcess.CurrentOrderId = orderId.ToString();
                    var frmOrderInstance = _mainForm.ServiceProvider.GetRequiredService<FrmOrderProcess>();
                    Form frmOrder = frmOrderInstance;
                    FormHelper.NewFormNew(_mainForm, frmOrder, WuserControl.Order, nameof(FrmOrderProcess));
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
                                SetTextEditHeight(c, height); // Đệ quy
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
                SetTextEditHeight(this, 25);
                await LoadDefaultSetting();
                SetStatusCheckboxStyle();
                _orderStatusList = [1];
                await LoadData();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error_);
            }
            finally
            {
                chkDraft.CheckedChanged -= Handler_CheckedChanged;
                chkFinish.CheckedChanged -= Handler_CheckedChanged;
                chkCancel.CheckedChanged -= Handler_CheckedChanged;

                chkDraft.CheckedChanged += Handler_CheckedChanged;
                chkFinish.CheckedChanged += Handler_CheckedChanged;
                chkCancel.CheckedChanged += Handler_CheckedChanged;
            }
        }

        private async void Handler_CheckedChanged(object sender, EventArgs e)
        {
            try
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
                await LoadData();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error_);
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
                    btnReloadOrder.Text = "Loading...";
                    await LoadData(); 
                    // Khởi động lại đếm ngược
                    _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                    _reloadTimer.Start();
                }
                else
                {
                    btnReloadOrder.Text = $"Tải lại sau: {remaining.Minutes:D2}:{remaining.Seconds:D2}";
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error_);
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

        private async void btnReloadOrder_Click(object sender, EventArgs e)
        {
            try
            {
                _reloadTimer?.Stop();
                await LoadData();
                btnReloadOrder.Text = "Loading...";
                _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                _reloadTimer?.Start();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error_);
            }
        }

        private void SetControlEnable(bool enable)
        {
            if (layoutControlTop != null)
                layoutControlTop.Enabled = enable;
            if (grdControlOrders != null)
                grdControlOrders.Enabled = enable;
        }

        private void SetStatusCheckboxStyle()
        {
            chkFinish.BackColor = Color.LightGreen;
            chkDraft.BackColor = Color.Green;
            chkDraft.ForeColor = Color.White;
            chkCancel.BackColor = Color.OrangeRed;
            chkCancel.ForeColor = Color.White;
            txtBranch.BackColor = Color.White;
            txtBranch.ForeColor = Color.OrangeRed;
        }

        private async Task LoadDefaultSetting()
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
            _branchId = branch?.BranchId ?? 0;
            txtBranch.Text = branch?.BranchName ?? "Chưa chọn chi nhánh";
            txtBranch.ReadOnly = true;
        }
    }
}