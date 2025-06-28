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
using Be.Common.Order.Dto;
using Be.Services.Order;
using Exception = System.Exception;
using OrderDetail = Be.Common.Order.Request.OrderDetail;
using OrderResponse = Be.Common.Order.Response.OrderResponse;
using Size = System.Drawing.Size;

namespace FrmMain
{
    public partial class FrmOrderProcess : XtraForm, IReloadableForm
    {
        #region Fileds
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentCode { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long CurrentOrderId { get; set; }
        private readonly IKiotVietService _kiotVietService;
        private int _branchId;
        private int _scannedBarcodeCount;
        private OrderResponse _orderResponse;
        private readonly IProductService _productService;
        private readonly IBranchService _branchService;
        private readonly ISystemService _systemService;
        private readonly IOrderCheckedService _orderCheckedService;
        private Dictionary<string, string> _productCodeBarCdeDic;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 30;

        #endregion

        #region Ctor
        public FrmOrderProcess(IKiotVietService kiotVietService, IProductService productService, IBranchService branchService,
            ISystemService systemService, IOrderCheckedService orderCheckedService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            _branchService = branchService;
            _systemService = systemService;
            _orderCheckedService = orderCheckedService;
            InitializeComponent();
            txtOrderCode.Text = CurrentCode;
            StartCountdownTimer();
        }

        #endregion


        public virtual async Task ReloadData(string code, long id)
        {
            CurrentCode = code;
            CurrentOrderId = id;
            txtOrderCode.Text = code;
            _scannedBarcodeCount = 0;
            await LoadData(code);
        }

        private async Task LoadProduct(List<OrderDetailResponse> orderDetailResponses)
        {
            try
            {
                var productCodes = orderDetailResponses
                    .Select(p => p.ProductCode)
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Distinct()
                    .ToList();
                var productCodeBarCode = await _productService.SynAndGetProductCodeBarCode(productCodes, _branchId);
                _productCodeBarCdeDic = new Dictionary<string, string>();
                foreach (var product in productCodeBarCode)
                {
                    if (!string.IsNullOrWhiteSpace(product.Code))
                    {
                        _productCodeBarCdeDic.TryAdd(product.Code, product.Code);
                    }

                    if (!string.IsNullOrWhiteSpace(product.BarCode))
                    {
                        _productCodeBarCdeDic.TryAdd(product.BarCode, product.Code);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error_);
            }
        }

        private async Task LoadData(string code)
        {
            try
            {
                if (!IsDisposed && !Disposing)
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

                // Xử lý trạng thái
                var status = (OrderStatusEnum)orderApiResponse.Status;

                // Nếu trạng thái khác Draft thì khoá luôn ProductCode
                txtProductCode.ReadOnly = status != OrderStatusEnum.Draft;

                // Load thông tin
                txtCustomerName.Text = orderApiResponse.CustomerName ?? "Khách lẻ";
                txtSaleName.Text = orderApiResponse.SoldByName;
                txtSumTotal.Text = NumberFormatter.FormatDecimal(orderApiResponse.Total);
                txtTotalPayment.Text = NumberFormatter.FormatDecimal(orderApiResponse.TotalPayment);
                txtTotal.Text = NumberFormatter.FormatDecimal(orderApiResponse.Total);
                txtDiscount.Text = NumberFormatter.FormatDecimal(orderApiResponse.OrderDetails.Sum(p => p.ViewDiscount * Convert.ToDecimal(p.Quantity))).ToString();
                _orderResponse = orderApiResponse;
                txtScanNumber.ReadOnly = true;
                foreach (var item in _orderResponse.OrderDetails)
                {
                    item.DisplayDiscount = item.DiscountRatio > 0
                        ? $"{NumberFormatter.FormatDecimal(item.Discount)} - {item.DiscountRatio}%"
                        : $"{NumberFormatter.FormatDecimal(item.Discount)}%";
                }

                var orderIsChecked = await _orderCheckedService.IsOrderChecked(_orderResponse.Id, _branchId);
                if (orderIsChecked)
                {
                   var result = MessageHelper.MsgBox("Tải dữ liệu đã quét trước đó?", MsgType.YesNo);
                   if (result == DialogResult.Yes)
                   {
                       var orderCheckedList =
                           await _orderCheckedService.GetOrderCheckedByOrderId(_orderResponse.Id, _branchId);
                       var productCheckedInOrderDict = new Dictionary<string, double>();

                       if (orderCheckedList.Any())
                       {
                           foreach (var checkedItem in orderCheckedList.Where(checkedItem =>
                                        !string.IsNullOrWhiteSpace(checkedItem.ProductCode)))
                           {
                               productCheckedInOrderDict.TryAdd(checkedItem.ProductCode, checkedItem.Count);
                           }
                       }

                       foreach (var item in _orderResponse.OrderDetails)
                       {
                           var productCheckedCount = productCheckedInOrderDict.GetValueOrDefault(item.ProductCode);
                           if (productCheckedCount <= 0) continue;
                           item.Checked = true;
                           item.ScanCount = productCheckedCount > item.Quantity ? item.Quantity : productCheckedCount;
                           _scannedBarcodeCount++;
                       }
                   }
                }
                txtScanNumber.Text = $"{_scannedBarcodeCount}/{orderApiResponse.OrderDetails.Count()}";

                gridControlOrder.DataSource = _orderResponse.OrderDetails;
                gridViewOrder.BestFitColumns();
                await LoadProduct(_orderResponse.OrderDetails);
                SetStatusControl(_orderResponse.Status);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu", MsgType.Error_);
            }
            finally
            {
                if (!IsDisposed && !Disposing)
                    SetControlEnable(true);
                txtProductCode.Focus();
            }
        }

        private async void FrmOrderProcess_Load(object sender, EventArgs e)
        {
            try
            {
                SetTextEditHeight(this, 25);
                BeginInvoke(() => txtProductCode.Focus());
                await LoadDefaultSetting();
                await ReloadData(CurrentCode, CurrentOrderId);
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình tải dữ liệu.", MsgType.Error_);
            }
        }

        private async void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode != Keys.Enter) return;
                var searchBarcode = txtProductCode.Text.Trim();
                txtProductCode.SelectAll();
                e.Handled = true;
                if (string.IsNullOrEmpty(searchBarcode)) return;
                var (isProductFound, productCode) = TryFindProductCode(searchBarcode);
                
                if (!isProductFound)
                {
                    MessageHelper.MsgBox($"Không tìm thấy sản phẩm mã: {searchBarcode}", MsgType.Error_);
                    return;
                }
                var product = _orderResponse.OrderDetails.FirstOrDefault(p => p.ProductCode == productCode);
                if (product == null)
                {
                    MessageHelper.MsgBox($"Không tìm thấy sản phẩm mã: {searchBarcode} trong đơn hàng", MsgType.Error_);
                    return;
                }
                
                if (!product.Checked) _scannedBarcodeCount++;
                product.Checked = true;
                if (product.ScanCount >= product.Quantity)
                {
                    MessageHelper.MsgBox($"Sản phẩm {product.ProductName} đã quét đủ số lượng yêu cầu.", MsgType.Information);
                }
                else
                {
                    product.ScanCount++;
                    // Chỉ gọi service khi còn quét
                    var productChecked =
                        await _orderCheckedService.FindProductChecked(_orderResponse.Id, product.ProductCode,
                            _branchId);
                    if (productChecked == null)
                    {
                        await _orderCheckedService.AddOrderCheck(new OrderCheckedDto()
                        {
                            OrderId = _orderResponse.Id,
                            OrderCode = _orderResponse.Code,
                            ProductCode = product.ProductCode,
                            ProductBarCode = searchBarcode,
                            BranchId = _branchId,
                            Count = product.ScanCount,
                            UserName = AppGlobals.UserInfo.UserName,
                        });
                    }
                    else
                    {
                        await _orderCheckedService.UpdateOrderCheck(productChecked.Id, product.ScanCount);
                    }
                }
                // Refresh UI
                gridControlOrder.RefreshDataSource();
                var rowHandle = gridViewOrder.LocateByValue("ProductCode", productCode);
                if (rowHandle >= 0)
                {
                    gridViewOrder.FocusedRowHandle = rowHandle;
                    gridViewOrder.MakeRowVisible(rowHandle);
                }
                txtScanNumber.Text = $"{_scannedBarcodeCount}/{_orderResponse.OrderDetails.Count}";
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình thực hiện.", MsgType.Error_);
            }
        }

        private void gridViewOrder_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (sender is not GridView view) return;
            if (view.FocusedColumn.FieldName != "ScanCount") return;
            if (view.GetRow(view.FocusedRowHandle) is not OrderDetailResponse row) return;

            if (row.Checked) return;
            e.Cancel = true;
        }

        private void gridViewOrder_ShownEditor(object sender, EventArgs e)
        {
            var view = sender as GridView;
            var editor = gridViewOrder.ActiveEditor;
            if (editor == null) return;
            editor.KeyDown += Editor_KeyDown;
            editor.Focus();
            editor.SelectAll();
        }
        private async void gridViewOrder_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                if (sender is not GridView view || view.FocusedColumn.FieldName != "ScanCount") return;

                if (!int.TryParse(e.Value?.ToString(), out var scanCount))
                {
                    e.Value = 0;
                    return;
                }

                scanCount = Math.Max(0, scanCount);

                if (gridViewOrder.GetRow(gridViewOrder.FocusedRowHandle) is not OrderDetailResponse row) return;

                var validCount = Math.Min(scanCount, row.Quantity);
                e.Value = scanCount;

                var productChecked = await _orderCheckedService.FindProductChecked(_orderResponse.Id, row.ProductCode, _branchId);
                if (productChecked == null)
                {
                    await _orderCheckedService.AddOrderCheck(new OrderCheckedDto
                    {
                        OrderId = _orderResponse.Id,
                        OrderCode = _orderResponse.Code,
                        ProductCode = row.ProductCode,
                        ProductBarCode = "", // nhập tay không có barcode
                        BranchId = _branchId,
                        Count = validCount,
                        UserName = AppGlobals.UserInfo.UserName,
                    });
                }
                else
                {
                    await _orderCheckedService.UpdateOrderCheck(productChecked.Id, validCount);
                }
            }
            catch
            {
                MessageHelper.MsgBox("Có lỗi khi cập nhật số lần quét.", MsgType.Error_);
            }
        }
        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            gridViewOrder.CloseEditor();
            gridViewOrder.UpdateCurrentRow();
            BeginInvoke(() =>
            {
                txtProductCode.Focus();
            });
        }
        private void gridViewOrder_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (sender is not GridView view) return;

            if (view.GetRow(e.RowHandle) is not OrderDetailResponse row) return;
            var scanCount = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "ScanCount"));
            var quantity = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "Quantity"));
            if (!row.Checked) return;
            if (scanCount != quantity)
            {
                e.Appearance.BackColor = Color.LightCoral; // màu đỏ nhạt
                e.Appearance.ForeColor = Color.Black;
                return;
            }
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
                    // Nếu ScanCount != Quantity thì cảnh báo không cho hoàn thành.
                    if (_orderResponse.OrderDetails.Any(p => p.ScanCount != p.Quantity))
                    {
                        MessageHelper.MsgBox("Vui lòng kiểm tra số lượng trước khi hoàn thành", MsgType.Error_);
                        return;
                    }
                    if (_scannedBarcodeCount == _orderResponse.OrderDetails.Count)
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
                    }
                    break;
            }
        }

        private async void txtOrderCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
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
                await LoadData(orderCode);
                txtProductCode.Focus();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình thực hiện.", MsgType.Error_);
            }
        }

        private (bool check, string code) TryFindProductCode(string searchBarCode)
        {
            return _productCodeBarCdeDic.TryGetValue(searchBarCode, out var codeValue) ? (true, codeValue) : (false, null);
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

        private async void btnReloadOrder_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentCode = txtOrderCode.Text.Trim();
                _reloadTimer?.Stop();
                await ReloadData(CurrentCode, CurrentOrderId);
                btnReloadOrder.Text = "Đang tải dữ liệu...";
                _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                _reloadTimer?.Start();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình xử lý đơn hàng.", MsgType.Error_);
            }
        }

        private async void FinishOrder()
        {
            try
            {
                if (!IsDisposed && !Disposing)
                    SetControlEnable(false);

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
                await ReloadData(CurrentCode, CurrentOrderId);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình xử lý đơn hàng.", MsgType.Error_);
            }
            finally
            {
                if (!IsDisposed && !Disposing)
                    SetControlEnable(true);
            }
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

        private void SetStatusControl(int status = 1)
        {
            chkStatus.Checked = true;
            switch (status)
            {
                case 1:
                    chkStatus.Text = "Phiếu tạm";
                    chkStatus.BackColor = Color.Green;
                    chkStatus.ForeColor = Color.White;
                    txtOrderCode.BackColor = Color.Green;
                    txtOrderCode.ForeColor = Color.White;
                    btnFinish.Enabled = true;
                    break;
                case 3:
                    chkStatus.Text = "Hoàn thành";
                    chkStatus.BackColor = Color.LightGreen;
                    txtOrderCode.ForeColor = Color.Black;
                    txtOrderCode.BackColor = Color.LightGreen;
                    btnFinish.Enabled = false;
                    break;
                case 2:
                    chkStatus.Text = "Đã huỷ";
                    chkStatus.BackColor = Color.OrangeRed;
                    txtOrderCode.BackColor = Color.OrangeRed;
                    btnFinish.Enabled = false;
                    break;
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
    }

}