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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmAddPurchase : FrmBasePos, IReloadableForm
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
        private readonly ISupplyService _supplyService;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private Dictionary<string, string> _productLookupDictionary;
        private readonly FrmMainF _mainForm;
        private List<Product> _products;
        private List<PurchaseOrderDetail> _purchaseOrderDetails;

        #endregion

        public FrmAddPurchase(IKiotVietService kiotVietService, IProductService productService,
            ISystemService systemService, IBranchService branchService, ISupplyService supplyService, IMapper mapper,
            IPurchaseOrderService purchaseOrderService, FrmMainF mainForm) : base(branchService, systemService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            _supplyService = supplyService;
            _purchaseOrderService = purchaseOrderService;
            _mainForm = mainForm;
            InitializeComponent();
            txtOrderCode.Text = CurrentCode;
        }

        private void FrmPurchaseProcess_Load(object sender, EventArgs e)
        {
            SetTextEditHeight(this, 25);
            BeginInvoke(() => txtProductCode.Focus());
            InitForm();
            layoutCtlReload.Enabled = false;
            btnReload.Enabled = false;
            SetStatusCheckboxStyle(1);
        }

        private void FrmOrderProcess_Shown(object sender, EventArgs e)
        {
            txtProductCode.Focus();
        }
        public async Task ReLoadData(string code, long id)
        {
            try
            {
                CurrentCode = code;
                CurrentId = id;
                txtOrderCode.Text = code;
                _scannedBarcodeCount = 0;
                await LoadProduct();
                await LoadSupplier();
                await LoadDefaultSetting();
                await SyncAndGetProductCodeBarCode(null);
                Text = "Thêm Nhận hàng";
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,"Có lỗi trong quá trình lấy dữ liệu", MsgType.Error);
            }
        }

        private async Task LoadProduct()
        {
            try
            {
                _products = [];
                _products = await _productService.GetProductsByBranchId(_branchId);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error);
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
                MessageHelper.MsgBox(this,"Có lỗi trong quá trình lấy dữ liệu", MsgType.Error);
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

        private async Task SyncAndGetProductCodeBarCode(List<PurchaseOrderDetail> purchaseOrderDetails)
        {
            var productCodes = new List<string>();
            if (purchaseOrderDetails != null)
            {
                productCodes = purchaseOrderDetails
                    .Where(p => !string.IsNullOrWhiteSpace(p.ProductCode))
                    .Select(p => p.ProductCode)
                    .ToList();
            }
            
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
                    MessageHelper.MsgBox(this,"Không tìm thấy sản phẩm với mã vạch đã nhập.", MsgType.Error);
                    return;
                }

                // Kiểm tra sản phẩm đã tồn tại
                var existingItem = _purchaseOrderDetails
                    .FirstOrDefault(p => p.ProductCode == productCode);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                    existingItem.ScanCount++;
                    if (!existingItem.Checked) _scannedBarcodeCount++;
                    existingItem.Checked = true;
                }
                else
                {
                    if (_products == null || _products.Count == 0)
                    {
                        MessageHelper.MsgBox(this,"Danh sách sản phẩm rỗng.", MsgType.Error);
                        return;
                    }

                    var product = _products.FirstOrDefault(p => p.BarCode == searchBarcode) ??
                                  _products.FirstOrDefault(p => p.Code == searchBarcode);
                    if (product == null)
                    {
                        MessageHelper.MsgBox(this,"Không tìm thấy sản phẩm với mã vạch đã nhập.", MsgType.Error);
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
                        ScanCount = 1,
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
                MessageHelper.MsgBox(this,$"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error);
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
            var scanCount = Math.Max(0, transferredQuantity);
            e.Value = scanCount;
        }
        private void gridViewOrder_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (sender is not GridView view) return;

            if (view.GetRow(e.RowHandle) is not PurchaseOrderDetail row) return;

            if (!row.Checked) return;
            e.Appearance.BackColor = Color.LightGreen;
            e.Appearance.ForeColor = Color.Black;    
            e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
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
            existingItem.Quantity = (double)newValue;
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
        private void grdViewOrder_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (sender is not GridView view) return;
            if (view.FocusedColumn.FieldName != "Quantity") return;
            if (view.GetRow(view.FocusedRowHandle) is not TransferDetail row) return;
            if (row.Checked) return;
            e.Cancel = true;
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

        /// <summary>
        /// Nhân viên kho lên phiếu tạm, kế toán hoàn thành đơn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (_purchaseOrderDetails == null || _purchaseOrderDetails.Count == 0)
            {
                MessageHelper.MsgBox(this, "Chưa có sản phẩm nào trong phiếu nhập", MsgType.Error);
                return;
            }

            if (_scannedBarcodeCount != _purchaseOrderDetails.Count)
            {
                var listNotScan = _purchaseOrderDetails.Where(p => !p.Checked)
                    .Select(p => p.ProductCode)
                    .ToList();
                var message =
                    $"Còn {listNotScan.Count} sản phẩm chưa quét mã: {string.Join(", ", listNotScan)}.\nVui lòng thực hiện trước khi hoàn thành.";
                MessageHelper.MsgBox(this, message, MsgType.Error);
                txtProductCode.Focus();
                return;
            }

            if (lkpSupplier.EditValue == null)
            {
                MessageHelper.MsgBox(this, "Chưa chọn nhà cung cấp", MsgType.Error);
                lkpSupplier.Focus();
                return;
            }

            SaveDraftOrder(true);
        }

        private async void SaveDraftOrder(bool draft)
        {
            try
            {
                SetControlEnable(false);

                const string orderUrl = $"https://public.kiotapi.com/purchaseorders";
                var supplierId = Convert.ToInt64(lkpSupplier.EditValue);

                var supplier = await _supplyService.GetSupplierByCode(supplierId);
                if (supplier == null)
                {
                    MessageHelper.MsgBox(this,"Nhà cung cấp không tồn tại.", MsgType.Error);
                    return;
                }
                var purchaseRequest = new
                {
                    purchaseDate = DateTime.ParseExact(
                        txtPurchaseDate.Text.Trim(),
                        "dd/MM/yyyy HH:mm:ss",
                        CultureInfo.InvariantCulture),
                    branchId = BranchId,
                    supplier = new
                    {
                        code = supplier.Code,
                        name = supplier.Name,
                        contactNumber = supplier.ContactNumber,
                        address = supplier.Address,
                    },
                    description = AppGlobals.UserInfo.UserName + "/" + AppGlobals.ComputerName,
                    isDraft = draft? 1 : 3,
                    discount = 0,
                    discountRatio = 0,
                    paidAmount = 0,
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
                
                var (updateSuccess, updateContent) = await _kiotVietService.CallApiAsync(orderUrl, purchaseRequest, CurrentCode == "" ? "POST" : "PUT");
                var purchase = JsonConvert.DeserializeObject<PurchaseOrderResponse>(updateContent);
                if (!updateSuccess || string.IsNullOrEmpty(updateContent))
                {
                    MessageHelper.MsgBox(this,$"Có lỗi khi tạo đơn Nhập hàng: {updateContent}", MsgType.Error);
                    return;
                }

                MessageHelper.MsgBox(this, draft? "Thao tác tạo đơn Nhập hàng thành công." : "Thao tác Nhập hàng thành công", MsgType.Information);
                CurrentCode = purchase.Code;
                CurrentId = purchase.Id;
                await UpdateProductChecked(purchase.Code, purchase.Id, _purchaseOrderDetails);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this,"Có lỗi trong quá trình xử lý đơn hàng.", MsgType.Error);
            }
            finally
            {
                SetControlEnable(true);
                await FormHelper.OpenFormWithScope<FrmProduct>(_mainForm, _mainForm.ServiceProvider, "", 0,
                    nameof(FrmProduct), WuserControl.FrmPurchase);
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
                    BranchId = BranchId,
                    ProductCode = orderDetail.ProductCode,
                    ProductBarCode = !string.IsNullOrEmpty(orderDetail.ProductBarCode) ? orderDetail.ProductBarCode : orderDetail.ProductCode,
                    UserName = AppGlobals.UserInfo.UserName,
                    Checked = true,
                    ScanCount = orderDetail.Quantity
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
            btnSaveDraft.Enabled = status == 1;
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
                        if (c.Name is nameof(btnSaveDraft)) break;
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

        private async void btnReload_Click(object sender, EventArgs e)
        {
            await ReLoadData(CurrentCode, CurrentId);
        }

    }

}