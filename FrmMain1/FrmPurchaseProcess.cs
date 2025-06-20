using Be.Common.Purchase_Order.Response;
using Be.Services.Catalog;
using Be.Services.KiotViet;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using FrmMain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmPurchaseProcess : XtraForm
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentCode { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static long CurrentId { get; set; }
        private readonly IKiotVietService _kiotVietService;
        private int _scannedBarcodeCount;
        private PurchaseOrderResponse _orderResponse;
        private readonly IProductService _productService;
        private Dictionary<string, string> _productLookupDictionary;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 60;
        public FrmPurchaseProcess(IKiotVietService kiotVietService, IProductService productService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            InitializeComponent();
            txtOrderCode.Text = CurrentCode;
            StartCountdownTimer();
        }

        public void ReloadData(long purchaseId)
        {
            CurrentId = purchaseId;
            txtOrderCode.Text = CurrentCode;
            _scannedBarcodeCount = 0;
            LoadData(purchaseId);
        }

        private async void LoadProduct(List<PurchaseOrderDetail> purchaseOrderDetails)
        {
            try
            {
                var productCodes = purchaseOrderDetails
                    .Select(p => p.ProductCode)
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Distinct()
                    .ToList();
                var productCodeBarCode = await _productService.SynAndGetProductCodeBarCode(productCodes);
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

        private async void LoadData(long purchaseId)
        {
            try
            {
                SetControlEnable(false);
                var url = $"https://public.kiotapi.com/purchaseorders/{purchaseId}";
                var (success, content) = await _kiotVietService.CallApiAsync(url, (string)null, "GET");
                if (!success && content == null) MessageBox.Show("Lỗi khi lấy dữ liệu Kiotviet");

                var purchaseOrderResponse = JsonConvert.DeserializeObject<PurchaseOrderResponse>(content);
                if (purchaseOrderResponse == null) return;
                foreach (var purchaseOrderDetail in purchaseOrderResponse.PurchaseOrderDetails)
                {
                    var start = purchaseOrderDetail.ProductName.LastIndexOf('(');
                    var end = purchaseOrderDetail.ProductName.LastIndexOf(')');

                    if (start == -1 || end <= start) continue;
                    purchaseOrderDetail.Unit = purchaseOrderDetail.ProductName.Substring(start + 1, end - start - 1).Trim();
                    purchaseOrderDetail.ProductName = purchaseOrderDetail.ProductName[..start].Trim();
                }
                SetStatusCheckboxStyle();
                
                // Reset trạng thái
                chkFinish.Checked = false;
                chkCancel.Checked = false;
                chkDraft.Checked = false;

                txtCustomerName.Text = purchaseOrderResponse.SupplierName; // Tên Người nhập
                txtSaleName.Text = purchaseOrderResponse.PurchaseName; // Tên nhà cung cấp.
                txtPurchaseDate.Text = purchaseOrderResponse.PurchaseDate.ToString("dd/MM/yyyy HH:mm:ss");
                txtTotal.Text = NumberFormatter.FormatDecimal(purchaseOrderResponse.Total, 0);
                txtDiscount.Text = NumberFormatter.FormatDecimal(purchaseOrderResponse.Discount, 0);
                txtNeedPayment.Text =
                    NumberFormatter.FormatDecimal(purchaseOrderResponse.Total - purchaseOrderResponse.Discount, 0);
                txtTotalPayment.Text = NumberFormatter.FormatDecimal(purchaseOrderResponse.TotalPayment, 0);
                txtTotalItems.Text = purchaseOrderResponse.PurchaseOrderDetails.Count().ToString();
                
                // Đếm tổng số sản phẩm
                txtProductCount.Text = purchaseOrderResponse.PurchaseOrderDetails
                    .Where(p => !string.IsNullOrWhiteSpace(p.ProductCode))
                    .Select(p => p.Quantity)
                    .Sum().ToString();
                txtDescription.Text = purchaseOrderResponse.Description; // Ghi chú

                _orderResponse = purchaseOrderResponse;
                txtScanNumber.ReadOnly = true;
                txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" +
                                     purchaseOrderResponse.PurchaseOrderDetails.Count().ToString();
                
                grdControlOrders.DataSource = _orderResponse.PurchaseOrderDetails;
                grdViewOrder.BestFitColumns();
                LoadProduct(_orderResponse.PurchaseOrderDetails);
                switch ((OrderStatusEnum)purchaseOrderResponse.Status)
                {
                    case OrderStatusEnum.Finished:
                        MessageHelper.MsgBox($"Phiếu: {purchaseOrderResponse.Code} đã Nhập hàng", MsgType.Error_);
                        txtProductCode.ReadOnly = true;
                        chkFinish.Checked = true;
                        txtProductCode.ReadOnly = true;
                        break;
                    case OrderStatusEnum.Cancel:
                        MessageHelper.MsgBox($"Phiếu nhập hàng: {purchaseOrderResponse.Code} đã Huỷ", MsgType.Error_);
                        chkCancel.Checked = true;
                        txtProductCode.ReadOnly = true;
                        break;
                    case OrderStatusEnum.Draft:
                        chkDraft.Checked = true;
                        txtProductCode.ReadOnly = false;
                        break;
                    default:
                        txtProductCode.ReadOnly = true;
                        break;
                }

                txtOrderCode.Focus();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu", MsgType.Error_);
            }
            finally
            {
                txtProductCode.Focus();
                SetControlEnable(true);
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

        private void FrmPurchaseProcess_Load(object sender, EventArgs e)
        {
            SetTextEditHeight(this, 25);
            BeginInvoke(() => txtProductCode.Focus());
            SetStatusCheckboxStyle();
            ReloadData(CurrentId);
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
                var findProduct = _orderResponse.PurchaseOrderDetails.FirstOrDefault(p => p.ProductCode == productCode);
                if (findProduct != null)
                {
                    if (findProduct.Checked) return;
                    _scannedBarcodeCount++;
                    findProduct.Checked = true;
                    grdControlOrders.RefreshDataSource();
                    var rowHandle = grdViewOrder.LocateByValue("ProductCode", productCode);
                    if (rowHandle < 0) return;
                    grdViewOrder.FocusedRowHandle = rowHandle;
                    grdViewOrder.MakeRowVisible(rowHandle);
                    txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" + _orderResponse.PurchaseOrderDetails.Count().ToString();
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

            if (view.GetRow(e.RowHandle) is not PurchaseOrderDetail row) return;

            if (!row.Checked) return;
            e.Appearance.BackColor = Color.LightGreen; 
            e.Appearance.ForeColor = Color.Black; 
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            switch (_orderResponse.Status)
            {
                case (int)OrderStatusEnum.Finished:
                    MessageHelper.MsgBox("Đơn Nhập hàng đã hoàn thành, vui lòng kiểm tra lại", MsgType.Error_);
                    break;
                case (int)OrderStatusEnum.Cancel:
                    MessageHelper.MsgBox("Đơn Nhập hàng đã huỷ, vui lòng kiểm tra lại", MsgType.Error_);
                    break;
                default:
                    if (_scannedBarcodeCount == _orderResponse.PurchaseOrderDetails.Count())
                    {
                        var confirm = MessageHelper.MsgBox("Chắc chắn hoàn thành đơn Nhập hàng", MsgType.YesNo);
                        if (confirm != DialogResult.Yes) return;
                        FinishOrder();
                    }
                    else
                    {
                        var listNotScan = _orderResponse.PurchaseOrderDetails.Where(p => !p.Checked)
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

        private void txtOrderCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            var orderCode = txtOrderCode.Text.Trim();

            if (_scannedBarcodeCount > 0 && _orderResponse.PurchaseOrderDetails.Any(p => p.Checked))
            {
                var result = MessageHelper.MsgBox("Bạn chắc chắn tải lại dữ liệu", MsgType.YesNo);
                if (result != DialogResult.Yes) return;
            }

            if (string.IsNullOrEmpty(orderCode)) return;
            _scannedBarcodeCount = 0;
            LoadData(CurrentId);
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

                LoadData(CurrentId); // gọi reload dữ liệu

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
            ReloadData(CurrentId);
            btnReloadOrder.Text = "Loading...";
            _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
            _reloadTimer?.Start();
        }

       private async void FinishOrder()
       {
            try
            {
                SetControlEnable(false);

                var orderUrl = $"https://public.kiotapi.com/purchaseorders/{CurrentId}";
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, (string)null, "GET");

                if (!success || string.IsNullOrEmpty(content))
                {
                    MessageHelper.MsgBox("Lỗi khi lấy dữ liệu Kiotviet", MsgType.Error_);
                    return;
                }

                var purchaseOrderResponse = JsonConvert.DeserializeObject<PurchaseOrderResponse>(content);
                if (purchaseOrderResponse == null)
                {
                    MessageHelper.MsgBox("Dữ liệu đơn hàng trả về không hợp lệ", MsgType.Error_);
                    return;
                }

                switch ((OrderStatusEnum)purchaseOrderResponse.Status)
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
                var orderRequest = new 
                {
                    purchaseDate = purchaseOrderResponse.PurchaseDate,
                    branchId = (int)purchaseOrderResponse.BranchId,
                    supplier = new
                    {
                        code = purchaseOrderResponse.SupplierCode,
                        name = purchaseOrderResponse.SupplierName,
                    },
                    description = purchaseOrderResponse.Description,
                    isDraft = 3,

                    discount = purchaseOrderResponse.Discount,
                    discountRatio = 0,
                    paidAmount = 0,
                    //paymentMethod = null,
                    surcharges = new List<object>(),
                    totalPayment = purchaseOrderResponse.TotalPayment,
                    makeInvoice = true,
                    purchaseOrderDetails = purchaseOrderResponse.PurchaseOrderDetails.Select(product => new PurchaseOrderDetail
                    {
                        ProductId = product.ProductId,
                        ProductCode = product.ProductCode,
                        ProductName = product.ProductName,
                        Quantity = product.Quantity,
                        Price = product.Price,
                        Discount = product.Discount,
                        DiscountRatio = product.DiscountRatio
                    }).ToList(),
                };

                var (updateSuccess, updateContent) = await _kiotVietService.CallApiAsync(orderUrl, orderRequest, "PUT");

                if (!updateSuccess || string.IsNullOrEmpty(updateContent))
                {
                    MessageHelper.MsgBox($"Có lỗi khi cập nhật đơn hàng: {updateContent}", MsgType.Error_);
                    return;
                }

                MessageHelper.MsgBox("Nhập hàng thành thành công.", MsgType.Information);
                ReloadData(CurrentId);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình xử lý đơn hàng.", MsgType.Error_);
            }
            finally
            {
                SetControlEnable(true);
            }
        }

       private void SetControlEnable(bool enable)
       {
           layoutControlTop.Enabled = enable;
           grdControlOrders.Enabled = enable;
       }
       private void SetStatusCheckboxStyle()
       {
           SetCheckboxColor(chkFinish, Color.LightGreen, Color.Black);
           SetCheckboxColor(chkDraft, Color.Green, Color.White);
           SetCheckboxColor(chkCancel, Color.OrangeRed, Color.White);           
           txtOrderCode.BackColor = Color.White;
           txtOrderCode.ForeColor = Color.OrangeRed;
        }

       private static void SetCheckboxColor(CheckEdit checkEdit, Color backColor, Color foreColor)
       {
           checkEdit.BackColor = backColor;
           checkEdit.ForeColor = foreColor;
       }
    }

}