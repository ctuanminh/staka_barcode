using Be.Common.PurchaseOrder.Request;
using Be.Common.PurchaseOrder.Response;
using Be.Services.KiotViet;
using Be.Services.Pos;
using Be.Services.System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using FrmMain.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmPurchase : FrmBasePos, IReloadableForm
    {
        private readonly FrmMainF _mainForm;
        private readonly IKiotVietService _kiotVietService;
        private const string PurchaseOrderUrl = "https://public.kiotapi.com/purchaseorders";
        private List<int> _purchaseStatusList;
        private DateTime _searchFromPurchase;
        private DateTime _searchToPurchase;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 60;

        public FrmPurchase(FrmMainF mainForm, IKiotVietService kiotVietService, IBranchService branchService,
            ISystemService systemService)
            : base(branchService, systemService)
        {
            _mainForm = mainForm;
            _kiotVietService = kiotVietService;
            InitializeComponent();
            StartCountdownTimer();
        }

        public async Task ReLoadData(string code, long id)
        {
            try
            {
                _purchaseStatusList = [1];
                await LoadData("", 0);
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình tải dữ liệu: {exception}", MsgType.Error);
            }
        }

        private async Task LoadData(string code, long id)
        {
            try
            {
                SetControlEnable(false);
                await LoadDefaultSetting();

                var request = new SearchPurchaseOrderRequest
                {
                    BranchIds = [BranchId],
                    Status = _purchaseStatusList.ToArray(),
                    PageSize = 100,
                    OrderBy = "purchaseDate",
                    OrderDirection = "Desc",
                    FromPurchaseDate = _searchFromPurchase,
                    ToPurchaseDate = _searchToPurchase
                };

                var (success, content) = await _kiotVietService.CallApiAsync(PurchaseOrderUrl, request);

                if (!success || string.IsNullOrWhiteSpace(content))
                {
                    MessageHelper.MsgBox(this,"Không thể tải dữ liệu từ hệ thống.", MsgType.Error);
                    grdControlOrders.DataSource = null;
                    return;
                }

                PurchaseOrderPagedData purchaseOrderPagedData;
                try
                {
                    purchaseOrderPagedData = JsonConvert.DeserializeObject<PurchaseOrderPagedData>(content);
                }
                catch
                {
                    MessageHelper.MsgBox(this,"Dữ liệu trả về không hợp lệ.", MsgType.Error);
                    grdControlOrders.DataSource = null;
                    return;
                }

                if (purchaseOrderPagedData?.Data == null || purchaseOrderPagedData.Data.Count == 0)
                {
                    grdControlOrders.DataSource = null;
                    return;
                }
                purchaseOrderPagedData.Data.Sort((x, y) => y.PurchaseDate.CompareTo(x.PurchaseDate));

                grdControlOrders.DataSource = purchaseOrderPagedData.Data;
                grdViewOrders.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,"Có lỗi xảy ra trong quá trình tải dữ liệu.", MsgType.Error);
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
                var purchaseOrderId = Convert.ToInt64(view.GetRowCellValue(view.FocusedRowHandle, "Id"));
                var purchaseOrderCode = view.GetRowCellValue(view.FocusedRowHandle, "Code");
                if (purchaseOrderId <= 0) return;
                var tabKey = $"edit_purchase";
                if (FormHelper.OpenedKeyForm(nameof(FrmAddPurchase),tabKey, out var openForm))
                {
                    if (openForm is FrmAddPurchase processForm)
                    {
                        processForm.ReloadData(purchaseOrderId, purchaseOrderCode.ToString());
                    }
                    openForm.Focus();
                }
                else
                {
                    FrmAddPurchase.CurrentCode = purchaseOrderCode.ToString();
                    FrmAddPurchase.CurrentId = purchaseOrderId;
                    FrmAddPurchase.IsEditMode = true;
                    var frmPurchaseInstance = _mainForm.ServiceProvider.GetRequiredService<FrmAddPurchase>();
                    Form frmPurchase = frmPurchaseInstance;
                    FormHelper.ShowManyForm(_mainForm, frmPurchase, WuserControl.FrmPurchaseAdd, tabKey);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,"Lỗi khi chuyển dữ liệu", MsgType.Error);
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
                SetDefaultDatePurchase();
                InitGridView();
                txtBranchName.Text = BranchName;
                grdViewOrders.MouseMove += grdViewOrders_MouseMove;
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình tải dữ liệu: {exception}", MsgType.Error);
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
                    if (!_purchaseStatusList.Contains(statusValue))
                        _purchaseStatusList.Add(statusValue);
                }
                else
                {
                    _purchaseStatusList.Remove(statusValue);
                    if (_purchaseStatusList.Count == 0)
                    {
                        chkDraft.CheckedChanged -= Handler_CheckedChanged;
                        chkDraft.Checked = true;
                        _purchaseStatusList.Add(1);
                        chkDraft.CheckedChanged += Handler_CheckedChanged;
                    }
                }

                await LoadData("", 0);
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình tải dữ liệu: {exception}", MsgType.Error);
            }
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

            _searchFromPurchase = _fromPurchaseDate.Value;
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

            _searchToPurchase = _toPurchaseDate.Value;
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
            _searchFromPurchase = firstDayOfMonth;
            _searchToPurchase = lastDayOfMonth;
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
                    btnReloadPurchase.Text = "Loading...";
                    await LoadData("", 0);
                    // Khởi động lại đếm ngược
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

        private async void btnReloadOrder_Click(object sender, EventArgs e)
        {
            try
            {
                _reloadTimer?.Stop();
                await LoadData("", 0);
                btnReloadPurchase.Text = "Loading...";
                _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                _reloadTimer?.Start();
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình tải dữ liệu: {exception}", MsgType.Error);
            }
        }

        private void SetStatusCheckboxStyle()
        {
            SetCheckboxColor(chkFinish, Color.LightGreen, Color.Black);
            SetCheckboxColor(chkDraft, Color.Green, Color.White);
            SetCheckboxColor(chkCancel, Color.OrangeRed, Color.White);
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

        private void InitGridView()
        {
            grdViewOrders.OptionsDetail.EnableMasterViewMode = false;
            grdViewOrders.Columns["PurchaseDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            grdViewOrders.Columns["PurchaseDate"].DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
        }

        private async void btnAddPurchase_Click(object sender, EventArgs e)
        {
            await FormHelper.OpenManyFormWithScope<FrmAddPurchase>(
                _mainForm, _mainForm.ServiceProvider,
                "", 0, 
                "AddPurchase", WuserControl.FrmPurchase);
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
                await FormHelper.OpenFormWithScope<FrmPurchaseProcess>(_mainForm,
                    _mainForm.ServiceProvider,
                    orderCode,
                    Convert.ToInt64( orderId),
                    nameof(FrmPurchaseProcess),
                    WuserControl.OrderProcess);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,"Lỗi khi chuyển dữ liệu", MsgType.Error);
            }
        }
    }
}