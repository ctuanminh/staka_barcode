using AutoMapper;
using Be.Common.PurchaseOrder.Dto;
using Be.Common.PurchaseOrder.Response;
using Be.Common.Tranfer.Response;
using Be.Core.Entities;
using Be.Services.Catalog;
using Be.Services.KiotViet;
using Be.Services.Pos;
using Be.Services.PurchaseOrder;
using Be.Services.Supplier;
using Be.Services.System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using FrmMain.App;
using FrmMain.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmAddPurchase : XtraForm
    {
        #region Ctor & Private Fields
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentCode { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static long CurrentId { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool IsEditMode { get; set; } = false;
        private int _branchId;
        private readonly IKiotVietService _kiotVietService;
        private int _scannedBarcodeCount;
        private PurchaseOrderResponse _purchaseOrderResponse;
        private readonly IProductService _productService;
        private readonly ISystemService _systemService;
        private readonly IBranchService _branchService;
        private readonly ISupplyService _supplyService;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private Dictionary<string, string> _productLookupDictionary;
        private readonly FrmMainF _mainForm;
        private List<Product> _products;
        private List<PurchaseOrderDetail> _purchaseOrderDetails;
        private readonly IMapper _mapper;
        #endregion
        public FrmAddPurchase(IKiotVietService kiotVietService, IProductService productService, ISystemService systemService, IBranchService branchService, ISupplyService supplyService, IMapper mapper, IPurchaseOrderService purchaseOrderService, FrmMainF mainForm)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            _systemService = systemService;
            _branchService = branchService;
            _supplyService = supplyService;
            _mapper = mapper;
            _purchaseOrderService = purchaseOrderService;
            _mainForm = mainForm;
            InitializeComponent();
            txtOrderCode.Text = CurrentCode;
        }

        private async void FrmPurchaseProcess_Load(object sender, EventArgs e)
        {
            try
            {
                SetTextEditHeight(this, 25);
                BeginInvoke(() => txtProductCode.Focus());
                InitForm();
                await LoadProduct();
                await LoadSupplier();
                await LoadDefaultSetting();
                if (CurrentId > 0 && CurrentCode != null)
                {
                    ReloadData(CurrentId, CurrentCode);
                    btnReload.Text = "Tải lại dữ liệu";
                    btnReload.Enabled = true;
                    layoutCtlReload.Enabled = true;
                }
                else
                {
                    layoutCtlReload.Enabled = false;
                    btnReload.Enabled = false;
                    SetStatusCheckboxStyle(1);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu" + ex, MsgType.Error_);
            }
        }

        public async void ReloadData(long purchaseId, string purchaseCode)
        {
            try
            {
                CurrentCode = purchaseCode;
                CurrentId = purchaseId;
                IsEditMode = true;
                txtOrderCode.Text = purchaseCode;
                _scannedBarcodeCount = 0;
                await LoadData(CurrentId);
                Text = IsEditMode switch
                {
                    false => "Thêm Nhận hàng",
                    true => $"Sửa Nhận hàng: {CurrentCode}"
                };
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu", MsgType.Error_);
            }
        }

        private async Task LoadProduct()
        {
            try
            {
                _products = [];
                _products = await _productService.GetProducts(_branchId);
                //_productLookupDictionary = new Dictionary<string, string>();
                //foreach (var product in _products)
                //{
                //    if (product.Code != product.BarCode)
                //    {
                //        _productLookupDictionary.TryAdd(product.BarCode, product.Code);
                //    }
                //    else
                //    {
                //        _productLookupDictionary.TryAdd(product.Code, product.Code);
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error_);
            }
        }

        private async Task LoadSupplier()
        {
            try
            {
                var suppliers = await _supplyService.GetSuppliers();
                lkpSupplier.Properties.DataSource = suppliers;
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu", MsgType.Error_);
            }
        }

        private void InitForm()
        {
            txtProductCode.Text = "";
            txtSaleName.Text = AppGlobals.UserInfo?.FullName ?? "Không xác định";
            txtPurchaseDate.Text = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss");
            txtTotal.Text = "0";
            txtDiscount.Text = "0";
            txtNeedPayment.Text = "0";
            txtTotalPayment.Text = "0";
            txtTotalItems.Text = "0";
            txtProductCount.Text = "0";
            _purchaseOrderDetails = null;
            SetStyleGridView();
            grdControlOrders.DataSource = null;
            grdControlOrders.RefreshDataSource();
        }

        private async Task LoadData(long purchaseId)
        {
            try
            {
                SetControlEnable(false);
                var url = $"https://public.kiotapi.com/purchaseorders/{purchaseId}";
                var (success, content) = await _kiotVietService.CallApiAsync(url, (string)null, "GET");
                // Log the request
                await _systemService.AddRequest(new RequestEntity()
                {
                    Module = Name,
                    Url = url,
                    IsSuccess = success,
                    BranchId = _branchId
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
                _purchaseOrderResponse = purchaseOrderResponse;
                SetStatusCheckboxStyle(_purchaseOrderResponse.Status);

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
                if (purchaseOrderResponse.Id > 0) txtDescription.ReadOnly = true;
                lkpSupplier.EditValue = purchaseOrderResponse.SupplierId;

                var purchaseCheckedDic = new Dictionary<string, string>();
                var purchaseCheckedList = await _purchaseOrderService.GetPurchaseCheckedByPurchaseId(CurrentId);
                foreach (var item in purchaseCheckedList)
                {
                    if (item.ProductBarCode == item.ProductCode)
                    {
                        purchaseCheckedDic.TryAdd(item.ProductBarCode, item.ProductCode);
                    }
                    else
                    {
                        purchaseCheckedDic.TryAdd(item.ProductCode, item.ProductBarCode);
                    }
                }

                if (purchaseCheckedList.Any())
                {
                    foreach (var detail in purchaseOrderResponse.PurchaseOrderDetails)
                    {
                        if (detail.ProductBarCode != null)
                        {
                            if (!purchaseCheckedDic.TryGetValue(detail.ProductBarCode, out var code)) continue;
                        }
                        else
                        {
                            if (!purchaseCheckedDic.TryGetValue(detail.ProductCode, out var code)) continue;
                        }

                        detail.Checked = true;
                        _scannedBarcodeCount++;
                    }
                }
                txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" +
                                     purchaseOrderResponse.PurchaseOrderDetails.Count();
                _purchaseOrderDetails = purchaseOrderResponse.PurchaseOrderDetails;
                grdControlOrders.DataSource = _purchaseOrderDetails;
                grdViewOrder.BestFitColumns();
                txtOrderCode.Focus();
                SetStatusCheckboxStyle(_purchaseOrderResponse.Status);
                await SyncAndGetProductCodeBarCode(_purchaseOrderDetails);
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

        private async Task SyncAndGetProductCodeBarCode(List<PurchaseOrderDetail> purchaseOrderDetails)
        {
            var productCodes = purchaseOrderDetails
                .Where(p => !string.IsNullOrWhiteSpace(p.ProductCode))
                .Select(p => p.ProductCode)
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

        private async void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode != Keys.Enter) return;

                var searchBarcode = txtProductCode.Text.Trim();
                if (string.IsNullOrEmpty(searchBarcode)) return;

                txtProductCode.SelectAll();
                _purchaseOrderDetails ??= [];

                //Tìm xem product tương ứng barcode:
                var foundProductCode = _productLookupDictionary.TryGetValue(searchBarcode, out var productCode);
                if (!foundProductCode)
                {
                    MessageHelper.MsgBox("Không tìm thấy sản phẩm với mã vạch đã nhập.", MsgType.Error_);
                    return;
                }

                // Kiểm tra sản phẩm đã tồn tại
                var existingItem = _purchaseOrderDetails
                    .FirstOrDefault(p => p.ProductCode == productCode);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                    if (!existingItem.Checked) _scannedBarcodeCount++;
                    existingItem.Checked = true;
                }
                else
                {
                    if (_products == null || _products.Count == 0)
                    {
                        MessageHelper.MsgBox("Danh sách sản phẩm rỗng.", MsgType.Error_);
                        return;
                    }

                    var product = _products.FirstOrDefault(p => p.BarCode == searchBarcode);
                    if (product == null)
                    {
                        MessageHelper.MsgBox("Không tìm thấy sản phẩm với mã vạch đã nhập.", MsgType.Error_);
                        return;
                    }

                    _purchaseOrderDetails.Add(new PurchaseOrderDetail
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
                    });

                    _scannedBarcodeCount++;
                }

                // Cập nhật thống kê UI
                txtProductCount.Text = _purchaseOrderDetails.Sum(p => p.Quantity).ToString();
                txtTotalItems.Text = _purchaseOrderDetails.Count.ToString();
                txtScanNumber.Text = $"{_scannedBarcodeCount}/{_purchaseOrderDetails.Count}";

                grdControlOrders.DataSource = null;
                grdControlOrders.DataSource = _purchaseOrderDetails;

                // Cập nhật tình trạng đã quét nếu là phiếu sửa
                if (_purchaseOrderResponse?.Id > 0)
                {
                    await UpdateProductChecked(
                        _purchaseOrderResponse.Code,
                        _purchaseOrderResponse.Id,
                        _purchaseOrderResponse.PurchaseOrderDetails
                    );
                }

                // Tìm dòng trong Grid
                var rowHandle = grdViewOrder.LocateByValue("ProductCode", searchBarcode);
                if (rowHandle >= 0)
                {
                    grdViewOrder.FocusedRowHandle = rowHandle;
                    grdViewOrder.MakeRowVisible(rowHandle);
                }

                txtProductCode.Focus();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error_);
            }
        }

        #region Gridview

        private void gridViewOrder_ShownEditor(object sender, EventArgs e)
        {
            var view = sender as GridView;
            view?.ActiveEditor?.Focus();
            view?.ActiveEditor?.SelectAll();
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
                e.Appearance.BackColor = Color.LightGreen; // Màu xanh nhạt
                e.Appearance.ForeColor = Color.Black;      // Text màu đen (tuỳ chọn)
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
        }

        private void SetStyleGridView()
        {
            grdViewOrder.Appearance.Row.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            grdViewOrder.Appearance.HeaderPanel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            grdViewOrder.Appearance.FocusedRow.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            grdViewOrder.Appearance.FocusedCell.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            grdViewOrder.Appearance.GroupRow.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        }

        private void grdViewOrder_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName != "Quantity") return;
            var newValue = grdViewOrder.GetRowCellValue(e.RowHandle, e.Column);
            var productCode = grdViewOrder.GetRowCellValue(e.RowHandle, "ProductCode");
            var productId = grdViewOrder.GetRowCellValue(e.RowHandle, "ProductId");

            var existingItem = _purchaseOrderDetails
                .FirstOrDefault(p => p.ProductBarCode == productCode.ToString()) ?? _purchaseOrderDetails
                .FirstOrDefault(p => p.ProductId == Convert.ToInt64(productId));
            if (existingItem == null) return;
            existingItem.Quantity = (int)newValue;
            txtProductCount.Text = newValue.ToString();
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

        private void rpBtnDelete_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (grdViewOrder.FocusedRowHandle < 0) return;
            var productCode = grdViewOrder.GetRowCellValue(grdViewOrder.FocusedRowHandle, "ProductCode")?.ToString();
            if (string.IsNullOrEmpty(productCode)) return;
            var existingItem = _purchaseOrderDetails.FirstOrDefault(p => p.ProductCode == productCode);
            if (existingItem == null) return;
            {
                _purchaseOrderDetails.Remove(existingItem);
                grdControlOrders.DataSource = null;
                grdControlOrders.DataSource = _purchaseOrderDetails;
                grdViewOrder.BestFitColumns();
                if (existingItem.Checked) _scannedBarcodeCount--;
                txtProductCount.Text = _purchaseOrderDetails.Sum(p => p.Quantity).ToString();
                txtTotalItems.Text = _purchaseOrderDetails.Count.ToString();
                txtScanNumber.Text = $"{_scannedBarcodeCount}/{_purchaseOrderDetails.Count}";
            }
        }

        #endregion

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (_scannedBarcodeCount != _purchaseOrderDetails.Count)
            {
                var listNotScan = _purchaseOrderDetails.Where(p => !p.Checked)
                    .Select(p => p.ProductCode)
                    .ToList();
                var message =
                    $"Còn {listNotScan.Count} sản phẩm chưa quét mã: {string.Join(", ", listNotScan)}.\nVui lòng thực hiện trước khi hoàn thành.";
                MessageHelper.MsgBox(message, MsgType.Error_);
                txtProductCode.Focus();
                return;
            }
            if (_purchaseOrderDetails == null || _purchaseOrderDetails.Count == 0)
            {
                MessageHelper.MsgBox("Chưa có sản phẩm nào trong phiếu nhập", MsgType.Error_);
                return;
            }

            if (lkpSupplier.EditValue == null)
            {
                MessageHelper.MsgBox("Chưa chọn nhà cung cấp", MsgType.Error_);
                lkpSupplier.Focus();
                return;
            }
            FinishOrder();

        }

        private void FrmOrderProcess_Shown(object sender, EventArgs e)
        {
            txtProductCode.Focus();
        }

        private async void FinishOrder()
        {
            try
            {
                SetControlEnable(false);

                var orderUrl = $"https://public.kiotapi.com/purchaseorders";
                var supplierId = Convert.ToInt64(lkpSupplier.EditValue);

                var supplier = await _supplyService.GetSupplierByCode(supplierId);
                if (supplier == null)
                {
                    MessageHelper.MsgBox("Nhà cung cấp không tồn tại.", MsgType.Error_);
                    return;
                }
                var purchaseRequest = new
                {
                    purchaseDate = DateTime.ParseExact(
                        txtPurchaseDate.Text.Trim(),
                        "dd/MM/yyyy HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    branchId = _branchId,
                    supplier = new
                    {
                        code = supplier.Code,
                        name = supplier.Name,
                        contactNumber = supplier.ContactNumber,
                        address = supplier.Address,
                    },
                    description = AppGlobals.UserInfo.UserName + "/" + AppGlobals.ComputerName,
                    isDraft = 1,
                    discount = 0,
                    discountRatio = 0,
                    paidAmount = 0,
                    //paymentMethod = null,
                    surcharges = new List<object>(),
                    totalPayment = 0,
                    makeInvoice = true,
                    purchaseOrderDetails = _purchaseOrderDetails.Select(product => new PurchaseOrderDetail
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
                if (CurrentCode != null)
                {
                    orderUrl = orderUrl + '/' + CurrentId;
                }
                var (updateSuccess, updateContent) = await _kiotVietService.CallApiAsync(orderUrl, purchaseRequest, CurrentCode == null ? "POST" : "PUT");
                var purchase = JsonConvert.DeserializeObject<PurchaseOrderResponse>(updateContent);
                if (!updateSuccess || string.IsNullOrEmpty(updateContent))
                {
                    MessageHelper.MsgBox($"Có lỗi khi tạo đơn Nhập hàng: {updateContent}", MsgType.Error_);
                    return;
                }

                MessageHelper.MsgBox(
                    CurrentCode != null ? "Lưu phiếu nhập hàng thành công." : "Tạo đơn Nhập hàng công.",
                    MsgType.Information);

                IsEditMode = true;
                CurrentCode = purchase.Code;
                CurrentId = purchase.Id;
                ReloadData(CurrentId, CurrentCode);
                await UpdateProductChecked(purchase.Code, purchase.Id, _purchaseOrderDetails);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình xử lý đơn hàng.", MsgType.Error_);
            }
            finally
            {
                SetControlEnable(true);
                if (FormHelper.OpenedForm(nameof(FrmPurchase), WuserControl.FrmPurchase, out var openForm))
                {
                    if (openForm is FrmPurchase processForm)
                    {
                        processForm.ReloadData();
                        openForm.Focus();
                    }
                }
                else
                {
                    var frmPurchase = _mainForm.ServiceProvider.GetRequiredService<FrmPurchase>();
                    FormHelper.NewFormNew(_mainForm, frmPurchase, WuserControl.FrmPurchase);
                }
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
                        _branchId) ?? await _purchaseOrderService.GetPurchaseCheckedByProduct(purchaseId, orderDetail.ProductBarCode,
                    _branchId);
                if (productChecked != null) continue;
                var purchaseCheckedDto = new PurchaseCheckedDto()
                {
                    PurchaseId = purchaseId,
                    PurchaseCode = purchaseCode,
                    BranchId = _branchId,
                    ProductCode = orderDetail.ProductCode,
                    ProductBarCode = !string.IsNullOrEmpty(orderDetail.ProductBarCode) ? orderDetail.ProductBarCode : orderDetail.ProductCode,
                    UserName = AppGlobals.UserInfo.UserName,
                    Checked = true
                };
                await _purchaseOrderService.AddPurchaseChecked(purchaseCheckedDto);
            }
        }

        private void SetControlEnable(bool enable)
        {
            layoutControlTop.Enabled = enable;
            grdControlOrders.Enabled = enable;
        }
        private void SetStatusCheckboxStyle(int status)
        {
            chkStatus.Checked = true;
            txtProductCode.ReadOnly = status != 1;
            txtProductCode.BackColor = Color.White;
            btnFinish.Enabled = status == 1;
            clmQuantity.OptionsColumn.AllowEdit = status == 1;
            switch (status)
            {
                case 1:
                    chkStatus.Text = "Nhập hàng";
                    SetCheckboxColor(chkStatus, Color.Green, Color.White);
                    break;
                case 3:
                    chkStatus.Text = "KT xác nhận";
                    SetCheckboxColor(chkStatus, Color.LightGreen, Color.Black);
                    break;

                default:
                    chkStatus.Text = "Đã huỷ";
                    SetCheckboxColor(chkStatus, Color.OrangeRed, Color.White);
                    break;
            }
        }
        private static void SetCheckboxColor(CheckEdit checkEdit, Color backColor, Color foreColor)
        {
            checkEdit.BackColor = backColor;
            checkEdit.ForeColor = foreColor;
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
        private (bool check, string code) TryFindProductCode(string searchBarCode)
        {
            return _productLookupDictionary.TryGetValue(searchBarCode.ToUpper(), out var codeValue) ? (true, codeValue) : (false, null);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            ReloadData(CurrentId, CurrentCode);
        }
    }

}