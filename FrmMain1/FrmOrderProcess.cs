using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using FrmMain.Dto.Response;
using Newtonsoft.Json;
using Services;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmOrderProcess : XtraForm
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentCode { get; set; }
        private readonly IKiotVietService _kiotVietService;
        private string _searchProductCode = "";
        private int _scanBarcode = 0;
        private OrderResponse orderResponse;
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
                var orderUrl = $"https://public.kiotapi.com/orders/code/{code}";
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, (string)null, "GET");
                if (!success && content == null) MessageBox.Show("Lỗi khi lấy dữ liệu Kiotviet");

                var orderApiResponse = JsonConvert.DeserializeObject<OrderResponse>(content);
                if (orderApiResponse == null) return;
                txtCustomerName.Text = orderApiResponse.CustomerName;
                txtSaleName.Text = orderApiResponse.SoldByName;
                txtSumTotal.Text = orderApiResponse.Total.ToString();
                txtTotalPayment.Text = orderApiResponse.TotalPayment.ToString(CultureInfo.InvariantCulture);
                orderResponse = orderApiResponse;
                txtScanNumber.Text = $"{_scanBarcode.ToString()}" + "/" + orderApiResponse.OrderDetails.Count().ToString();
                gridControlOrder.DataSource = orderResponse.OrderDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private async void txtOrderCode_EditValueChanged(object sender, EventArgs e)
        {
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
        }

        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            var barcode = txtProductCode.Text.Trim();
            txtProductCode.SelectAll();
            _searchProductCode = barcode;
            if (string.IsNullOrEmpty(barcode)) return;
            e.Handled = true;
            var findProduct = orderResponse.OrderDetails.FirstOrDefault(p => p.ProductCode == barcode);
            if (findProduct != null)
            {
                _scanBarcode++;
                findProduct.Checked = true;
                gridControlOrder.RefreshDataSource();
                var rowHandle = gridViewOrder.LocateByValue("ProductCode", barcode);
                if (rowHandle < 0) return;
                gridViewOrder.FocusedRowHandle = rowHandle;        
                gridViewOrder.MakeRowVisible(rowHandle);
                txtScanNumber.Text = $"{_scanBarcode.ToString()}" + "/" + orderResponse.OrderDetails.Count().ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm mã : " + barcode);
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
    }

}