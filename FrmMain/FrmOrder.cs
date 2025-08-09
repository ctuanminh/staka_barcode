using Be.Services.KiotViet;
using Be.Services.Pos;
using Be.Services.System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using FrmMain.Dto.Request;
using FrmMain.Dto.Response;
using FrmMain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmOrder : FrmBasePos, IReloadableForm
    {
        private readonly FrmMainF _mainForm;
        private readonly IKiotVietService _kiotVietService;
        private List<int> _orderStatusList;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 30;

        public FrmOrder(FrmMainF mainForm,
            IKiotVietService kiotVietService,
            IBranchService branchService,
            ISystemService systemService) : base(branchService, systemService)
        {
            _mainForm = mainForm;
            _kiotVietService = kiotVietService;
            InitializeComponent();
            StartCountdownTimer();
        }

        public async Task ReLoadData(string code, long id)
        {
            _orderStatusList = [1];
            await LoadDefaultSetting();
            if (IsHandleCreated && Visible)
            {
                await LoadData("", 0);
            }
        }

        private async Task LoadData(string code, long id)
        {
            try
            {
                if (!IsDisposed && !Disposing)
                    SetControlEnable(false);
                const string orderUrl = $"https://public.kiotapi.com/orders";
                var request = new SearchOrderRequest()
                {
                    BranchIds = [BranchId],
                    Status = _orderStatusList.ToArray(),
                    PageSize = 200,
                    OrderBy = "purchaseDate",
                    OrderDirection = "Desc"
                };
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, request);
                
                if (!success || string.IsNullOrWhiteSpace(content)) return;
                var orderPagedResponse = JsonConvert.DeserializeObject<OrderPagedResponse>(content);
                foreach (var order in orderPagedResponse.Data)
                {
                    if (string.IsNullOrWhiteSpace(order.CustomerName))
                        order.CustomerName = "Khách lẻ";
                }
                grdControlOrders.DataSource = orderPagedResponse.Data;
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox(this,"Lỗi trong quá trình lấy dữ liệu: " + exception, MsgType.Error);
            }
            finally
            {
                if (!IsDisposed && !Disposing)
                {
                    SetControlEnable(true);
                }
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
                        button.MinimumSize = new Size(0, 35);
                        button.MaximumSize = new Size(0, 35);
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

        private void FrmOrder_Load(object sender, EventArgs e)
        {
            try
            {
                SetTextEditHeight(this, 25);
                SetStatusCheckboxStyle();
                _orderStatusList = [1];
                txtBranch.Text = BranchName;
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error);
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
        private async void FrmOrder_Shown(object sender, EventArgs e)
        {
            try
            {
                await LoadData("", 0);
                txtBranch.Text = BranchName;
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox(this, "Lỗi trong quá trình Khởi tạo giao diện: " + exception, MsgType.Error);
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
                await LoadData("", 0);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error);
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
                    await LoadData("", 0);
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
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error);
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
                await LoadData("", 0);
                btnReloadOrder.Text = "Loading...";
                _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                _reloadTimer?.Start();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error);
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

        private void grdViewOrders_MouseMove(object sender, MouseEventArgs e)
        {
            var view = sender as GridView;
            var hitInfo = view.CalcHitInfo(e.Location);

            if (hitInfo.InRowCell && hitInfo.Column.FieldName == "Action")
            {
                grdControlOrders.Cursor = Cursors.Hand;
            }
            else
            {
                grdControlOrders.Cursor = Cursors.Default;
            }
        }
        
        private async void rpBtnAction_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (grdViewOrders.FocusedRowHandle < 0) return;

                var orderCode = grdViewOrders.GetRowCellValue(grdViewOrders.FocusedRowHandle, "Code")?.ToString();
                var orderId = grdViewOrders.GetRowCellValue(grdViewOrders.FocusedRowHandle, "Id")?.ToString();

                if (string.IsNullOrEmpty(orderCode) || string.IsNullOrEmpty(orderId)) return;
                await FormHelper.OpenFormWithScope<FrmOrderProcess>(_mainForm,
                    _mainForm.ServiceProvider,
                    orderCode,
                    Convert.ToInt64(orderId),
                    nameof(FrmOrderProcess),
                    WuserControl.OrderProcess);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,"Lỗi khi chuyển dữ liệu", MsgType.Error);
            }
        }

    }
}