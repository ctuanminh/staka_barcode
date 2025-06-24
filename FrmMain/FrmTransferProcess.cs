using Be.Common.Purchase_Order.Response;
using Be.Common.Tranfer.Response;
using Be.Core.Entities;
using Be.Services.Catalog;
using Be.Services.Identity;
using Be.Services.KiotViet;
using Be.Services.System;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using Be.Services.Transfer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmTransferProcess : XtraForm
    {
        #region Fields
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentCode { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static long CurrentId { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static bool Transfer;

        private readonly IKiotVietService _kiotVietService;
        private readonly IUserService _userService;
        private int _scannedBarcodeCount;
        private int _branchId;
        private TransferResponse _transferResponse;
        private readonly IProductService _productService;
        private readonly ISystemService _systemService;
        private readonly ITransferService _transferService;
        private Dictionary<string, string> _productLookupDictionary;
        private Timer _reloadTimer;
        private DateTime _nextReloadTime;
        private const int ReloadIntervalMinutes = 60;
        #endregion

        #region Form
        public FrmTransferProcess(IKiotVietService kiotVietService, IProductService productService, IUserService userService, ISystemService systemService, ITransferService transferService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            _userService = userService;
            _systemService = systemService;
            _transferService = transferService;
            InitializeComponent();
            StartCountdownTimer();
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
            _branchId = setting?.SettingValue != null ? int.Parse(setting.SettingValue) : 0;
            ReloadData(CurrentCode, CurrentId, Transfer);
        }

        private void FrmTransferProcess_Shown(object sender, EventArgs e)
        {
            txtProductCode.Focus();
        }
        #endregion

        #region LoadData

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
                _scannedBarcodeCount = 0;

                var url = $"https://public.kiotapi.com/transfers/{transferId}";
                var (success, content) = await _kiotVietService.CallApiAsync(url, (string)null, "GET");

                //Lưu số lần request lên kiotviet
                await _systemService.AddRequest(new RequestEntity()
                {
                    Module = Name,
                    Url = url,
                    IsSuccess = success,
                    BranchId = _branchId
                });

                if (!success || string.IsNullOrWhiteSpace(content))
                {
                    MessageHelper.MsgBox("Không tìm thấy dữ liệu phiếu chuyển hàng", MsgType.Error_);
                    return;
                }

                TransferResponse transferResponse;
                try
                {
                    transferResponse = JsonConvert.DeserializeObject<TransferResponse>(content);
                }
                catch (Exception ex)
                {
                    MessageHelper.MsgBox("Lỗi đọc dữ liệu trả về từ API.", MsgType.Error_);
                    return;
                }

                if (transferResponse == null) return;

                // Kiểm tra transfer nếu chưa tồn tại thì thêm mới
                var transferExist = await _transferService.GetTransferById(transferResponse.Id);
                if (transferExist == null)
                {
                    await _transferService.AddOrUpdateTransfer(new TransferEntity()
                    {
                        TransferId = transferResponse.Id,
                        TransferCode = transferResponse.Code,
                        FromBranchId = transferResponse.FromBranchId,
                        ToBranchId = transferResponse.ToBranchId,
                        Status = transferResponse.Status
                    });
                }

                // Cập nhật UI
                HandleTransferStatusUi(transferResponse);
                SetStatusCheckboxStyle();
                ResetStatusCheckboxes();
                ProcessProductUnits(transferResponse.Details);

                var userTransfer = await _userService.GetUserById(transferResponse.CreatedById);
                txtToBranchName.Text = transferResponse.ToBranchName;
                txtTranferName.Text = userTransfer?.FullName;
                txtFromBranchName.Text = transferResponse.FromBranchName;

                // Tổng hợp số lượng
                txtTotalSend.Text = transferResponse.Details.Sum(p => p.TransferredQuantity).ToString();
                txtTotalReceivered.Text = transferResponse.Details.Sum(p => p.ReceiveQuantity).ToString();

                _transferResponse = transferResponse;

                txtDispatchedDate.Text = transferResponse.DispatchedDate?.ToString("dd/MM/yyyy HH:mm:ss") ?? string.Empty;
                txtReceivedDate.Text = transferResponse.ReceivedDate?.ToString("dd/MM/yyyy HH:mm:ss") ?? string.Empty;
                txtNotes.Text = transferResponse.NoteBySource;

                // Đổ dữ liệu vào grid 1 lần duy nhất
                gridControlOrder.DataSource = _transferResponse.Details;
                gridViewOrder.BestFitColumns();

                // Xử lý load những sản phẩm đã check
                var shouldLoadCheckedList = (Transfer && transferResponse.Status == (int)TransferStatusEnum.Draft)
                                            || (!Transfer && transferResponse.Status == (int)TransferStatusEnum.Transferred);

                if (shouldLoadCheckedList)
                {
                    var transferCheckedList = await _transferService.GetCheckedProductsByParentTransfer(
                        transferId, transferResponse.Code, _branchId, AppGlobals.UserInfo.UserName, Transfer);

                    if (transferCheckedList != null && transferCheckedList.Any())
                    {
                        if (MessageHelper.MsgBox("Tải lại những sản phẩm đã check mã", MsgType.YesNo) == DialogResult.Yes)
                        {
                            var transferCheckedListDic = transferCheckedList
                                .ToDictionary(p => p.ProductBarCode, p => p.Checked);

                            foreach (var product in _transferResponse.Details)
                            {
                                if (!transferCheckedListDic.TryGetValue(product.ProductCode, out var isChecked) ||
                                    !isChecked) continue;
                                product.Checked = true;
                                _scannedBarcodeCount++;
                            }

                            gridControlOrder.RefreshDataSource();
                        }
                    }
                }

                txtScanNumber.ReadOnly = true;
                txtScanNumber.Text = $"{_scannedBarcodeCount}/{transferResponse.Details.Count()}";

                LoadProduct(_transferResponse.Details);
                SetOrderStatusUI(transferResponse.Status);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu.\n" + ex.Message, MsgType.Error_);
            }
            finally
            {
                SetControlEnable(true);
                BeginInvoke(new Action(() => txtProductCode.Focus()));
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
            catch (Exception ex)
            {
                MessageHelper.MsgBox($"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error_);
            }
        }

        #endregion

        /// <summary>
        /// Tìm theo mã vạch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode != Keys.Enter) return;

                var searchBarcode = txtProductCode.Text.Trim();
                txtProductCode.SelectAll();

                if (string.IsNullOrEmpty(searchBarcode)) return;

                e.Handled = true;

                var (isProductFound, productCode) = TryFindProductCode(searchBarcode);

                if (!isProductFound)
                {
                    MessageHelper.MsgBox("Không tìm thấy sản phẩm mã: " + searchBarcode, MsgType.Error_);
                    return;
                }

                var findProduct = _transferResponse.Details.FirstOrDefault(p => p.ProductCode == productCode);
                if (findProduct == null)
                {
                    MessageHelper.MsgBox("Không tìm thấy sản phẩm mã: " + searchBarcode + " trong đơn hàng", MsgType.Error_);
                    return;
                }

                var productChecked = await _transferService.GetCheckedProductByTransfer(
                        _transferResponse.Id,
                        _transferResponse.Code,
                        _branchId,
                        AppGlobals.UserInfo.UserName,
                        Transfer,
                        findProduct.ProductCode
                    );

                if (productChecked == null)
                {
                    await _transferService.AddOrUpdateProductCheck(new TransferChecked()
                    {
                        TransferId = _transferResponse.Id,
                        TransferCode = _transferResponse.Code,
                        ProductBarCode = findProduct.ProductCode,
                        BranchId = _branchId,
                        UserName = AppGlobals.UserInfo.UserName,
                        Checked = true
                    });
                }

                if (findProduct.Checked) return;

                _scannedBarcodeCount++;
                findProduct.Checked = true;

                await InvokeAsync(() =>
                {
                    gridControlOrder.RefreshDataSource();
                    var rowHandle = gridViewOrder.LocateByValue("ProductCode", productCode);
                    if (rowHandle >= 0)
                    {
                        gridViewOrder.FocusedRowHandle = rowHandle;
                        gridViewOrder.MakeRowVisible(rowHandle);
                    }
                    txtScanNumber.Text = $"{_scannedBarcodeCount}/{_transferResponse.Details.Count()}";
                    txtProductCode.Focus();
                });
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Có lỗi trong quá trình lấy dữ liệu", MsgType.Error_);
            }
        }

        #region gridView
        private void gridViewOrder_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (sender is not GridView view) return;

            if (view.GetRow(e.RowHandle) is not TransferDetail row) return;

            if (!row.Checked) return;
            e.Appearance.BackColor = Color.LightGreen; // Màu xanh nhạt
            e.Appearance.ForeColor = Color.Black;      // Text màu đen (tuỳ chọn)
        }

        private void gridViewOrder_ShownEditor(object sender, CancelEventArgs e)
        {
            var view = sender as GridView;
            view?.ActiveEditor?.Focus();
            view?.ActiveEditor?.SelectAll();

        }
        private void gridViewOrder_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (sender is not GridView view) return;
            if (view.FocusedColumn.FieldName != "ReceiveQuantity") return;

            var transferredQuantityObj = view.GetRowCellValue(view.FocusedRowHandle, "TransferredQuantity");
            if (transferredQuantityObj == null) return;

            if (!int.TryParse(e.Value?.ToString(), out var receiveQuantity)) return;
            if (!int.TryParse(transferredQuantityObj.ToString(), out var transferredQuantity)) return;

            if (receiveQuantity <= 0)
            {
                e.Value = 0;
            }
            if (receiveQuantity > transferredQuantity)
            {
                e.Value = transferredQuantity;
            }
        }

        #endregion

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (_transferResponse != null && _scannedBarcodeCount == _transferResponse.Details.Count())
            {
                // Kiểm tra: nếu số thực nhận: ReceiveQuantity # TransferredQuantity thì không cho hoàn thành
                //Commnet lại sau khi thống nhất với Dũng: Nhỏ hơn SL chuyển cho nhận, vượt SL thì sao chép phiếu 
                //Phải huỷ phiếu gốc.
                //if (!Transfer)
                //{
                //    var hasMismatch =
                //        _transferResponse.Details.Any(p => (int)p.ReceiveQuantity > p.TransferredQuantity);
                //    if (hasMismatch)
                //    {
                //        var productsMismatch = _transferResponse.Details
                //            .Where(p => (int)p.ReceiveQuantity != p.TransferredQuantity)
                //            .Select(p => p.ProductCode)
                //            .ToList();

                //        var firstMismatchRow = _transferResponse.Details
                //            .FindIndex(p => (int)p.ReceiveQuantity != p.TransferredQuantity);
                //        if (firstMismatchRow >= 0)
                //        {
                //            gridViewOrder.FocusedRowHandle = firstMismatchRow;
                //            gridViewOrder.MakeRowVisible(firstMismatchRow);
                //        }

                //        MessageHelper.MsgBox(
                //        $"Mã SP {string.Join(", ", productsMismatch)} có Số lượng thực nhận không khớp với số lượng chuyển. Vui lòng kiểm tra lại.",
                //        MsgType.Error_);
                //        return;
                //    }
                //}

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
            return _productLookupDictionary.TryGetValue(searchBarCode.ToUpper(), out var codeValue) ? (true, codeValue) : (false, null);
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
            ReloadData(CurrentCode, CurrentId, Transfer);
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
                        receiveQuantity = Transfer ? product.TransferredQuantity : product.ReceiveQuantity,
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
            rpReceiveQuantity.ReadOnly = Transfer;
            clmReceiveQuantity.Visible = !Transfer;
            clmReceiveQuantity.OptionsColumn.AllowEdit = !Transfer;
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
                    txtProductCode.ReadOnly = Transfer;
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
        private static bool CanComplete(TransferResponse transfer)
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