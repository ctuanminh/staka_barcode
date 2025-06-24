using AutoMapper;
using Be.Common.Purchase_Order.Response;
using Be.Core.Entities;
using Be.Services.Catalog;
using Be.Services.KiotViet;
using Be.Services.Pos;
using Be.Services.Supplier;
using Be.Services.System;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraScheduler.Native;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
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
using DevExpress.CodeParser;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmAddPurchase: XtraForm
    {
        #region Ctor & Private Fields
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentCode { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static long CurrentId { get; set; }
        private int _branchId;
        private readonly IKiotVietService _kiotVietService;
        private int _scannedBarcodeCount;
        private PurchaseOrderResponse _orderResponse;
        private readonly IProductService _productService;
        private readonly ISystemService _systemService;
        private readonly IBranchService _branchService;
        private readonly ISupplyService _supplyService;
        private Dictionary<string, string> _productLookupDictionary;
        private List<Product> _products;
        private List<PurchaseOrderDetail> _purchaseOrderDetails;
        private readonly IMapper _mapper;
        #endregion
        public FrmAddPurchase(IKiotVietService kiotVietService, IProductService productService, ISystemService systemService, IBranchService branchService, ISupplyService supplyService, IMapper mapper)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            _systemService = systemService;
            _branchService = branchService;
            _supplyService = supplyService;
            _mapper = mapper;
            InitializeComponent();
            txtOrderCode.Text = CurrentCode;
        }

        private async void FrmPurchaseProcess_Load(object sender, EventArgs e)
        {
            try
            {
                SetTextEditHeight(this, 25);
                BeginInvoke(() => txtProductCode.Focus());
                SetStatusCheckboxStyle();
                InitForm();
                await LoadProduct();
                await LoadSupplier();
                await LoadDefaultSetting();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu" + ex, MsgType.Error_);
            }
        }

        public async void ReloadData(long purchaseId)
        {
            try
            {
                CurrentId = purchaseId;
                txtOrderCode.Text = CurrentCode;
                _scannedBarcodeCount = 0;
                await LoadData(purchaseId);
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
                SetStatusCheckboxStyle();
                
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
                grdControlOrders.DataSource = _orderResponse.PurchaseOrderDetails;
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
        }

        #region Gridview

        private void gridViewOrder_ShownEditor(object sender, CancelEventArgs e)
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

            if (view.GetRow(e.RowHandle) is not PurchaseOrderDetail row) return;

            if (!row.Checked) return;
            e.Appearance.BackColor = Color.LightGreen;
            e.Appearance.ForeColor = Color.Black;
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

        private async void txtOrderCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
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
                await LoadData(CurrentId);
                txtProductCode.Focus();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu", MsgType.Error_);
            }
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
                // Build orderRequest từ dữ liệu hiện tại
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

                var (updateSuccess, updateContent) = await _kiotVietService.CallApiAsync(orderUrl, purchaseRequest, "POST");
                var purchase = JsonConvert.DeserializeObject<PurchaseOrderResponse>(updateContent);
                if (!updateSuccess || string.IsNullOrEmpty(updateContent))
                {
                    MessageHelper.MsgBox($"Có lỗi khi tạo đơn Nhập hàng: {updateContent}", MsgType.Error_);
                    return;
                }

                MessageHelper.MsgBox("Tạo đơn Nhập hàng công.", MsgType.Information);
                InitForm();
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