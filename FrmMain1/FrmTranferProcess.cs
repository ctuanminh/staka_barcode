using Be.Common.Order.Request;
using Be.Common.Order.Response;
using Be.Common.Purchase_Order.Response;
using Be.Common.Tranfer.Response;
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
    public partial class FrmTranferProcess : XtraForm
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentCode { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static long CurrentId { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool Tranfer = false;
        private readonly IKiotVietService _kiotVietService;
        private string _searchProductCode = "";
        private int _scannedBarcodeCount;
        private TransferResponse _transferResponse;
        private readonly IProductService _productService;
        private Dictionary<string, string> productLookupDictionary;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 60;
        public FrmTranferProcess(IKiotVietService kiotVietService, IProductService productService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            InitializeComponent();
            //ReloadData(CurrentCode, CurrentId);
            //txtTranferCode.Text = CurrentCode;
            StartCountdownTimer();
        }

        public void ReloadData(string tranferCode, long tranferId, bool _tranfer)
        {
            CurrentId = tranferId;
            CurrentCode = tranferCode;
            txtTranferCode.Text = tranferCode;
            Tranfer = _tranfer;
            _scannedBarcodeCount = 0;
            LoadData(tranferId);
        }

        private async void LoadProduct(List<TransferDetail> transferDetails)
        {
            try
            {
                var productCodes = transferDetails
                    .Select(p => p.ProductCode)
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Distinct()
                    .ToList();
                var productCodeBarCode = await _productService.SynAndGetProductCodeBarCode(productCodes);
                productLookupDictionary = new Dictionary<string, string>();
                foreach (var product in productCodeBarCode)
                {
                    if (!string.IsNullOrWhiteSpace(product.Code))
                    {
                        productLookupDictionary.TryAdd(product.Code, product.Code);
                    }

                    if (!string.IsNullOrWhiteSpace(product.BarCode))
                    {
                        productLookupDictionary.TryAdd(product.BarCode, product.Code);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error_);
            }
        }

        private async void LoadData(long tranferId)
        {
            try
            {
                SetControlEnable(false);
                Text = Tranfer ? "Xử lý Phiếu Chuyển hàng" : "Xử lý Phiếu Nhận hàng";
                grpCtlFilter.Text = Tranfer ? "Phiếu Chuyển hàng" : "Phiếu Nhận hàng";
                var url = $"https://public.kiotapi.com/transfers/{tranferId}";
                var (success, content) = await _kiotVietService.CallApiAsync(url, (string)null, "GET");
                if (!success && content == null) MessageBox.Show("Lỗi khi lấy dữ liệu Kiotviet");

                var transferResponse = JsonConvert.DeserializeObject<TransferResponse>(content);
                if (transferResponse == null) return;

                SetStatusCheckboxStyle();

                // Reset trạng thái
                chkFinish.Checked = false;
                chkCancel.Checked = false;
                chkDraft.Checked = false;

                switch ((OrderStatusEnum)transferResponse.Status)
                {
                    case OrderStatusEnum.Finished:
                        MessageHelper.MsgBox($"Phiếu: {transferResponse.Code} đã Nhập hàng", MsgType.Error_);
                        txtProductCode.ReadOnly = true;
                        break;
                    case OrderStatusEnum.Cancel:
                        MessageHelper.MsgBox($"Phiếu nhập hàng: {transferResponse.Code} đã Huỷ", MsgType.Error_);
                        txtProductCode.ReadOnly = true;
                        break;
                    case OrderStatusEnum.Draft:
                        txtProductCode.ReadOnly = false;
                        break;
                    default:
                        txtProductCode.ReadOnly = true;
                        break;
                }

                SetOrderStatusUI(transferResponse.Status);

                // Xử lý tên sản phẩm tách đơn vị
                foreach (var transfer in transferResponse.Details)
                {
                    var start = transfer.ProductName.LastIndexOf('(');
                    var end = transfer.ProductName.LastIndexOf(')');
                    if (start == -1 || end <= start) continue;
                    transfer.Unit = transfer.ProductName.Substring(start + 1, end - start - 1).Trim();
                    transfer.ProductName = transfer.ProductName[..start].Trim();
                }

                txtCustomerName.Text = transferResponse.FromBranchName; // Tên Người nhập
                txtSaleName.Text = transferResponse.ToBranchName; // Tên nhà cung cấp.
                txtPurchaseDate.Text = transferResponse.DispatchedDate?.ToString("dd/MM/yyyy HH:mm:ss");
                _transferResponse = transferResponse;
                txtScanNumber.ReadOnly = true;
                txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" +
                                     transferResponse.Details.Count().ToString();
                
                gridControlOrder.DataSource = _transferResponse.Details;
                gridViewOrder.BestFitColumns();
                LoadProduct(_transferResponse.Details);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu", MsgType.Error_);
            }
            finally
            {
                layoutControlTop.Enabled = true;
                txtProductCode.Focus();
                gridControlOrder.Enabled = true;
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

        private void FrmTranferProcess_Load(object sender, EventArgs e)
        {
            SetTextEditHeight(this, 25);
            BeginInvoke(new Action(() => txtProductCode.Focus()));
            SetStatusCheckboxStyle();
            ReloadData(CurrentCode, CurrentId, Tranfer);
        }

        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            var searchBarcode = txtProductCode.Text.Trim();
            txtProductCode.SelectAll();
            _searchProductCode = searchBarcode;
            if (string.IsNullOrEmpty(searchBarcode)) return;
            var (isProductFound, productCode) = TryFindProductCode(searchBarcode);
            e.Handled = true;
            if (isProductFound)
            {
                var findProduct = _transferResponse.Details.FirstOrDefault(p => p.ProductCode == productCode);
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
                    txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" + _transferResponse.Details.Count().ToString();
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
            e.Appearance.BackColor = Color.LightGreen; // Màu xanh nhạt
            e.Appearance.ForeColor = Color.Black;      // Text màu đen (tuỳ chọn)
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            switch (_transferResponse.Status)
            {
                case (int)OrderStatusEnum.Finished:
                    MessageHelper.MsgBox("Đơn hàng đã hoàn thành, vui lòng kiểm tra lại", MsgType.Error_);
                    break;
                case (int)OrderStatusEnum.Cancel:
                    MessageHelper.MsgBox("Đơn hàng đã huỷ, vui lòng kiểm tra lại", MsgType.Error_);
                    break;
                default:
                    if (_scannedBarcodeCount == _transferResponse.Details.Count())
                    {
                        var confirm = MessageHelper.MsgBox("Hoàn thành đơn hàng", MsgType.YesNo);
                        if (confirm != DialogResult.Yes) return;
                        FinishOrder();
                    }
                    else
                    {
                        var listNotScan = _transferResponse.Details.Where(p => !p.Checked)
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

            var orderCode = txtTranferCode.Text.Trim();

            if (_scannedBarcodeCount > 0 && _transferResponse.Details.Any(p => p.Checked))
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
            return productLookupDictionary.TryGetValue(searchBarCode, out var codeValue) ? (true, codeValue) : (false, null);
        }

        private void FrmTranferProcess_Shown(object sender, EventArgs e)
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
            ReloadData(CurrentCode, CurrentId,Tranfer);
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
                    purchaseDate = DateTime.UtcNow,
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

                MessageHelper.MsgBox("Đơn hàng đã được hoàn thành thành công.", MsgType.Information);
                ReloadData(CurrentCode, CurrentId, Tranfer);
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
            gridControlOrder.Enabled = enable;
        }

        private void SetStatusCheckboxStyle()
        {
            chkFinish.BackColor = Color.LightGreen;
            chkDraft.BackColor = Color.Green;
            chkDraft.ForeColor = Color.White;
            chkCancel.BackColor = Color.OrangeRed;
            chkCancel.ForeColor = Color.White;
            chkTranfered.BackColor = Color.Cyan;
            chkTranfered.ForeColor = Color.Black;
        }

        private void SetOrderStatusUI(int status)
        {
            chkFinish.Checked = status == 3; // Trạng thái đã nhận
            chkCancel.Checked = status == 4; // Trạng thái đã huỷ
            chkDraft.Checked = status == 1; // Trạng thái tạm
            chkTranfered.Checked = status == 2; // Trạng thái đã chuyển
        }

    }

}