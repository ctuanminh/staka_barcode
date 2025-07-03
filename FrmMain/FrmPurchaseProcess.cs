using Be.Common.PurchaseOrder.Dto;
using Be.Common.PurchaseOrder.Response;
using Be.Core.Entities;
using Be.Services.Catalog;
using Be.Services.KiotViet;
using Be.Services.Pos;
using Be.Services.System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using FrmMain.App;
using FrmMain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Services.PurchaseOrder;
using Color = System.Drawing.Color;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmPurchaseProcess : FrmBasePos, IReloadableForm
    {
        #region Ctor & Private Fields
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentCode { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static long CurrentId { get; set; }
        private readonly IKiotVietService _kiotVietService;
        private int _scannedBarcodeCount;
        private PurchaseOrderResponse _purchaseOrder;
        private readonly IProductService _productService;
        private readonly ISystemService _systemService;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private List<Product> _products;
        private readonly FrmMainF _mainForm;
        private Dictionary<string, string> _productLookupDictionary;
        #endregion

        public FrmPurchaseProcess(IKiotVietService kiotVietService, IProductService productService,
            ISystemService systemService, IBranchService branchService, IPurchaseOrderService purchaseOrderService, FrmMainF mainForm) : base(branchService, systemService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            _systemService = systemService;
            _purchaseOrderService = purchaseOrderService;
            _mainForm = mainForm;
            InitializeComponent();
        }

        public async Task ReLoadData(string code, long id)
        {
            try
            {
                CurrentId = id;
                CurrentCode = code;
                txtOrderCode.Text = CurrentCode;
                _scannedBarcodeCount = 0;
                if (IsHandleCreated && Visible)
                {
                    await LoadData(CurrentCode, CurrentId);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình lấy dữ liệu", MsgType.Error);
            }
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
                var productCodeBarCode = await _productService.SynAndGetProductCodeBarCode(productCodes, BranchId);
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
                MessageHelper.MsgBox(this, $"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error);
            }
        }

        private async Task LoadData(string code, long id)
        {
            try
            {
                SetControlEnable(false);
                var url = $"https://public.kiotapi.com/purchaseorders/{id}";
                var (success, content) = await _kiotVietService.CallApiAsync(url, (string)null, "GET");
                // Log the request
                await _systemService.AddRequest(new RequestEntity()
                {
                    Module = Name,
                    Url = url,
                    IsSuccess = success,
                    BranchId = BranchId
                });
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

                _purchaseOrder = purchaseOrderResponse;

                switch ((OrderStatusEnum)purchaseOrderResponse.Status)
                {
                    case OrderStatusEnum.Finished:
                        txtProductCode.ReadOnly = true;
                        chkFinish.Checked = true;
                        txtProductCode.ReadOnly = true;
                        break;
                    case OrderStatusEnum.Cancel:
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
                var isPurchaseCheckedAny = await _purchaseOrderService.IsPurchaseChecked(_purchaseOrder.Id, BranchId);
                if (isPurchaseCheckedAny)
                {
                    var result = MessageHelper.MsgBox(this, "Tải dữ liệu đã quét trước đó?", MsgType.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        var purchaseCheckedList =
                            await _purchaseOrderService.GetPurchaseCheckedByPurchaseId(_purchaseOrder.Id);
                        var productCheckedInPurchaseDict = new Dictionary<string, double>();
                        if (purchaseCheckedList.Any())
                        {
                            foreach (var checkedItem in purchaseCheckedList)
                            {
                                productCheckedInPurchaseDict.TryAdd(checkedItem.ProductCode, checkedItem.ScanCount);
                            }
                        }
                        foreach (var item in _purchaseOrder.PurchaseOrderDetails)
                        {
                            var productCheckedCount = productCheckedInPurchaseDict.GetValueOrDefault(item.ProductCode);
                            if (productCheckedCount <= 0) continue;
                            item.Checked = true;
                            item.ScanCount = productCheckedCount > item.Quantity ? item.Quantity : productCheckedCount;
                            _scannedBarcodeCount++;
                        }
                    }
                }
                txtScanNumber.ReadOnly = true;
                txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" +
                                     purchaseOrderResponse.PurchaseOrderDetails.Count();
                grdControlOrders.DataSource = _purchaseOrder.PurchaseOrderDetails;
                grdViewOrder.BestFitColumns();
                LoadProduct(_purchaseOrder.PurchaseOrderDetails);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình lấy dữ liệu", MsgType.Error);
            }
            finally
            {
                txtProductCode.Focus();
                SetControlEnable(true);
            }
        }

        private async Task LoadProduct()
        {
            try
            {
                _products = [];
                _products = await _productService.GetProducts(BranchId);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, $"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error);
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
                        if (c.Name is nameof(btnDraf)) break;
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

        private async void FrmPurchaseProcess_Load(object sender, EventArgs e)
        {
            try
            {
                SetTextEditHeight(this, 25);
                BeginInvoke(() => txtProductCode.Focus());
                SetStatusCheckboxStyle();
                await LoadDefaultSetting();
                await LoadProduct();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình lấy dữ liệu", MsgType.Error);
            }
        }

        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            var searchBarcode = txtProductCode.Text.Trim();
            txtProductCode.SelectAll();
            if (string.IsNullOrEmpty(searchBarcode)) return;

            //Kiểm tra xem product đã có trong đơn đặt chưa?
            //nếu có và scanned thì tăng số lượng.
            //nếu chưa thì thêm và check, sluong = 1;
            var existingProduct =
                _purchaseOrder.PurchaseOrderDetails.FirstOrDefault(p => p.ProductBarCode == searchBarcode) ??
                _purchaseOrder.PurchaseOrderDetails.FirstOrDefault(p => p.ProductCode == searchBarcode);
            if (existingProduct == null)
            {
                var product = _products.FirstOrDefault(p => p.BarCode == searchBarcode) ??
                              _products.FirstOrDefault(p => p.Code == searchBarcode);
                if (product != null)
                {
                    _purchaseOrder.PurchaseOrderDetails.Add(new PurchaseOrderDetail
                    {
                        ProductId = product.Id,
                        ProductCode = product.Code,
                        ProductBarCode = product.BarCode,
                        ProductName = product.Name,
                        Quantity = 1,
                        Price = product.RetailPrice,
                        Unit = product.Unit ?? "Cái",
                        Discount = 0,
                        DiscountRatio = 0,
                        Checked = true,
                        IsNew = true,
                        ScanCount = 1
                    });
                    _scannedBarcodeCount++;
                }
                else
                {
                    MessageHelper.MsgBox(this, "Không tìm thấy sản phẩm mã: " + searchBarcode, MsgType.Error);
                }
            }
            else
            {
                if (!existingProduct.Checked)
                {
                    existingProduct.Checked = true;
                    _scannedBarcodeCount++;
                }
                existingProduct.Quantity++;
                existingProduct.ScanCount++;
            }
            e.Handled = true;
            grdControlOrders.RefreshDataSource();
            var (isProductFound, productCode) = TryFindProductCode(searchBarcode);
            var rowHandle = grdViewOrder.LocateByValue("ProductCode", productCode);
            if (rowHandle < 0) return;
            grdViewOrder.FocusedRowHandle = rowHandle;
            grdViewOrder.MakeRowVisible(rowHandle);
            txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" + _purchaseOrder.PurchaseOrderDetails.Count().ToString();

            //if (isProductFound)
            //{
            //    var findProduct = _purchaseOrder.PurchaseOrderDetails.FirstOrDefault(p => p.ProductCode == productCode);
            //    if (findProduct != null)
            //    {
            //        if (findProduct.Checked) return;
            //        _scannedBarcodeCount++;
            //        findProduct.Checked = true;
            //    }
            //    else
            //    {
            //        MessageHelper.MsgBox(this, "Không tìm thấy sản phẩm mã: " + searchBarcode + " trong đơn hàng", MsgType.Error);
            //    }
            //}
            //else
            //{
            //    MessageHelper.MsgBox(this, "Không tìm thấy sản phẩm mã: " + searchBarcode, MsgType.Error);
            //}
        }
        private void gridViewOrder_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (sender is not GridView view) return;

            if (view.GetRow(e.RowHandle) is not PurchaseOrderDetail row) return;

            if (!row.Checked) return;
            if (row.IsNew)
            {
                e.Appearance.BackColor = Color.LightCoral; // Màu đỏ nhạt
                e.Appearance.ForeColor = Color.Black;
            }
            else
            {
                e.Appearance.BackColor = Color.LightGreen;
                e.Appearance.ForeColor = Color.Black;
            }
        }

        private void rpBtnDelete_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (grdViewOrder.FocusedRowHandle < 0) return;
            var productCode = grdViewOrder.GetRowCellValue(grdViewOrder.FocusedRowHandle, "ProductCode")?.ToString();
            if (string.IsNullOrEmpty(productCode)) return;
            var existingItem = _purchaseOrder.PurchaseOrderDetails.FirstOrDefault(p => p.ProductCode == productCode);
            if (existingItem == null) return;
            {
                _purchaseOrder.PurchaseOrderDetails.Remove(existingItem);
                grdControlOrders.DataSource = null;
                grdControlOrders.DataSource = _purchaseOrder.PurchaseOrderDetails;
                grdViewOrder.BestFitColumns();
                if (existingItem.Checked) _scannedBarcodeCount--;
                txtProductCount.Text = _purchaseOrder.PurchaseOrderDetails.Sum(p => p.Quantity).ToString();
                txtTotalItems.Text = _purchaseOrder.PurchaseOrderDetails.Count.ToString();
                txtScanNumber.Text = $"{_scannedBarcodeCount}/{_purchaseOrder.PurchaseOrderDetails.Count}";
            }
        }

        private void grdViewOrder_MouseMove(object sender, MouseEventArgs e)
        {
            var view = sender as GridView;
            var hitInfo = view.CalcHitInfo(e.Location);

            if (hitInfo.InRowCell && hitInfo.Column.FieldName == "Delete")
            {
                grdControlOrders.Cursor = Cursors.Hand;
            }
            else
            {
                grdControlOrders.Cursor = Cursors.Default;
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            switch (_purchaseOrder.Status)
            {
                case (int)OrderStatusEnum.Finished:
                    MessageHelper.MsgBox(this, "Đơn Nhập hàng đã hoàn thành, vui lòng kiểm tra lại", MsgType.Error);
                    break;
                case (int)OrderStatusEnum.Cancel:
                    MessageHelper.MsgBox(this, "Đơn Nhập hàng đã huỷ, vui lòng kiểm tra lại", MsgType.Error);
                    break;
                default:
                    if (_scannedBarcodeCount == _purchaseOrder.PurchaseOrderDetails.Count())
                    {
                        var confirm = MessageHelper.MsgBox(this, "Chắc chắn hoàn thành đơn Nhập hàng", MsgType.YesNo);
                        if (confirm != DialogResult.Yes) return;
                        FinishOrder();
                    }
                    else
                    {
                        var listNotScan = _purchaseOrder.PurchaseOrderDetails.Where(p => !p.Checked)
                            .Select(p => p.ProductCode)
                            .ToList();
                        var message =
                            $"Còn {listNotScan.Count} sản phẩm chưa quét mã: {string.Join(", ", listNotScan)}.\nVui lòng thực hiện trước khi hoàn thành.";
                        MessageHelper.MsgBox(this, message, MsgType.Error);
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

                if (_scannedBarcodeCount > 0 && _purchaseOrder.PurchaseOrderDetails.Any(p => p.Checked))
                {
                    var result = MessageHelper.MsgBox(this, "Bạn chắc chắn tải lại dữ liệu", MsgType.YesNo);
                    if (result != DialogResult.Yes) return;
                }

                if (string.IsNullOrEmpty(orderCode)) return;
                _scannedBarcodeCount = 0;
                await LoadData(CurrentCode, CurrentId);
                txtProductCode.Focus();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình lấy dữ liệu", MsgType.Error);
            }
        }

        private (bool check, string code) TryFindProductCode(string searchBarCode)
        {
            return _productLookupDictionary.TryGetValue(searchBarCode, out var codeValue) ? (true, codeValue) : (false, null);
        }

        private async void FrmPurchaseOrderProcess_Shown(object sender, EventArgs e)
        {
            try
            {
                await LoadData(CurrentCode, CurrentId);
                txtProductCode.Focus();
                grdViewOrder.MouseMove += grdViewOrder_MouseMove;
                rpAction.ButtonClick += rpBtnDelete_ButtonClick;
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình thực hiện.", MsgType.Error);
            }
        }

        private async void btnReloadOrder_Click(object sender, EventArgs e)
        {
            await ReLoadData(CurrentCode, CurrentId);
        }

        private async void FinishOrder()
        {
            try
            {
                SetControlEnable(false);

                var orderUrl = $"https://public.kiotapi.com/purchaseorders/{CurrentId}";
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, (string)null, "GET");

                if (!success || string.IsNullOrWhiteSpace(content))
                {
                    MessageHelper.MsgBox(this, "Lỗi khi lấy dữ liệu Kiotviet", MsgType.Error);
                    return;
                }

                var purchaseOrderResponse = JsonConvert.DeserializeObject<PurchaseOrderResponse>(content);
                if (purchaseOrderResponse == null)
                {
                    MessageHelper.MsgBox(this, "Dữ liệu đơn hàng trả về không hợp lệ", MsgType.Error);
                    return;
                }

                switch ((OrderStatusEnum)purchaseOrderResponse.Status)
                {
                    case OrderStatusEnum.Finished:
                        MessageHelper.MsgBox(this, $"Đơn hàng: {CurrentCode} đã Hoàn thành", MsgType.Information);
                        return;

                    case OrderStatusEnum.Cancel:
                        MessageHelper.MsgBox(this, $"Đơn hàng: {CurrentCode} đã Huỷ", MsgType.Information);
                        return;

                    case OrderStatusEnum.Draft:
                        break;

                    default:
                        MessageHelper.MsgBox(this, $"Trạng thái đơn hàng không hợp lệ", MsgType.Error);
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
                    MessageHelper.MsgBox(this, $"Có lỗi khi cập nhật đơn hàng: {updateContent}", MsgType.Error);
                    return;
                }

                MessageHelper.MsgBox(this, "Lưu phiếu nhập hàng thành công.", MsgType.Information);
                await ReLoadData(CurrentCode, CurrentId);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình xử lý đơn hàng.", MsgType.Error);
            }
            finally
            {
                SetControlEnable(true);
            }
        }
        private async void btnDraft_Click(object sender, EventArgs e)
        {
            try
            {
                if (_purchaseOrder.PurchaseOrderDetails.Count <= 0)
                {
                    MessageHelper.MsgBox(this, "Không có sản phẩm nào trong đơn hàng", MsgType.Error);
                    return;
                }

                if (_scannedBarcodeCount < _purchaseOrder.PurchaseOrderDetails.Count)
                {
                    MessageHelper.MsgBox(this, "Chưa quét đủ sản phẩm, vui lòng kiểm tra quét mã.", MsgType.Error);
                    return;
                }
                SetControlEnable(false);

                var orderUrl = $"https://public.kiotapi.com/purchaseorders/{CurrentId}";
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, (string)null, "GET");

                if (!success || string.IsNullOrWhiteSpace(content))
                {
                    MessageHelper.MsgBox(this, "Lỗi khi lấy dữ liệu Kiotviet", MsgType.Error);
                    return;
                }

                var purchaseOrderResponse = JsonConvert.DeserializeObject<PurchaseOrderResponse>(content);
                if (purchaseOrderResponse == null)
                {
                    MessageHelper.MsgBox(this, "Dữ liệu đơn hàng trả về không hợp lệ", MsgType.Error);
                    return;
                }

                switch ((OrderStatusEnum)purchaseOrderResponse.Status)
                {
                    case OrderStatusEnum.Finished:
                        MessageHelper.MsgBox(this, $"Phiếu nhập: {CurrentCode} đã Hoàn thành", MsgType.Information);
                        return;

                    case OrderStatusEnum.Cancel:
                        MessageHelper.MsgBox(this, $"Phiếu nhập: {CurrentCode} đã Huỷ", MsgType.Information);
                        return;

                    case OrderStatusEnum.Draft:
                        break;

                    default:
                        MessageHelper.MsgBox(this, $"Trạng thái Phiếu nhập không hợp lệ", MsgType.Error);
                        return;
                }

                // Build purchaseOrderRequest từ dữ liệu hiện tại
                var purchaseRequest = new
                {
                    purchaseDate = _purchaseOrder.PurchaseDate,
                    branchId = (int)_purchaseOrder.BranchId,
                    supplier = new
                    {
                        code = _purchaseOrder.SupplierCode,
                        name = _purchaseOrder.SupplierName,
                    },
                    description = _purchaseOrder.Description,
                    isDraft = 1,

                    discount = purchaseOrderResponse.Discount,
                    discountRatio = 0,
                    paidAmount = 0,
                    surcharges = new List<object>(),
                    totalPayment = purchaseOrderResponse.TotalPayment,
                    makeInvoice = true,
                    purchaseOrderDetails = _purchaseOrder.PurchaseOrderDetails.Select(product => new PurchaseOrderDetail
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

                var (updateSuccess, updateContent) = await _kiotVietService.CallApiAsync(orderUrl, purchaseRequest, "PUT");

                if (!updateSuccess || string.IsNullOrEmpty(updateContent))
                {
                    MessageHelper.MsgBox(this, $"Có lỗi khi cập nhật đơn hàng: {updateContent}", MsgType.Error);
                    return;
                }

                MessageHelper.MsgBox(this, "Lưu phiếu nhập hàng thành công.", MsgType.Information);
                // Cập nhật tình trạng đã quét nếu là phiếu sửa
                await UpdateProductChecked(
                    _purchaseOrder.Code,
                    _purchaseOrder.Id,
                    _purchaseOrder.PurchaseOrderDetails
                );
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình xử lý đơn hàng.", MsgType.Error);
            }
            finally
            {
                SetControlEnable(true);
                await FormHelper.OpenFormWithScope<FrmPurchase>(_mainForm, _mainForm.ServiceProvider, "", 0,
                    nameof(FrmPurchase), WuserControl.FrmPurchase);
                Close();
            }
        }

        private async Task UpdateProductChecked(string purchaseCode, long purchaseId, List<PurchaseOrderDetail> purchaseOrderDetail)
        {
            foreach (var orderDetail in purchaseOrderDetail)
            {
                if (orderDetail.Checked == false) continue;
                var barcode = orderDetail.ProductBarCode ?? orderDetail.ProductCode;
                var productChecked = await
                    _purchaseOrderService.GetPurchaseCheckedByProduct(purchaseId, barcode,
                        BranchId) ?? await _purchaseOrderService.GetPurchaseCheckedByProduct(purchaseId, orderDetail.ProductBarCode,
                    BranchId);
                if (productChecked == null)
                {
                    var purchaseCheckedDto = new PurchaseCheckedDto()
                    {
                        PurchaseId = purchaseId,
                        PurchaseCode = purchaseCode,
                        BranchId = BranchId,
                        ProductCode = orderDetail.ProductCode,
                        ProductBarCode = !string.IsNullOrEmpty(orderDetail.ProductBarCode) ? orderDetail.ProductBarCode : orderDetail.ProductCode,
                        UserName = AppGlobals.UserInfo.UserName,
                        Checked = true,
                        ScanCount = orderDetail.ScanCount > 0 ? orderDetail.ScanCount : 1
                    };
                    await _purchaseOrderService.AddPurchaseChecked(purchaseCheckedDto);
                }
                else
                {
                    productChecked.ScanCount = orderDetail.ScanCount;
                    await _purchaseOrderService.UpdatePurchaseChecked(productChecked.Id, productChecked);
                }
                
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