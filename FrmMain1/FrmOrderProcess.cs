using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using FrmMain.Dto.Response;
using FrmMain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Be.Services.Catalog;
using Be.Services.KiotViet;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmOrderProcess : XtraForm
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentCode { get; set; }
        private readonly IKiotVietService _kiotVietService;
        private string _searchProductCode = "";
        private int _scannedBarcodeCount;
        private OrderResponse _orderResponse;
        private readonly IProductService _productService;
        private Dictionary<string, string> productLookupDictionary;
        public FrmOrderProcess(IKiotVietService kiotVietService, IProductService productService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            InitializeComponent();
            ReloadData(CurrentCode);
            txtOrderCode.Text = CurrentCode;
        }

        public void ReloadData(string code)
        {
            CurrentCode = code;
            txtOrderCode.Text = code;
            _scannedBarcodeCount = 0;
            LoadData(code);
            LoadProduct();
        }

        private async void LoadProduct()
        {
            try
            {
                var productCodeBarCode = await _productService.GetProductCodeBarCode();
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

        private async void LoadData(string code)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(LoadingForm), true, true);
                SplashScreenManager.Default.SetWaitFormCaption("Đang lấy Đơn hàng");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng đợi...");
                layoutControlTop.Enabled = false;
                gridControlOrder.Enabled = false;
                var orderUrl = $"https://public.kiotapi.com/orders/code/{code}";
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, (string)null, "GET");
                if (!success && content == null) MessageBox.Show("Lỗi khi lấy dữ liệu Kiotviet");

                var orderApiResponse = JsonConvert.DeserializeObject<OrderResponse>(content);
                if (orderApiResponse == null) return;
                foreach (var orderApi in orderApiResponse.OrderDetails)
                {
                    var start = orderApi.ProductName.LastIndexOf('(');
                    var end = orderApi.ProductName.LastIndexOf(')');

                    if (start == -1 || end <= start) continue;
                    orderApi.Unit = orderApi.ProductName.Substring(start + 1, end - start - 1).Trim();
                    orderApi.ProductName = orderApi.ProductName[..start].Trim();
                }
                switch (orderApiResponse.Status)
                {
                    case (int)OrderStatusEnum.Finished:
                        MessageHelper.MsgBox($"Đơn hàng: {code} đã Hoàn thành", MsgType.Error_);
                        txtProductCode.ReadOnly = true;
                        chkFinish.Checked = true;
                        chkFinish.BackColor = Color.LightGreen;
                        chkDraft.Checked = false;
                        chkCancel.Checked = false;
                        break;
                    case (int)OrderStatusEnum.Cancel:
                        MessageHelper.MsgBox($"Đơn hàng: {code} đã Huỷ", MsgType.Error_);
                        txtProductCode.ReadOnly = true;
                        chkCancel.Checked = true;
                        chkCancel.BackColor = Color.OrangeRed;
                        chkDraft.Checked = false;
                        chkFinish.Checked = false;
                        break;
                    case (int)OrderStatusEnum.Draft:
                        chkDraft.Checked = true;
                        chkDraft.BackColor = Color.Green;
                        chkDraft.ForeColor = Color.White;
                        chkFinish.Checked = false;
                        chkCancel.Checked = false;
                        break;
                    default:
                        break;
                }

                if (orderApiResponse.Status != 1)
                {
                    MessageHelper.MsgBox($"Đơn hàng: {code} đã hoàn thành", MsgType.Error_);
                    txtProductCode.ReadOnly = true;
                }
                else
                {
                    txtProductCode.ReadOnly = false;
                }

                txtCustomerName.Text = orderApiResponse.CustomerName; // Tên khách hàng
                txtSaleName.Text = orderApiResponse.SoldByName; // Tên người bán.
                txtSumTotal.Text = NumberFormatter.FormatDecimal(orderApiResponse.Total); // Tổng hoá đơn
                txtTotalPayment.Text = NumberFormatter.FormatDecimal(orderApiResponse.TotalPayment); // Khách đã trả
                txtTotal.Text = NumberFormatter.FormatDecimal(orderApiResponse.Total); // Khách cần trả
                _orderResponse = orderApiResponse;
                txtScanNumber.ReadOnly = true;
                txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" +
                                     orderApiResponse.OrderDetails.Count().ToString();
                gridControlOrder.DataSource = _orderResponse.OrderDetails;
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

        private void SetTextEditHeight(Control control, int height)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextEdit textEdit)
                {
                    textEdit.Properties.AutoHeight = false;
                    textEdit.MinimumSize = new Size(0, height);
                    textEdit.MaximumSize = new Size(0, height);
                }
                else if (c.HasChildren)
                {
                    SetTextEditHeight(c, height); // Đệ quy
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
                var findProduct = _orderResponse.OrderDetails.FirstOrDefault(p => p.ProductCode == productCode);
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
                    txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" + _orderResponse.OrderDetails.Count().ToString();
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

            if (view.GetRow(e.RowHandle) is not OrderDetail row) return;

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
            if (_scannedBarcodeCount == _orderResponse.OrderDetails.Count())
            {
                var result = MessageHelper.MsgBox("Hoàn thành đơn hàng", MsgType.YesNo);
                if (result == DialogResult.Yes)
                {
                    MessageHelper.MsgBox("Hoàn thành đơn hàng thành công", MsgType.Information);
                }
            }
            else
            {
                MessageHelper.MsgBox("Quét mã sản phẩm trước khi hoàn thành", MsgType.Error_);
            }

        }

        private void txtOrderCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            var orderCode = txtOrderCode.Text.Trim();
            if (string.IsNullOrEmpty(orderCode)) return;
            LoadData(orderCode);
        }

        private (bool check, string code) TryFindProductCode(string searchBarCode)
        {
            return productLookupDictionary.TryGetValue(searchBarCode, out var codeValue) ? (true, codeValue) : (false, null);
        }

        private void FrmOrderProcess_Shown(object sender, EventArgs e)
        {
            txtProductCode.Focus();
        }
    }

}