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
using Be.Common.Order.Response;
using Be.Common.Purchase_Order.Response;
using Be.Services.Catalog;
using Be.Services.KiotViet;
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
        private string _searchProductCode = "";
        private int _scannedBarcodeCount;
        private PurchaseOrderResponse _orderResponse;
        private readonly IProductService _productService;
        private Dictionary<string, string> productLookupDictionary;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 60;
        public FrmPurchaseProcess(IKiotVietService kiotVietService, IProductService productService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            InitializeComponent();
            ReloadData(CurrentId);
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

        private async void LoadProduct(List<OrderDetailResponse> orderDetailResponses)
        {
            try
            {
                var productCodeBarCode = await _productService.SynAndGetProductCodeBarCode(orderDetailResponses);
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

        private async void LoadData(long purchaseId)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(LoadingForm), true, true);
                SplashScreenManager.Default.SetWaitFormCaption("Đang lấy Đơn Nhập hàng");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng đợi...");
                layoutControlTop.Enabled = false;
                gridControlOrder.Enabled = false;
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
                chkFinish.BackColor = Color.LightGreen;
                chkCancel.BackColor = Color.OrangeRed;
                chkCancel.ForeColor = Color.White;
                chkDraft.BackColor = Color.Green;
                chkDraft.BackColor = Color.Green;
                chkDraft.ForeColor = Color.White;
                
                // Reset trạng thái
                chkFinish.Checked = false;
                chkCancel.Checked = false;
                chkDraft.Checked = false;

                switch ((OrderStatusEnum)purchaseOrderResponse.Status)
                {
                    case OrderStatusEnum.Finished:
                        MessageHelper.MsgBox($"Phiếu nhập hàng: {purchaseOrderResponse.Code} đã Hoàn thành", MsgType.Error_);
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

                //txtCustomerName.Text = purchaseOrderResponse.CustomerName; // Tên khách hàng
                //txtSaleName.Text = orderApiResponse.SoldByName; // Tên người bán.
                //txtSumTotal.Text = NumberFormatter.FormatDecimal(orderApiResponse.Total); // Tổng hoá đơn
                //txtTotalPayment.Text = NumberFormatter.FormatDecimal(orderApiResponse.TotalPayment); // Khách đã trả
                //txtTotal.Text = NumberFormatter.FormatDecimal(orderApiResponse.Total); // Khách cần trả
                _orderResponse = purchaseOrderResponse;
                txtScanNumber.ReadOnly = true;
                txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" +
                                     purchaseOrderResponse.PurchaseOrderDetails.Count().ToString();
                
                gridControlOrder.DataSource = _orderResponse.PurchaseOrderDetails;
                gridViewOrder.BestFitColumns();
                //LoadProduct(_orderResponse.PurchaseOrderDetails);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu", MsgType.Error_);
            }
            finally
            {
                layoutControlTop.Enabled = true;
                SplashScreenManager.CloseForm();
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

        private void FrmOrderProcess_Load(object sender, EventArgs e)
        {
            SetTextEditHeight(this, 25);
            BeginInvoke(new Action(() => txtProductCode.Focus()));
            chkFinish.BackColor = Color.LightGreen;
            chkDraft.BackColor = Color.Green;
            chkDraft.ForeColor = Color.White;
            chkCancel.BackColor = Color.OrangeRed;
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
                var findProduct = _orderResponse.PurchaseOrderDetails.FirstOrDefault(p => p.ProductCode == productCode);
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

            if (view.GetRow(e.RowHandle) is not OrderDetailResponse row) return;

            if (!row.Checked) return;
            e.Appearance.BackColor = Color.LightGreen; // Màu xanh nhạt
            e.Appearance.ForeColor = Color.Black;      // Text màu đen (tuỳ chọn)
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (_orderResponse is not { Status: 1 })
            {
                MessageHelper.MsgBox("Vui lòng kiểm tra dữ liệu", MsgType.Error_);
                return;
            }
            if (_scannedBarcodeCount == _orderResponse.PurchaseOrderDetails.Count())
            {
                var result = MessageHelper.MsgBox("Hoàn thành đơn hàng", MsgType.YesNo);
                if (result != DialogResult.Yes) return;
                MessageHelper.MsgBox("Hoàn thành đơn hàng thành công", MsgType.Information);
                txtOrderCode.Focus();
            }
            else
            {
                var listNotScan = _orderResponse.PurchaseOrderDetails.Where(p => !p.Checked)
                    .Select(p => p.ProductCode)
                    .ToList();
                var message = $"Còn {listNotScan.Count} sản phẩm chưa quét mã: {string.Join(", ", listNotScan)}.\nVui lòng thực hiện trước khi hoàn thành.";
                MessageHelper.MsgBox(message, MsgType.Error_);
                txtProductCode.Focus();
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
            return productLookupDictionary.TryGetValue(searchBarCode, out var codeValue) ? (true, codeValue) : (false, null);
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
    }

}