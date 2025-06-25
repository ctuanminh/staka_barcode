using AutoMapper;
using Be.Common.PurchaseOrder.Response;
using Be.Core.Entities;
using Be.Services.Catalog;
using Be.Services.KiotViet;
using Be.Services.Pos;
using Be.Services.Supplier;
using Be.Services.System;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using FrmMain.App;
using FrmMain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Common.PurchaseOrder.Dto;
using Be.Services.PurchaseOrder;
using DevExpress.XtraGrid.Views.Base;
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
        private List<Product> _products;
        private List<PurchaseOrderDetail> _purchaseOrderDetails;
        private readonly IMapper _mapper;
        #endregion
        public FrmAddPurchase(IKiotVietService kiotVietService, IProductService productService, ISystemService systemService, IBranchService branchService, ISupplyService supplyService, IMapper mapper, IPurchaseOrderService purchaseOrderService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            _systemService = systemService;
            _branchService = branchService;
            _supplyService = supplyService;
            _mapper = mapper;
            _purchaseOrderService = purchaseOrderService;
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
               if(CurrentId > 0) ReloadData(CurrentId, CurrentCode);
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
                switch (IsEditMode)
                {
                    case false:
                        chkStatus.Text = "Nhập hàng";
                        Text = "Thêm Nhận hàng";
                        SetStatusCheckboxStyle(1);
                        break;
                    case true:
                        Text = $"Sửa Nhận hàng: {CurrentCode}";
                        await LoadData(CurrentId);
                        break;
                }
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
                lkpSupplier.EditValue = purchaseOrderResponse.SupplierId;

                var purchaseCheckedList = await _purchaseOrderService.GetPurchaseCheckedByPurchaseId(CurrentId);
                if (purchaseCheckedList != null && purchaseCheckedList.Any())
                {
                    var purchaseCheckedDic = purchaseCheckedList.ToDictionary(pc => pc.ProductBarCode, pc => pc.Checked);
                    foreach (var detail in purchaseOrderResponse.PurchaseOrderDetails)
                    {
                        if (detail.ProductBarCode != null)
                        {
                            if (purchaseCheckedDic.TryGetValue(detail.ProductBarCode, out var isChecked))
                            {
                                detail.Checked = isChecked;
                            }
                        }
                        else
                        {
                            if (purchaseCheckedDic.TryGetValue(detail.ProductCode, out var isChecked))
                            {
                                detail.Checked = isChecked;
                            }
                        }
                    }
                }
                _purchaseOrderDetails = purchaseOrderResponse.PurchaseOrderDetails;
                grdControlOrders.DataSource = _purchaseOrderDetails;
                grdViewOrder.BestFitColumns();
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

        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)

        {
            if (e.KeyCode != Keys.Enter) return;

            var searchBarcode = txtProductCode.Text.Trim();
            txtProductCode.SelectAll();

            if (string.IsNullOrEmpty(searchBarcode)) return;
            _purchaseOrderDetails ??= [];

            // Kiểm tra sản phẩm đã tồn tại trong danh sách mua
            var existingItem = _purchaseOrderDetails
                .FirstOrDefault(p => p.ProductBarCode == searchBarcode);

            if (existingItem != null)
            {
                existingItem.Quantity++;
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

                var newItem = new PurchaseOrderDetail
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
                    Checked = true
                };

                _purchaseOrderDetails.Add(newItem);
            }

            // Cập nhật lại thống kê
            txtProductCount.Text = _purchaseOrderDetails.Sum(p => p.Quantity).ToString();
            txtTotalItems.Text = _purchaseOrderDetails.Count.ToString();
            grdControlOrders.DataSource = null;
            grdControlOrders.DataSource = _purchaseOrderDetails;
            grdControlOrders.RefreshDataSource();
            grdViewOrder.BestFitColumns();
            var rowHandle = grdViewOrder.LocateByValue("ProductCode", searchBarcode);
            if (rowHandle < 0) return;
            grdViewOrder.FocusedRowHandle = rowHandle;
            grdViewOrder.MakeRowVisible(rowHandle);
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
            if (view.FocusedColumn.FieldName != "TransferredQuantity") return;

            var transferredQuantityObj = view.GetRowCellValue(view.FocusedRowHandle, "TransferredQuantity");
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
            if (e.RowHandle != view.FocusedRowHandle) return;
            e.Appearance.BackColor = Color.LightGreen;
            e.Appearance.ForeColor = Color.Black;
            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
        }

        private void SetStyleGridView()
        {
            grdViewOrder.Appearance.Row.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            grdViewOrder.Appearance.HeaderPanel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            grdViewOrder.Appearance.FocusedRow.Font = new Font("Tahoma",9F, FontStyle.Bold);
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
                .FirstOrDefault(p => p.ProductBarCode == productCode.ToString())?? _purchaseOrderDetails
                .FirstOrDefault(p => p.ProductId == Convert.ToInt64(productId));
            if (existingItem == null) return;
            existingItem.Quantity = (int)newValue;
            txtProductCount.Text = newValue.ToString();
        }

        #endregion

        private void btnFinish_Click(object sender, EventArgs e)
        {
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

        private (bool check, string code) TryFindProductCode(string searchBarCode)
        {
            return _productLookupDictionary.TryGetValue(searchBarCode, out var codeValue) ? (true, codeValue) : (false, null);
        }

        private void FrmOrderProcess_Shown(object sender, EventArgs e)
        {
            txtProductCode.Focus();
        }

        private void btnReloadOrder_Click(object sender, EventArgs e)
        {
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
                    description = txtDescription.Text,
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
                    orderUrl = orderUrl +'/' + CurrentId;
                }
                var (updateSuccess, updateContent) = await _kiotVietService.CallApiAsync(orderUrl, purchaseRequest, CurrentCode == null? "POST": "PUT");
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
                UpdateProductChecked(purchase);
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

        private async Task UpdateProductChecked(PurchaseOrderResponse purchase)
        {
            foreach (var orderDetail in purchase.PurchaseOrderDetails)
            {
                var productChecked = await 
                    _purchaseOrderService.GetPurchaseCheckedByProduct(purchase.Id, orderDetail.ProductBarCode,
                        _branchId)?? await _purchaseOrderService.GetPurchaseCheckedByProduct(purchase.Id, orderDetail.ProductBarCode,
                    _branchId);
                if (productChecked != null) continue;
                var purchaseCheckedDto = new PurchaseCheckedDto()
                {
                    PurchaseId = purchase.Id,
                    PurchaseCode = purchase.Code,
                    BranchId = _branchId,
                    ProductBarCode = !string.IsNullOrEmpty(orderDetail.ProductBarCode)? orderDetail.ProductBarCode : orderDetail.ProductCode,
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
    }

}