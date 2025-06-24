using Be.Common.Purchase_Order.Response;
using Be.Core.Entities;
using Be.Services.Catalog;
using Be.Services.KiotViet;
using Be.Services.System;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using FrmMain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using Be.Services.Pos;
using Be.Services.Supplier;
using FrmMain.App;
using Newtonsoft.Json;
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

        private async void LoadSupplier()
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

        private async void InitForm()
        {
            txtSaleName.Text = AppGlobals.UserInfo?.FullName ?? "Không xác định";
            txtPurchaseDate.Text = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss");
            txtTotal.Text = "0";
            txtDiscount.Text = "0";
            txtNeedPayment.Text = "0";
            txtTotalPayment.Text = "0";
            txtTotalItems.Text = "0";
            await LoadProduct();
            // Đếm tổng số sản phẩm
            txtProductCount.Text = "0";
            grdControlOrders.DataSource = _purchaseOrderDetails;
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

        private async void FrmPurchaseProcess_Load(object sender, EventArgs e)
        {
            try
            {
                SetTextEditHeight(this, 25);
                BeginInvoke(() => txtProductCode.Focus());
                SetStatusCheckboxStyle();
                //ReloadData(CurrentId);
                InitForm();
                LoadSupplier();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu" + ex, MsgType.Error_);
            }
        }

        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            var searchBarcode = txtProductCode.Text.Trim();
            txtProductCode.SelectAll();
            if (string.IsNullOrEmpty(searchBarcode)) return;

            var product = _products.FirstOrDefault(p => p.BarCode == searchBarcode);
            var productDetail = _mapper.Map<PurchaseOrderDetail>(product);
            if(product !=null) _purchaseOrderDetails.Add(productDetail);
            grdControlOrders.RefreshDataSource();
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

                var orderUrl = $"https://public.kiotapi.com/purchaseorders/{CurrentId}";
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, (string)null, "GET");

                if (!success || string.IsNullOrWhiteSpace(content))
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
       //{
       //    SetCheckboxColor(chkFinish, Color.LightGreen, Color.Black);
       //    SetCheckboxColor(chkDraft, Color.Green, Color.White);
       //    SetCheckboxColor(chkCancel, Color.OrangeRed, Color.White);           
           //txtOrderCode.BackColor = Color.White;
           //txtOrderCode.ForeColor = Color.OrangeRed;
        }
    }

}