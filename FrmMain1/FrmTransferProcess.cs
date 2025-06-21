using Be.Common.Purchase_Order.Response;
using Be.Common.Tranfer.Response;
using Be.Services.Catalog;
using Be.Services.Identity;
using Be.Services.KiotViet;
using DevExpress.LookAndFeel.Design;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using FrmMain.App;
using FrmMain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmTransferProcess : XtraForm
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentCode { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static long CurrentId { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool Transfer;

        private readonly IKiotVietService _kiotVietService;
        private readonly IUserService _userService;
        private int _scannedBarcodeCount;
        private int _currentBranchId;
        private TransferResponse _transferResponse;
        private readonly IProductService _productService;
        private Dictionary<string, string> _productLookupDictionary;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 60;
        public FrmTransferProcess(IKiotVietService kiotVietService, IProductService productService, IUserService userService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            _userService = userService;
            InitializeComponent();
            StartCountdownTimer();
        }

        public void ReloadData(string transferCode, long transferId, bool _tranfer)
        {
            CurrentId = transferId;
            CurrentCode = transferCode;
            txtTranferCode.Text = transferCode;
            Transfer = _tranfer;
            _scannedBarcodeCount = 0;
            LoadData(transferId);
        }

        private async void LoadData(long transferId)
        {
            try
            {
                SetControlEnable(false);

                var url = $"https://public.kiotapi.com/transfers/{transferId}";
                var (success, content) = await _kiotVietService.CallApiAsync(url, (string)null, "GET");
                if (!success || string.IsNullOrWhiteSpace(content))
                {
                    MessageHelper.MsgBox("Không tìm thấy dữ liệu phiếu chuyển hàng", MsgType.Error_);
                    return;
                }

                var transferResponse = JsonConvert.DeserializeObject<TransferResponse>(content);
                if (transferResponse == null) return;

                HandleTransferStatusUi(transferResponse);
                SetStatusCheckboxStyle();
                ResetStatusCheckboxes();
                ProcessProductUnits(transferResponse.Details);

                var userTransfer = await _userService.GetUserById(transferResponse.CreatedById);
                txtToBranchName.Text = transferResponse.ToBranchName;
                txtTranferName.Text = userTransfer?.FullName;
                txtFromBranchName.Text = transferResponse.FromBranchName;

                // Tổng hợp số lượng
                var totalSend = transferResponse.Details
                    .Where(p => !string.IsNullOrWhiteSpace(p.ProductCode))
                    .Sum(p => p.TransferredQuantity);
                txtTotalSend.Text = totalSend.ToString();

                var totalReceived = transferResponse.Details
                    .Where(p => !string.IsNullOrWhiteSpace(p.ProductCode))
                    .Sum(p => p.TotalReceive);
                txtTotalReceivered.Text = totalReceived.ToString();

                _transferResponse = transferResponse;

                txtScanNumber.ReadOnly = true;
                txtScanNumber.Text = $"{_scannedBarcodeCount}/{transferResponse.Details.Count()}";

                txtDispatchedDate.Text = transferResponse.DispatchedDate?.ToString("dd/MM/yyyy HH:mm:ss") ?? string.Empty;
                txtReceivedDate.Text = transferResponse.ReceivedDate?.ToString("dd/MM/yyyy HH:mm:ss") ?? string.Empty;

                gridControlOrder.DataSource = _transferResponse.Details;
                gridViewOrder.BestFitColumns();

                LoadProduct(_transferResponse.Details);

                SetOrderStatusUI(transferResponse.Status);
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

        private void FrmTransferProcess_Load(object sender, EventArgs e)
        {
            SetTextEditHeight(this, 25);
            BeginInvoke(new Action(() => txtProductCode.Focus()));
            SetStatusCheckboxStyle();
            var setting = AppGlobals.AppSetting.FirstOrDefault(s =>
                s.ComputerName == Environment.MachineName &&
                s.ModuleName == "Branch" &&
                s.SettingKey == "BranchId");
            _currentBranchId = setting?.SettingValue != null ? int.Parse(setting.SettingValue) : 0;
            ReloadData(CurrentCode, CurrentId, Transfer);
        }

        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            var searchBarcode = txtProductCode.Text.Trim();
            txtProductCode.SelectAll();
            if (string.IsNullOrEmpty(searchBarcode)) return;
            var (isProductFound, productCode) = TryFindProductCode(searchBarcode);
            e.Handled = true;
            if (isProductFound)
            {
                var findProduct = _transferResponse.Details.FirstOrDefault(p => p.ProductCode == productCode);
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
                    txtScanNumber.Text = $"{_scannedBarcodeCount.ToString()}" + "/" + _transferResponse.Details.Count().ToString();
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

            if (view.GetRow(e.RowHandle) is not TransferDetail row) return;

            if (!row.Checked) return;
            e.Appearance.BackColor = Color.LightGreen; // Màu xanh nhạt
            e.Appearance.ForeColor = Color.Black;      // Text màu đen (tuỳ chọn)
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (_scannedBarcodeCount == _transferResponse.Details.Count())
            {
                FinishOrder();
            }
            else
            {
                var listNotScan = _transferResponse.Details.Where(p => !p.Checked)
                    .Select(p => p.ProductCode)
                    .ToList();
                var message =
                    $"Còn {listNotScan.Count} sản phẩm chưa quét mã: {string.Join(", ", listNotScan)}.\nVui lòng thực hiện trước khi hoàn thành.";
                MessageHelper.MsgBox(message, MsgType.Error_);
                txtProductCode.Focus();
            }
        }

        private (bool check, string code) TryFindProductCode(string searchBarCode)
        {
            return _productLookupDictionary.TryGetValue(searchBarCode, out var codeValue) ? (true, codeValue) : (false, null);
        }

        private void FrmTransferProcess_Shown(object sender, EventArgs e)
        {
            txtProductCode.Focus();
        }

        // Tick mỗi giây
        private void ReloadTimer_Tick(object sender, EventArgs e)
        {
            var remaining = _nextReloadTime - DateTime.Now;

            if (remaining <= TimeSpan.Zero)
            {
                _reloadTimer.Stop();
                btnReloadOrder.Text = "Loading...";

                LoadData(CurrentId); // gọi reload dữ liệu

                // Khởi động lại đếm ngược
                _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
                _reloadTimer.Start();
            }
            else
            {
                btnReloadOrder.Text = $"Tải lại sau: {remaining.Minutes:D2}:{remaining.Seconds:D2}";
            }
        }

        // Hàm khởi động Timer đếm ngược
        private void StartCountdownTimer()
        {
            _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);

            _reloadTimer = new Timer();
            _reloadTimer.Interval = 1000; // mỗi 1 giây
            _reloadTimer.Tick += ReloadTimer_Tick;
            _reloadTimer.Start();
        }

        private void btnReloadOrder_Click(object sender, EventArgs e)
        {
            _reloadTimer?.Stop();
            ReloadData(CurrentCode, CurrentId,Transfer);
            btnReloadOrder.Text = "Loading...";
            _nextReloadTime = DateTime.Now.AddMinutes(ReloadIntervalMinutes);
            _reloadTimer?.Start();
        }

       private async void FinishOrder()
       {
            try
            {
                SetControlEnable(false);
                var orderUrl = $"https://public.kiotapi.com/transfers/{CurrentId}";
                var (success, content) = await _kiotVietService.CallApiAsync(orderUrl, (string)null, "GET");
                if (!success || string.IsNullOrEmpty(content))
                {
                    MessageHelper.MsgBox("Lỗi khi lấy dữ liệu Kiotviet", MsgType.Error_);
                    return;
                }

                var transferResponse = JsonConvert.DeserializeObject<TransferResponse>(content);
                if (transferResponse == null)
                {
                    MessageHelper.MsgBox("Dữ liệu đơn hàng trả về không hợp lệ", MsgType.Error_);
                    return;
                }

                if (!CanComplete(transferResponse))
                {
                    MessageHelper.MsgBox("Kiểm tra dữ liệu trước khi thực hiện", MsgType.Error_);
                }


                // Xác định trạng thái kế tiếp
                var nextStatus = transferResponse.Status switch
                {
                    (int)TransferStatusEnum.Draft => (int)TransferStatusEnum.Transferred,
                    _ => (int)TransferStatusEnum.Finished
                };

                var transferRequest = new
                {
                    fromBranchId = transferResponse.FromBranchId,
                    toBranchId = transferResponse.ToBranchId,
                    code = transferResponse.Code,
                    status = nextStatus,
                    isDraft = false,
                    dispatchedDate = transferResponse.DispatchedDate,
                    transferDetails = transferResponse.Details.Select(product => new
                    {
                        transferId = transferResponse.Id,
                        productId = product.ProductId,
                        productCode = product.ProductCode,
                        productName = product.ProductName,
                        sendQuantity = product.TransferredQuantity,
                        receiveQuantity =product.TransferredQuantity,
                        price = product.Price
                    }).ToList(),
                };

                var (updateSuccess, updateContent) = await _kiotVietService.CallApiAsync(orderUrl, transferRequest, "PUT");

                if (!updateSuccess || string.IsNullOrEmpty(updateContent))
                {
                    MessageHelper.MsgBox($"Có lỗi khi cập nhật đơn hàng: {updateContent}", MsgType.Error_);
                    return;
                }

                MessageHelper.MsgBox("Thao tác được thực hiện thành công.", MsgType.Information);
                ReloadData(CurrentCode, CurrentId, Transfer);
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

       
        private async void LoadProduct(List<TransferDetail> transferDetails)
       {
           try
           {
               var productCodes = transferDetails
                   .Select(p => p.ProductCode)
                   .Where(code => !string.IsNullOrWhiteSpace(code))
                   .Distinct()
                   .ToList();
               var productCodeBarCode = await _productService.SynAndGetProductCodeBarCode(productCodes);
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
               MessageHelper.MsgBox($"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error_);
           }
       }


        private void SetControlEnable(bool enable)
        {
            layoutControlTop.Enabled = enable;
            gridControlOrder.Enabled = enable;
        }

        private void SetStatusCheckboxStyle()
        {
            chkFinish.BackColor = Color.LightGreen;
            chkDraft.BackColor = Color.Green;
            chkDraft.ForeColor = Color.White;
            chkCancel.BackColor = Color.OrangeRed;
            chkCancel.ForeColor = Color.White;
            chkTranfered.BackColor = Color.Cyan;
            chkTranfered.ForeColor = Color.Black;
        }

        private void SetOrderStatusUI(int status)
        {
            chkFinish.Checked = status == 3; // Trạng thái đã nhận
            chkCancel.Checked = status == 4; // Trạng thái đã huỷ
            chkDraft.Checked = status == 1; // Trạng thái tạm
            chkTranfered.Checked = status == 2; // Trạng thái đã chuyển
        }


        /// <summary>
        /// Tên sản phẩm & đơn vị tính
        /// </summary>
        /// <param name="details"></param>
        private static void ProcessProductUnits(IEnumerable<TransferDetail> details)
        {
            foreach (var transfer in details)
            {
                var start = transfer.ProductName.LastIndexOf('(');
                var end = transfer.ProductName.LastIndexOf(')');
                if (start == -1 || end <= start) continue;

                transfer.Unit = transfer.ProductName.Substring(start + 1, end - start - 1).Trim();
                transfer.ProductName = transfer.ProductName[..start].Trim();
            }
        }

        /// <summary>
        /// Xử lý trạng thái hiển thị tuỳ thuộc vào trạng thái của phiếu chuyển hàng
        /// và chi nhánh hiện tại: là chi nhánh chuyển hay nhận hàng.
        /// </summary>
        /// <param name="transfer"></param>
        private void HandleTransferStatusUi(TransferResponse transfer)
        {
            ltCtlCode.Text = Transfer ? "Mã Phiếu Chuyển" : "Mã Phiếu Nhận";
            switch (transfer.Status)
            {
                case (int)TransferStatusEnum.Draft:
                    Text = Transfer ? "Xử lý Phiếu Chuyển hàng" : "Xử lý Phiếu Nhận hàng";
                    grpCtlFilter.Text = "Phiếu Chuyển hàng";
                    btnFinish.Text = Transfer ? "Chuyển hàng" : "Nhận hàng";
                    txtProductCode.ReadOnly = !Transfer;
                    break;

                case (int)TransferStatusEnum.Transferred:
                    Text = Transfer ? "Xử lý Phiếu Chuyển hàng" : "Xử lý Phiếu Nhận hàng";
                    grpCtlFilter.Text = Transfer ? "Phiếu Chuyển hàng" : "Phiếu Nhận hàng";
                    grpCtlFilter.Text = "Nhận hàng";
                    btnFinish.Text = "Nhận hàng";
                    if (Transfer)
                    {
                        MessageHelper.MsgBox("Phiếu đang ở trạng thái Đang chuyển, vui lòng kiểm tra lại",
                            MsgType.Error_);
                        txtProductCode.ReadOnly = true;
                    }
                    else
                    {
                        txtProductCode.ReadOnly = false;
                    }
                    break;

                case (int)TransferStatusEnum.Finished:
                case (int)TransferStatusEnum.Cancelled:
                    Text = Transfer ? "Xử lý Phiếu Chuyển hàng" : "Xử lý Phiếu Nhận hàng";
                    grpCtlFilter.Text = Transfer ? "Phiếu Chuyển hàng" : "Phiếu Nhận hàng";
                    txtProductCode.ReadOnly = true;
                    break;

                default:
                    txtProductCode.ReadOnly = true;
                    break;
            }
        }
        private bool CanComplete(TransferResponse transfer)
        {
            return transfer.Status switch
            {
                (int)TransferStatusEnum.Draft => Transfer,
                (int)TransferStatusEnum.Transferred => !Transfer,
                _ => false
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
        private void ResetStatusCheckboxes()
        {
            chkFinish.Checked = false;
            chkCancel.Checked = false;
            chkDraft.Checked = false;
        }
    }

}