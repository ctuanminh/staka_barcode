using Be.Common.Order.Request;
using Be.Common.Order.Response;
using Be.Core.Entities;
using Be.Services.Catalog;
using Be.Services.KiotViet;
using Be.Services.Pos;
using Be.Services.System;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using FrmMain.App;
using FrmMain.Dto.Response;
using FrmMain.Report;
using FrmMain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exception = System.Exception;
using OrderDetail = Be.Common.Order.Request.OrderDetail;
using OrderResponse = Be.Common.Order.Response.OrderResponse;
using Size = System.Drawing.Size;

namespace FrmMain
{
    public partial class FrmOrderProcess : XtraForm
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentCode { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentOrderId { get; set; }
        private bool _isReloading;
        private readonly IKiotVietService _kiotVietService;
        private int _branchId;
        private int _scannedBarcodeCount;
        private OrderResponse _orderResponse;
        private readonly IProductService _productService;
        private readonly IBranchService _branchService;
        private readonly ISystemService _systemService;
        private Dictionary<string, string> _productLookupDictionary;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 30;
        public FrmOrderProcess(IKiotVietService kiotVietService, IProductService productService, IBranchService branchService, 
            ISystemService systemService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            _branchService = branchService;
            _systemService = systemService;
            InitializeComponent();
            //ReloadData(CurrentCode);
            txtOrderCode.Text = CurrentCode;
            StartCountdownTimer();
        }

        public void ReloadData(string code, string id)
        {
            CurrentCode = code;
            CurrentOrderId = id;
            txtOrderCode.Text = code;
            _scannedBarcodeCount = 0;
            LoadData(code);
        }

        private async void LoadProduct(List<OrderDetailResponse> orderDetailResponses)
        {
            try
            {
                var productCodes = orderDetailResponses
                    .Select(p => p.ProductCode)
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Distinct()
                    .ToList();
                var productCodeBarCode = await _productService.SynAndGetProductCodeBarCode(productCodes, _branchId);
                _productLookupDictionary = new Dictionary<string, string>();
                foreach (var product in productCodeBarCode)
                {
                    if (!string.IsNullOrWhiteSpace(product.Code))
                    {
                        _productLookupDictionary.TryAdd(product.Code, product.Code);
                    }

                    if (!string.IsNullOrWhiteSpace(product.BarCode))
                    {
                        _productLookupDictionary.TryAdd(product.BarCode, product.Code);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error_);
            }
        }

        private async void LoadData(string code)
        {
            try
            {
                SetControlEnable(false);

                var orderUrl = $"https://public.kiotapi.com/orders/code/{code}";
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, (string)null, "GET");
                await _systemService.AddRequest(new RequestEntity()
                {
                    Module = "Order",
                    Url = orderUrl,
                    IsSuccess = success,
                    BranchId = _branchId
                });
                if (!success || string.IsNullOrWhiteSpace(content))
                {
                    MessageHelper.MsgBox("Lỗi khi lấy dữ liệu Kiotviet", MsgType.Error_);
                    return;
                }

                var orderApiResponse = JsonConvert.DeserializeObject<OrderResponse>(content);
                if (orderApiResponse == null)
                {
                    MessageHelper.MsgBox("Không có dữ liệu trả về từ API", MsgType.Error_);
                    return;
                }

                // Xử lý tên sản phẩm tách đơn vị
                foreach (var orderApi in orderApiResponse.OrderDetails)
                {
                    var start = orderApi.ProductName.LastIndexOf('(');
                    var end = orderApi.ProductName.LastIndexOf(')');
                    if (start == -1 || end <= start) continue;
                    orderApi.Unit = orderApi.ProductName.Substring(start + 1, end - start - 1).Trim();
                    orderApi.ProductName = orderApi.ProductName[..start].Trim();
                }

                // Reset trạng thái check
                chkFinish.Checked = chkCancel.Checked = chkDraft.Checked = false;

                // Xử lý trạng thái
                var status = (OrderStatusEnum)orderApiResponse.Status;

                // Nếu trạng thái khác Draft thì khoá luôn ProductCode
                txtProductCode.ReadOnly = status != OrderStatusEnum.Draft;

                // Load thông tin
                txtCustomerName.Text = orderApiResponse.CustomerName;
                txtSaleName.Text = orderApiResponse.SoldByName;
                txtSumTotal.Text = NumberFormatter.FormatDecimal(orderApiResponse.Total);
                txtTotalPayment.Text = NumberFormatter.FormatDecimal(orderApiResponse.TotalPayment);
                txtTotal.Text = NumberFormatter.FormatDecimal(orderApiResponse.Total);

                _orderResponse = orderApiResponse;
                txtScanNumber.ReadOnly = true;
                txtScanNumber.Text = $"{_scannedBarcodeCount}/{orderApiResponse.OrderDetails.Count()}";

                foreach (var item in _orderResponse.OrderDetails)
                {
                    item.DisplayDiscount = item.DiscountRatio > 0
                        ? $"{NumberFormatter.FormatDecimal(item.Discount)} - {item.DiscountRatio}%"
                        : $"{NumberFormatter.FormatDecimal(item.Discount)}%";
                }

                gridControlOrder.DataSource = _orderResponse.OrderDetails;
                gridViewOrder.BestFitColumns();
                LoadProduct(_orderResponse.OrderDetails);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu", MsgType.Error_);
            }
            finally
            {
                SetControlEnable(true);
                txtProductCode.Focus();
                SetStatusColor();
                SetOrderStatusUI(_orderResponse);
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
                        if (c.Name is nameof(btnFinish)) break;
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

        private void SetControlEnable(bool enable)
        {
            layoutControlTop.Enabled = enable;
            gridControlOrder.Enabled = enable;
        }

        private void SetStatusColor()
        {
            chkFinish.BackColor = Color.LightGreen;
            chkCancel.BackColor = Color.OrangeRed;
            chkCancel.ForeColor = Color.White;
            chkDraft.BackColor = Color.Green;
            chkDraft.ForeColor = Color.White;
            switch (_orderResponse.Status)
            {
                case (int)OrderStatusEnum.Finished:
                    txtOrderCode.ForeColor = Color.Black;
                    txtOrderCode.BackColor = Color.LightGreen;
                    break;
                case (int)OrderStatusEnum.Cancel:
                    txtOrderCode.BackColor = Color.OrangeRed;
                    break;
                default:
                    txtOrderCode.BackColor = Color.Green;
                    txtOrderCode.ForeColor = Color.White;
                    break;
            }
        }

        private void FrmOrderProcess_Load(object sender, EventArgs e)
        {
            try
            {
                SetTextEditHeight(this, 25);
                BeginInvoke(() => txtProductCode.Focus());
                SetStatusCheckboxStyle();
               _= LoadDefaultSetting();
                ReloadData(CurrentCode, CurrentOrderId);
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình tải dữ liệu.", MsgType.Error_);
            }
        }

        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            var searchBarcode = txtProductCode.Text.Trim();
            txtProductCode.SelectAll();
            if (string.IsNullOrEmpty(searchBarcode)) return;
            var (isProductFound, productCode) = TryFindProductCode(searchBarcode);
            e.Handled = true;
            if (isProductFound)
            {
                var findProduct = _orderResponse.OrderDetails.FirstOrDefault(p => p.ProductCode == productCode);
                if (findProduct != null)
                {
                    if (findProduct.Checked) return;
                    _scannedBarcodeCount++;
                    findProduct.Checked = true;
                    gridControlOrder.RefreshDataSource();
                    var rowHandle = gridViewOrder.LocateByValue("ProductCode", productCode);
                    if (rowHandle < 0) return;
                    gridViewOrder.FocusedRowHandle = rowHandle;
                    gridViewOrder.MakeRowVisible(rowHandle);
                    txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" + _orderResponse.OrderDetails.Count().ToString();
                }
                else
                {
                    MessageHelper.MsgBox("Không tìm thấy sản phẩm mã: " + searchBarcode + " trong đơn hàng", MsgType.Error_);
                }
            }
            else
            {
                MessageHelper.MsgBox("Không tìm thấy sản phẩm mã: " + searchBarcode, MsgType.Error_);
            }
        }
        private void gridViewOrder_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (sender is not GridView view) return;

            if (view.GetRow(e.RowHandle) is not OrderDetailResponse row) return;

            if (!row.Checked) return;
            e.Appearance.BackColor = Color.LightGreen; // Màu xanh nhạt
            e.Appearance.ForeColor = Color.Black;      // Text màu đen (tuỳ chọn)
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            switch (_orderResponse.Status)
            {
                case (int)OrderStatusEnum.Finished:
                    MessageHelper.MsgBox("Đơn hàng đã hoàn thành, vui lòng kiểm tra lại", MsgType.Error_);
                    break;
                case (int)OrderStatusEnum.Cancel:
                    MessageHelper.MsgBox("Đơn hàng đã huỷ, vui lòng kiểm tra lại", MsgType.Error_);
                    break;
                default:
                    if (_scannedBarcodeCount == _orderResponse.OrderDetails.Count())
                    {
                        var confirm = MessageHelper.MsgBox("Hoàn thành đơn hàng", MsgType.YesNo);
                        if (confirm != DialogResult.Yes) return;
                        FinishOrder();
                    }
                    else
                    {
                        var listNotScan = _orderResponse.OrderDetails.Where(p => !p.Checked)
                            .Select(p => p.ProductCode)
                            .ToList();
                        var message =
                            $"Còn {listNotScan.Count} sản phẩm chưa quét mã: {string.Join(", ", listNotScan)}.\nVui lòng thực hiện trước khi hoàn thành.";
                        MessageHelper.MsgBox(message, MsgType.Error_);
                        txtProductCode.Focus();
                        return;
                    }

                    break;
            }
        }

        private void txtOrderCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            var orderCode = txtOrderCode.Text.Trim();

            if (_scannedBarcodeCount > 0 && _orderResponse.OrderDetails.Any(p => p.Checked))
            {
                var result = MessageHelper.MsgBox("Bạn chắc chắn tải lại dữ liệu", MsgType.YesNo);
                if (result != DialogResult.Yes) return;
            }

            if (string.IsNullOrEmpty(orderCode)) return;
            _scannedBarcodeCount = 0;
            LoadData(orderCode);
            txtProductCode.Focus();
        }

        private (bool check, string code) TryFindProductCode(string searchBarCode)
        {
            return _productLookupDictionary.TryGetValue(searchBarCode, out var codeValue) ? (true, codeValue) : (false, null);
        }

        private void FrmOrderProcess_Shown(object sender, EventArgs e)
        {
            txtProductCode.Focus();
        }

        // Tick mỗi giây
        private void ReloadTimer_Tick(object sender, EventArgs e)
        {
            var remaining = _nextReloadTime - DateTime.Now;

            if (remaining <= TimeSpan.Zero)
            {
                _reloadTimer.Stop();
                btnReloadOrder.Text = "Loading...";

                LoadData(CurrentCode); // gọi reload dữ liệu

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
            ReloadData(CurrentCode, CurrentOrderId);
            btnReloadOrder.Text = "Đang tải dữ liệu...";
            _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
            _reloadTimer?.Start();
        }

        private async void FinishOrder()
        {
            try
            {
                layoutControlTop.Enabled = false;
                gridControlOrder.Enabled = false;

                var orderUrl = $"https://public.kiotapi.com/orders/{CurrentOrderId}";
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, (string)null, "GET");

                if (!success || string.IsNullOrEmpty(content))
                {
                    MessageHelper.MsgBox("Lỗi khi lấy dữ liệu Kiotviet", MsgType.Error_);
                    return;
                }

                var orderApiResponse = JsonConvert.DeserializeObject<OrderResponse>(content);
                if (orderApiResponse == null)
                {
                    MessageHelper.MsgBox("Dữ liệu đơn hàng trả về không hợp lệ", MsgType.Error_);
                    return;
                }

                switch ((OrderStatusEnum)orderApiResponse.Status)
                {
                    case OrderStatusEnum.Finished:
                        MessageHelper.MsgBox($"Đơn hàng: {CurrentCode} đã Hoàn thành", MsgType.Information);
                        return;

                    case OrderStatusEnum.Cancel:
                        MessageHelper.MsgBox($"Đơn hàng: {CurrentCode} đã Huỷ", MsgType.Information);
                        return;

                    case OrderStatusEnum.Draft:
                        break;

                    default:
                        MessageHelper.MsgBox($"Trạng thái đơn hàng không hợp lệ", MsgType.Error_);
                        return;
                }

                // Build orderRequest từ dữ liệu hiện tại
                var orderRequest = new OrderKiotRequest
                {
                    purchaseDate = orderApiResponse.PurchaseDate,
                    branchId = (int)orderApiResponse.BranchId,
                    soldById = orderApiResponse.SoldById,
                    discount = orderApiResponse.Discount,
                    description = orderApiResponse.Description,
                    method = orderApiResponse.UsingCod ? "COD" : "ONLINE",
                    totalPayment = orderApiResponse.TotalPayment,
                    makeInvoice = true,
                    orderDetails = orderApiResponse.OrderDetails.Select(product => new OrderDetail
                    {
                        productId = product.ProductId,
                        productCode = product.ProductCode,
                        productName = product.ProductName,
                        isMaster = product.IsMaster,
                        quantity = product.Quantity,
                        price = product.Price,
                        discount = product.Discount,
                        discountRatio = product.DiscountRatio
                    }).ToList(),
                    customer = string.IsNullOrEmpty(orderApiResponse.CustomerCode) ? null : new Customer
                    {
                        id = orderApiResponse.CustomerId,
                        code = orderApiResponse.CustomerCode,
                        name = orderApiResponse.CustomerName,
                        gender = false,
                        birthDate = DateTime.MinValue,
                        contactNumber = "",
                        address = "",
                        wardName = "",
                        email = "",
                        comments = ""
                    }
                };

                var (updateSuccess, updateContent) = await _kiotVietService.CallApiAsync(orderUrl, orderRequest, "PUT");

                if (!updateSuccess || string.IsNullOrEmpty(updateContent))
                {
                    var apiErrorResponse = JsonConvert.DeserializeObject<ApiErrorResponse>(updateContent);
                    MessageHelper.MsgBox($"Có lỗi khi cập nhật đơn hàng: {apiErrorResponse.ResponseStatus.Message}", MsgType.Error_);
                    return;
                }

                MessageHelper.MsgBox("Đơn hàng đã được hoàn thành thành công.", MsgType.Information);
                _isReloading = true;
                ReloadData(CurrentCode, CurrentOrderId);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình xử lý đơn hàng.", MsgType.Error_);
            }
            finally
            {
                layoutControlTop.Enabled = true;
                gridControlOrder.Enabled = true;
            }
        }

        private void btnPrintOrder_Click(object sender, EventArgs e)
        {
            PrintOrderGrid();
        }
        private void PrintOrderGrid()
        {
            var report = new OrderReport(_orderResponse);
            report.ShowPreview();
        }

        private void SetOrderStatusUI(OrderResponse orderApiResponse)
        {
            chkFinish.Checked = orderApiResponse.Status == 3;
            chkCancel.Checked = orderApiResponse.Status == 2;
            chkDraft.Checked = orderApiResponse.Status == 1;

            // Ẩn nút Draft nếu đã hoàn thành
            if (orderApiResponse.Status != 1)
                chkDraft.Visible = false;
            chkDraft.Refresh();
        }

        private void SetStatusCheckboxStyle()
        {
            chkFinish.BackColor = Color.LightGreen;
            chkDraft.BackColor = Color.Green;
            chkDraft.ForeColor = Color.White;
            chkCancel.BackColor = Color.OrangeRed;
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
        }
    }

}