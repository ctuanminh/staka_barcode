using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using FrmMain.Dto.Response;
using FrmMain.Utils;
using Newtonsoft.Json;
using Services;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
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
        public FrmOrderProcess(IKiotVietService kiotVietService)
        {
            _kiotVietService = kiotVietService;
            InitializeComponent();
            ReloadData(CurrentCode);
            txtOrderCode.Text = CurrentCode;
        }

        public void ReloadData(string code)
        {
            CurrentCode = code;
            txtOrderCode.Text = code;
            LoadData(code);
        }

        private async void LoadData(string code)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(LoadingForm), true, true);
                SplashScreenManager.Default.SetWaitFormCaption("Đang lấy Đơn hàng");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng đợi...");
                layoutControlTop.Enabled = false;
                var orderUrl = $"https://public.kiotapi.com/orders/code/{code}";
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, (string)null, "GET");
                if (!success && content == null) MessageBox.Show("Lỗi khi lấy dữ liệu Kiotviet");

                var orderApiResponse = JsonConvert.DeserializeObject<OrderResponse>(content);
                if (orderApiResponse == null) return;
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
            var barcode = txtProductCode.Text.Trim();
            txtProductCode.SelectAll();
            _searchProductCode = barcode;
            if (string.IsNullOrEmpty(barcode)) return;
            e.Handled = true;
            var findProduct = _orderResponse.OrderDetails.FirstOrDefault(p => p.ProductCode == barcode);
            if (findProduct != null)
            {
                if (findProduct.Checked) return;
                _scannedBarcodeCount++;
                findProduct.Checked = true;
                gridControlOrder.RefreshDataSource();
                var rowHandle = gridViewOrder.LocateByValue("ProductCode", barcode);
                if (rowHandle < 0) return;
                gridViewOrder.FocusedRowHandle = rowHandle;
                gridViewOrder.MakeRowVisible(rowHandle);
                txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" + _orderResponse.OrderDetails.Count().ToString();
            }
            else
            {
                MessageHelper.MsgBox("Không tìm thấy sản phẩm mã: " + barcode + " trong đơn hàng", MsgType.Error_);
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
            if(string.IsNullOrEmpty(orderCode)) return;
            LoadData(orderCode);
        }
    }

}