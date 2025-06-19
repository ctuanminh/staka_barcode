using Be.Services.KiotViet;
using Be.Services.Pos;
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
        private List<int> _orderStatusList;
        private readonly IBranchService _branchService;
        private int _branchId = 1000002446;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 15;
        public FrmOrder(FrmMainF mainForm, IKiotVietService kiotVietService, IBranchService branchService)
        {
            _mainForm = mainForm;
            _kiotVietService = kiotVietService;
            _branchService = branchService;
            InitializeComponent();
            StartCountdownTimer();
        }

        private void FrmOrder_Shown(object sender, EventArgs e)
        {
            _orderStatusList = [1];
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                SetControlEnable(false);
                const string orderUrl = $"https://public.kiotapi.com/orders";
                var request = new SearchOrderRequest()
                {
                    //Comment để test
                    BranchIds = [AppGlobals.BranchId],
                    //BranchIds = [_branchId],
                    Status = _orderStatusList.ToArray(),
                    PageSize = 200,
                    OrderBy = "purchaseDate",
                    OrderDirection = "Desc"
                };
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, request, "GET");
                if (!success || string.IsNullOrWhiteSpace(content)) return;
                var orderPagedResponse = JsonConvert.DeserializeObject<OrderPagedResponse>(content);
                grdControlOrders.DataSource = orderPagedResponse.Data;
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
            try
            {
                SetTextEditHeight(this, 25);

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
                txtBranch.Text = branch?.BranchName ?? "Chưa chọn chi nhánh";
                txtBranch.ReadOnly = true;
                txtBranch.BackColor = Color.White;
                txtBranch.ForeColor = Color.OrangeRed;
                SetStatusCheckboxStyle();
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox("Lỗi khi tải dữ liệu: " + exception.Message, MsgType.Error_);
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

        // Tick mỗi giây
        private void ReloadTimer_Tick(object sender, EventArgs e)
        {
            var remaining = _nextReloadTime - DateTime.Now;

            if (remaining <= TimeSpan.Zero)
            {
                _reloadTimer.Stop();
                btnReloadOrder.Text = "Loading...";
                LoadData(); 
                // Khởi động lại đếm ngược
                _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                _reloadTimer.Start();
            }
            else
            {
                btnReloadOrder.Text = $"Tải lại sau: {remaining.Minutes:D2}:{remaining.Seconds:D2}";
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
            btnReloadOrder.Text = "Loading...";
            _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
            _reloadTimer?.Start();
        }

        private void SetControlEnable(bool enable)
        {
            layoutControlTop.Enabled = enable;
            grdControlOrders.Enabled = enable;
        }

        private void SetStatusCheckboxStyle()
        {
            chkFinish.BackColor = Color.LightGreen;
            chkDraft.BackColor = Color.Green;
            chkDraft.ForeColor = Color.White;
            chkCancel.BackColor = Color.OrangeRed;
            chkCancel.ForeColor = Color.White;
        }
    }
}