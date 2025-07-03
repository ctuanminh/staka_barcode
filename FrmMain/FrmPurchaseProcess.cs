using Be.Common.PurchaseOrder.Dto;
using Be.Common.PurchaseOrder.Response;
using Be.Common.Tranfer.Response;
using Be.Core.Entities;
using Be.Services.Catalog;
using Be.Services.KiotViet;
using Be.Services.Pos;
using Be.Services.PurchaseOrder;
using Be.Services.System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
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
        private readonly IPurchaseOrderService _purchaseOrderService;
        private List<Product> _products;
        private readonly FrmMainF _mainForm;
        private Dictionary<string, string> _productCodeBarCodeDic;
        #endregion

        public FrmPurchaseProcess(IKiotVietService kiotVietService, IProductService productService,
            ISystemService systemService, IBranchService branchService, IPurchaseOrderService purchaseOrderService, FrmMainF mainForm) : base(branchService, systemService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
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

        /// <summary>
        /// Tạo Dic chứa ProductCode và BarCode
        /// </summary>
        /// <param name="purchaseOrderDetails"></param>
        private async void LoadProductCodeBarCode(List<PurchaseOrderDetail> purchaseOrderDetails)
        {
            try
            {
                var productCodes = purchaseOrderDetails
                    .Select(p => p.ProductCode)
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Distinct()
                    .ToList();
                var productCodeBarCode = await _productService.SynAndGetProductCodeBarCode(productCodes, BranchId);
                _productCodeBarCodeDic = new Dictionary<string, string>();
                foreach (var product in productCodeBarCode)
                {
                    if (!string.IsNullOrWhiteSpace(product.Code))
                    {
                        _productCodeBarCodeDic.TryAdd(product.Code, product.Code);
                    }

                    if (!string.IsNullOrWhiteSpace(product.BarCode))
                    {
                        _productCodeBarCodeDic.TryAdd(product.BarCode, product.Code);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, $"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error);
            }
        }

        /// <summary>
        /// Load dữ liệu chung cho Form Purchase Process
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task LoadData(string code, long id)
        {
            try
            {
                SetControlEnable(false);
                var purchaseOrderResponse = await GetPurchaseOrderFromApi(id);
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
                LoadProductCodeBarCode(_purchaseOrder.PurchaseOrderDetails);
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

        /// <summary>
        /// Call Api lấy dữ liệu PurchaseOrder từ KiotViet
        /// </summary>
        /// <param name="purchaseId"></param>
        /// <returns></returns>
        private async Task<PurchaseOrderResponse> GetPurchaseOrderFromApi(long purchaseId)
        {
            var purchaseUrl = $"https://public.kiotapi.com/purchaseorders/{purchaseId}";
            var (success, content) = await _kiotVietService.CallApiAsync(purchaseUrl, (string)null, "GET");

            if (!success || string.IsNullOrWhiteSpace(content))
            {
                MessageHelper.MsgBox(this, "Lỗi khi lấy dữ liệu Kiotviet", MsgType.Error);
                return null;
            }

            var purchaseOrderResponse = JsonConvert.DeserializeObject<PurchaseOrderResponse>(content);
            if (purchaseOrderResponse != null) return purchaseOrderResponse;
            MessageHelper.MsgBox(this, "Dữ liệu phiếu nhập hàng trả về không hợp lệ", MsgType.Error);
            return null;
        }

        /// <summary>
        /// Check thông tin hợp lệ trước khi lưu tạm
        /// </summary>
        /// <param name="status"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool IsValidDraftStatus(int status, string code)
        {
            var orderStatus = (OrderStatusEnum)status;
            switch (orderStatus)
            {
                case OrderStatusEnum.Finished:
                    MessageHelper.MsgBox(this, $"Phiếu nhập hàng: {code} đã Hoàn thành", MsgType.Information);
                    return false;

                case OrderStatusEnum.Cancel:
                    MessageHelper.MsgBox(this, $"Phiếu nhập hàng: {code} đã Huỷ", MsgType.Information);
                    return false;

                case OrderStatusEnum.Draft:
                    return true;

                default:
                    MessageHelper.MsgBox(this, "Trạng thái phiếu nhập không hợp lệ", MsgType.Error);
                    return false;
            }
        }

        /// <summary>
        /// Load Products để có Data tạo phiếu nhập mới.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Build PurchaseRequest từ PurchaseOrderResponse
        /// </summary>
        /// <param name="purchaseOrderResponse"></param>
        /// <returns></returns>
        private object BuildPurchaseRequest(PurchaseOrderResponse purchaseOrderResponse, PurchaseStatusEnum status)
        {
            return new
            {
                purchaseDate = purchaseOrderResponse.PurchaseDate,
                branchId = (int)purchaseOrderResponse.BranchId,
                supplier = new
                {
                    code = purchaseOrderResponse.SupplierCode,
                    name = purchaseOrderResponse.SupplierName,
                },
                description = purchaseOrderResponse.Description,
                isDraft = Convert.ToInt32(status),
                discount = purchaseOrderResponse.Discount,
                discountRatio = 0,
                paidAmount = 0,
                surcharges = new List<object>(),
                totalPayment = purchaseOrderResponse.TotalPayment,
                makeInvoice = true,
                purchaseOrderDetails = purchaseOrderResponse.PurchaseOrderDetails.Select(p => new
                {
                    p.ProductId,
                    p.ProductCode,
                    p.ProductName,
                    p.Quantity,
                    p.Price,
                    p.Discount,
                    p.DiscountRatio
                }).ToList()
            };
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

        private void gridViewOrder_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (sender is not GridView view) return;
            if (view.FocusedColumn.FieldName != "Quantity") return;

            var transferredQuantityObj = view.GetRowCellValue(view.FocusedRowHandle, "Quantity");
            if (transferredQuantityObj == null) return;

            if (!int.TryParse(transferredQuantityObj.ToString(), out var transferredQuantity)) return;

            if (transferredQuantity <= 0)
            {
                e.Value = 0;
            }
        }

        private void grdViewOrder_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (sender is not GridView view) return;
            if (view.FocusedColumn.FieldName != "Quantity") return;
            if (view.GetRow(view.FocusedRowHandle) is not TransferDetail row) return;
            if (row.Checked) return;
            e.Cancel = true;
        }

        private void grdViewOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            grdViewOrder.CloseEditor();
            grdViewOrder.UpdateCurrentRow();
            BeginInvoke(() =>
            {
                txtProductCode.Focus();
            });
        }

        private void grdViewOrder_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName != "Quantity") return;
            var newValue = grdViewOrder.GetRowCellValue(e.RowHandle, e.Column);
            var productCode = grdViewOrder.GetRowCellValue(e.RowHandle, "ProductCode");
            var productId = grdViewOrder.GetRowCellValue(e.RowHandle, "ProductId");

            var existingItem =
                _purchaseOrder.PurchaseOrderDetails
                .FirstOrDefault(p => p.ProductBarCode == productCode.ToString()) ??
                _purchaseOrder.PurchaseOrderDetails
                .FirstOrDefault(p => p.ProductId == Convert.ToInt64(productId));
            if (existingItem == null) return;
            existingItem.Quantity = (double)newValue;
            txtProductCount.Text = newValue.ToString();
        }
        private async void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                var isValid = await CheckBeforeChangeStatus();
                if (!isValid) return;
                SetControlEnable(false);
                var purchaseUrl = $"https://public.kiotapi.com/purchaseorders/{CurrentId}";
                var purchaseOrderResponse = await GetPurchaseOrderFromApi(CurrentId);
                // Build orderRequest từ dữ liệu hiện tại
                var purchaseRequest = BuildPurchaseRequest(purchaseOrderResponse, PurchaseStatusEnum.Finished);

                var (updateSuccess, updateContent) = await _kiotVietService.CallApiAsync(purchaseUrl, purchaseRequest, "PUT");

                if (!updateSuccess || string.IsNullOrEmpty(updateContent))
                {
                    MessageHelper.MsgBox(this, $"Có lỗi khi hoàn thành phiếu nhập hàng: {updateContent}", MsgType.Error);
                    return;
                }

                MessageHelper.MsgBox(this, "Hoàn thành Nhập hàng thành công.", MsgType.Information);
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
            return _productCodeBarCodeDic.TryGetValue(searchBarCode, out var codeValue) ? (true, codeValue) : (false, null);
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

        private async void btnDraft_Click(object sender, EventArgs e)
        {
            try
            {
                SetControlEnable(false);
                var isValid = await CheckBeforeChangeStatus();
                if (!isValid) return;
                var purchaseOrderResponse = await GetPurchaseOrderFromApi(CurrentId);

                // Build purchaseOrderRequest từ dữ liệu hiện tại
                var purchaseRequest = BuildPurchaseRequest(purchaseOrderResponse, PurchaseStatusEnum.Draft);
                var purchaseUrl = $"https://public.kiotapi.com/purchaseorders/{CurrentId}";
                var (updateSuccess, updateContent) = await _kiotVietService.CallApiAsync(purchaseUrl, purchaseRequest, "PUT");

                if (!updateSuccess || string.IsNullOrEmpty(updateContent))
                {
                    MessageHelper.MsgBox(this, $"Có lỗi khi cập nhật phiếu nhập: {updateContent}", MsgType.Error);
                    return;
                }

                MessageHelper.MsgBox(this, "Lưu phiếu nhập hàng thành công.", MsgType.Information);
                // Cập nhật tình trạng đã quét nếu là phiếu sửa
                await UpdateProductChecked(_purchaseOrder.Code, _purchaseOrder.Id, _purchaseOrder.PurchaseOrderDetails);
                await FormHelper.OpenFormWithScope<FrmPurchase>(_mainForm, _mainForm.ServiceProvider, "", 0,
                    nameof(FrmPurchase), WuserControl.FrmPurchase);
                Close();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình xử lý Phiếu nhập.", MsgType.Error);
            }
            finally
            {
                SetControlEnable(true);
            }
        }

        /// <summary>
        /// Kiểm tra purchase trước khi thay đổi trạng thái
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckBeforeChangeStatus()
        {
            if (_purchaseOrder.PurchaseOrderDetails.Count == 0)
            {
                MessageHelper.MsgBox(this, "Không có sản phẩm nào trong đơn hàng", MsgType.Error);
                return false;
            }

            var quantityMismatch = _purchaseOrder.PurchaseOrderDetails
                .Where(p => p.ScanCount != p.Quantity)
                .ToList();

            if (quantityMismatch.Any())
            {
                var message = $"Còn {quantityMismatch.Count} sản phẩm chưa đủ số lượng quét:\n" +
                              string.Join(", ", quantityMismatch.Select(p => $"{p.ProductCode} ({p.ScanCount}/{p.Quantity})"));
                MessageHelper.MsgBox(this, message, MsgType.Error);
                return false;
            }

            // Gọi API lấy trạng thái đơn hàng từ KiotViet
            var purchaseOrderResponse = await GetPurchaseOrderFromApi(CurrentId);
            if (purchaseOrderResponse == null)
            {
                MessageHelper.MsgBox(this, "Không thể lấy dữ liệu đơn hàng từ KiotViet.", MsgType.Error);
                return false;
            }

            // Kiểm tra trạng thái
            var orderStatus = (PurchaseStatusEnum)purchaseOrderResponse.Status;
            switch (orderStatus)
            {
                case PurchaseStatusEnum.Finished:
                    MessageHelper.MsgBox(this, $"Phiếu nhập: {CurrentCode} đã Hoàn thành", MsgType.Information);
                    return false;

                case PurchaseStatusEnum.Cancel:
                    MessageHelper.MsgBox(this, $"Phiếu nhập: {CurrentCode} đã Huỷ", MsgType.Information);
                    return false;

                case PurchaseStatusEnum.Draft:
                    return true; // Hợp lệ để tiếp tục xử lý

                default:
                    MessageHelper.MsgBox(this, "Trạng thái Phiếu nhập không hợp lệ", MsgType.Error);
                    return false;
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