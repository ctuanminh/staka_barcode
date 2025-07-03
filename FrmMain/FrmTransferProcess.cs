using Be.Common.Order.Response;
using Be.Common.PurchaseOrder.Response;
using Be.Common.Tranfer.Response;
using Be.Core.Entities;
using Be.Services.Catalog;
using Be.Services.Identity;
using Be.Services.KiotViet;
using Be.Services.Pos;
using Be.Services.System;
using Be.Services.Transfer;
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
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmTransferProcess : FrmBasePos, IReloadableForm
    {
        #region Fields
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string CurrentCode { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static long CurrentId { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private static bool _transfer;

        private readonly IKiotVietService _kiotVietService;
        private readonly IUserService _userService;
        private int _scannedBarcodeCount;
        private int _branchId;
        private TransferResponse _transferResponse;
        private readonly IProductService _productService;
        private readonly ITransferService _transferService;
        private Dictionary<string, string> _productLookupDictionary;
        #endregion

        #region Form

        public FrmTransferProcess(IKiotVietService kiotVietService,
            IProductService productService, IUserService userService,
            ISystemService systemService, ITransferService transferService,
            IBranchService branchService) : base(branchService, systemService)
        {
            _kiotVietService = kiotVietService;
            _productService = productService;
            _userService = userService;
            _transferService = transferService;
            InitializeComponent();
        }

        private void FrmTransferProcess_Load(object sender, EventArgs e)
        {
            SetTextEditHeight(this, 25);
            BeginInvoke(() => txtProductCode.Focus());
            SetStatusCheckboxStyle();
            var setting = AppGlobals.AppSetting.FirstOrDefault(s =>
                s.ComputerName == Environment.MachineName &&
                s.ModuleName == "Branch" &&
                s.SettingKey == "BranchId");
            _branchId = setting?.SettingValue != null ? int.Parse(setting.SettingValue) : 0;
        }

        private async void FrmTransferProcess_Shown(object sender, EventArgs e)
        {
            try
            {
                await LoadData(CurrentId);
                txtProductCode.Focus();
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình xử lý đơn hàng.", MsgType.Error);
            }
        }
        #endregion

        #region LoadData

        public async Task ReLoadData(string transferCode, long transferId)
        {
            CurrentId = transferId;
            CurrentCode = transferCode;
            txtTranferCode.Text = transferCode;
            _scannedBarcodeCount = 0;
            await LoadDefaultSetting();
            if (IsHandleCreated && Visible)
            {
                await LoadData(transferId);
            }
        }

        private async Task LoadData(long transferId)
        {
            try
            {
                SetControlEnable(false);
                _scannedBarcodeCount = 0;

                var url = $"https://public.kiotapi.com/transfers/{transferId}";
                var (success, content) = await _kiotVietService.CallApiAsync(url, (string)null, "GET");

                if (!success || string.IsNullOrWhiteSpace(content))
                {
                    MessageHelper.MsgBox(this, "Không tìm thấy dữ liệu phiếu chuyển hàng", MsgType.Error);
                    return;
                }

                TransferResponse transferResponse;
                try
                {
                    transferResponse = JsonConvert.DeserializeObject<TransferResponse>(content);
                }
                catch (Exception ex)
                {
                    MessageHelper.MsgBox(this, "Lỗi đọc dữ liệu trả về từ API.", MsgType.Error);
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
                SetStatusCheckboxStyle();
                ResetStatusCheckboxes();
                ProcessProductUnits(transferResponse.Details);

                var userTransfer = await _userService.GetUserById(transferResponse.CreatedById);
                txtToBranchName.Text = transferResponse.ToBranchName;
                txtTranferName.Text = userTransfer != null ? userTransfer.FullName : "Đơn hàng Staka";
                txtFromBranchName.Text = transferResponse.FromBranchName;

                // Tổng hợp số lượng
                txtTotalSend.Text = transferResponse.Details.Sum(p => p.TransferredQuantity).ToString();
                txtTotalReceivered.Text = transferResponse.Details.Sum(p => p.ReceiveQuantity).ToString();

                _transferResponse = transferResponse;

                txtDispatchedDate.Text = transferResponse.DispatchedDate?.ToString("dd/MM/yyyy HH:mm:ss") ?? string.Empty;
                txtReceivedDate.Text = transferResponse.ReceivedDate?.ToString("dd/MM/yyyy HH:mm:ss") ?? string.Empty;
                txtNotes.Text = transferResponse.NoteBySource;

                txtScanNumber.ReadOnly = true;
                txtScanNumber.Text = $"{_scannedBarcodeCount}/{transferResponse.Details.Count()}";
                // Đổ dữ liệu vào grid 1 lần duy nhất
                gridControlOrder.DataSource = _transferResponse.Details;
                gridViewOrder.BestFitColumns();
                if (_transferResponse.FromBranchId == BranchId)
                {
                    _transfer = true;
                }
                // Xử lý load những sản phẩm đã check
                var shouldLoadCheckedList = (_transfer && transferResponse.Status == (int)TransferStatusEnum.Draft)
                                            || (!_transfer && transferResponse.Status == (int)TransferStatusEnum.Transferred);

                if (shouldLoadCheckedList)
                {
                    var transferCheckedList = await _transferService.GetCheckedProductsByParentTransfer(
                        transferId, transferResponse.Code, _branchId, AppGlobals.UserInfo.UserName, _transfer);

                    if (transferCheckedList != null && transferCheckedList.Any())
                    {
                        if (MessageHelper.MsgBox(this, "Tải lại những sản phẩm đã check mã", MsgType.YesNo) == DialogResult.Yes)
                        {
                            foreach (var product in _transferResponse.Details)
                            {
                                var findProductChecked =
                                    transferCheckedList.FirstOrDefault(c => c.ProductBarCode == product.ProductCode) ??
                                    transferCheckedList.FirstOrDefault(c => c.ProductCode == product.ProductCode);
                                if (findProductChecked == null) continue;

                                product.Checked = true;
                                product.ScanCount = findProductChecked.ScanCount;
                                _scannedBarcodeCount++;
                            }
                            gridControlOrder.RefreshDataSource();
                        }
                    }
                }
                HandleTransferStatusUi(transferResponse);
                LoadProduct(_transferResponse.Details);
                SetOrderStatusUI(transferResponse.Status);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình lấy dữ liệu.\n" + ex.Message, MsgType.Error);
            }
            finally
            {
                SetControlEnable(true);
                BeginInvoke(() => txtProductCode.Focus());
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
                MessageHelper.MsgBox(this, $"Có lỗi trong quá trình lấy dữ liệu: {ex}", MsgType.Error);
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
                    MessageHelper.MsgBox(this, "Không tìm thấy sản phẩm mã: " + searchBarcode, MsgType.Error);
                    return;
                }

                var findProduct = _transferResponse.Details.FirstOrDefault(p => p.ProductCode == productCode);
                if (findProduct == null)
                {
                    MessageHelper.MsgBox(this, "Không tìm thấy sản phẩm mã: " + searchBarcode + " trong đơn hàng", MsgType.Error);
                    return;
                }

                if (!findProduct.Checked)
                {
                    _scannedBarcodeCount++;
                }

                findProduct.ScanCount++;
                findProduct.Checked = true;

                var productChecked = await _transferService.GetCheckedProductByTransfer(
                    _transferResponse.Id,
                    _transferResponse.Code,
                    _branchId,
                    AppGlobals.UserInfo.UserName,
                    _transfer,
                    searchBarcode,
                    findProduct.ProductCode
                );

                if (productChecked == null)
                {
                    await _transferService.AddProductCheck(new TransferChecked()
                    {
                        TransferId = _transferResponse.Id,
                        TransferCode = _transferResponse.Code,
                        ProductBarCode = searchBarcode, //Trường hợp barcode # productCode.
                        ProductCode = findProduct.ProductCode,
                        BranchId = _branchId,
                        UserName = AppGlobals.UserInfo.UserName,
                        Checked = true,
                        Transfer = _transfer,
                        ScanCount = findProduct.ScanCount
                    });
                }
                else
                {
                    productChecked.ScanCount = findProduct.ScanCount;
                    await _transferService.UpdateProductCheck(_transferResponse.Id, productChecked);
                }

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
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình lấy dữ liệu", MsgType.Error);
            }
        }

        #region gridView
        private void gridViewOrder_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (sender is not GridView view) return;

            if (view.GetRow(e.RowHandle) is not TransferDetail row) return;

            if (!row.Checked) return;
            var scanCount = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "ScanCount"));
            var quantity = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "TransferredQuantity"));
            if (scanCount != quantity)
            {
                e.Appearance.BackColor = Color.LightCoral; // màu đỏ nhạt
                e.Appearance.ForeColor = Color.Black;
                return;
            }
            e.Appearance.BackColor = Color.LightGreen; // Màu xanh nhạt
            e.Appearance.ForeColor = Color.Black;      // Text màu đen (tuỳ chọn)
        }

        private void gridViewOrder_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (sender is not GridView view) return;
            if (view.FocusedColumn.FieldName != "ScanCount") return;
            if (view.GetRow(view.FocusedRowHandle) is not TransferDetail row) return;
            if (row.Checked) return;
            e.Cancel = true;
        }
        
        private void gridViewEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            gridViewOrder.CloseEditor();
            gridViewOrder.UpdateCurrentRow();
            BeginInvoke(() =>
            {
                txtProductCode.Focus();
            });
        }

        private async void gridViewOrder_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                if (sender is not GridView view) return;
                if (view.FocusedColumn.FieldName != "ScanCount") return;

                var transferredQuantityObj = view.GetRowCellValue(view.FocusedRowHandle, "TransferredQuantity");
                var produtCodeObj = view.GetRowCellValue(view.FocusedRowHandle, "ProductCode");
                if (transferredQuantityObj == null) return;

                if (!int.TryParse(e.Value?.ToString(), out var scanCount)) return;
                if (!int.TryParse(transferredQuantityObj.ToString(), out var transferredQuantity)) return;
            
                if (gridViewOrder.GetRow(gridViewOrder.FocusedRowHandle) is not TransferDetail row) return;

                scanCount = Math.Max(0, scanCount);

                var validCount = Math.Min(scanCount, transferredQuantity);

                e.Value = scanCount;

                var productChecked = await _transferService.GetCheckedProductByTransfer(
                    _transferResponse.Id,
                    _transferResponse.Code,
                    _branchId,
                    AppGlobals.UserInfo.UserName,
                    _transfer,
                    txtProductCode.Text,
                    row.ProductCode
                );

                if (productChecked == null)
                {
                    await _transferService.AddProductCheck(new TransferChecked()
                    {
                        TransferId = _transferResponse.Id,
                        TransferCode = _transferResponse.Code,
                        ProductBarCode = txtProductCode.Text, //Trường hợp barcode # productCode.
                        ProductCode = row.ProductCode,
                        BranchId = _branchId,
                        UserName = AppGlobals.UserInfo.UserName,
                        Checked = true,
                        Transfer = _transfer,
                        ScanCount = scanCount
                    });
                }
                else
                {
                    productChecked.ScanCount = scanCount;
                    await _transferService.UpdateProductCheck(_transferResponse.Id, productChecked);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình lấy dữ liệu", MsgType.Error);
            }
        }

        private void gridViewOrder_ShownEditor(object sender, EventArgs e)
        {
            var view = sender as GridView;
            var editor = gridViewOrder.ActiveEditor;
            if (editor == null) return;
            editor.KeyDown += gridViewEditor_KeyDown;
            view?.ActiveEditor?.Focus();
            view?.ActiveEditor?.SelectAll();
        }
        #endregion

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (_transferResponse != null && _scannedBarcodeCount == _transferResponse.Details.Count())
            {
                // Kiểm tra: nếu số thực nhận: ReceiveQuantity # TransferredQuantity thì không cho hoàn thành
                //Commnet lại sau khi thống nhất với Dũng: Nhỏ hơn SL chuyển cho nhận, vượt SL thì sao chép phiếu 
                //Phải huỷ phiếu gốc.
                var hasMismatch =
                    _transferResponse.Details.Any(p => (int)p.ScanCount != p.TransferredQuantity);
                if (hasMismatch)
                {
                    var productsMismatch = _transferResponse.Details
                        .Where(p => (int)p.ReceiveQuantity != p.TransferredQuantity)
                        .Select(p => p.ProductCode)
                        .ToList();

                    var firstMismatchRow = _transferResponse.Details
                        .FindIndex(p => (int)p.ReceiveQuantity != p.TransferredQuantity);
                    if (firstMismatchRow >= 0)
                    {
                        gridViewOrder.FocusedRowHandle = firstMismatchRow;
                        gridViewOrder.MakeRowVisible(firstMismatchRow);
                    }

                    MessageHelper.MsgBox(this,
                        $"Mã SP {string.Join(", ", productsMismatch)} có Số lượng thực kiểm không khớp với số lượng chuyển. Vui lòng kiểm tra lại.",
                        MsgType.Error);
                    return;
                }
                FinishOrder();
            }
            else
            {
                var listNotScan = _transferResponse.Details.Where(p => !p.Checked)
                    .Select(p => p.ProductCode)
                    .ToList();
                var message =
                    $"Còn {listNotScan.Count} sản phẩm chưa quét mã: {string.Join(", ", listNotScan)}.\nVui lòng thực hiện trước khi hoàn thành.";
                MessageHelper.MsgBox(this, message, MsgType.Error);
                txtProductCode.Focus();
            }
        }

        private (bool check, string code) TryFindProductCode(string searchBarCode)
        {
            return _productLookupDictionary.TryGetValue(searchBarCode.ToUpper(), out var codeValue) ? (true, codeValue) : (false, null);
        }

        private async void btnReloadOrder_Click(object sender, EventArgs e)
        {
            try
            {
                await ReLoadData(CurrentCode, CurrentId);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình xử lý đơn hàng.", MsgType.Error);
            }
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
                    MessageHelper.MsgBox(this, "Lỗi khi lấy dữ liệu Kiotviet", MsgType.Error);
                    return;
                }

                var transferResponse = JsonConvert.DeserializeObject<TransferResponse>(content);
                if (transferResponse == null)
                {
                    MessageHelper.MsgBox(this, "Dữ liệu đơn hàng trả về không hợp lệ", MsgType.Error);
                    return;
                }

                if (!CanComplete(transferResponse))
                {
                    MessageHelper.MsgBox(this, "Kiểm tra dữ liệu trước khi thực hiện", MsgType.Error);
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
                        receiveQuantity = _transfer ? product.TransferredQuantity : product.ReceiveQuantity,
                        price = product.Price
                    }).ToList(),
                };

                var (updateSuccess, updateContent) = await _kiotVietService.CallApiAsync(orderUrl, transferRequest, "PUT");

                if (!updateSuccess || string.IsNullOrEmpty(updateContent))
                {
                    MessageHelper.MsgBox(this, $"Có lỗi khi cập nhật đơn hàng: {updateContent}", MsgType.Error);
                    return;
                }

                MessageHelper.MsgBox(this, "Thao tác được thực hiện thành công.", MsgType.Information);
                await ReLoadData(CurrentCode, CurrentId);
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox(this, "Có lỗi trong quá trình xử lý đơn hàng.", MsgType.Error);
            }
            finally
            {
                SetControlEnable(true);
            }
        }

        private void SetControlEnable(bool enable)
        {
            if (layoutControlTop != null)
                layoutControlTop.Enabled = enable;
            if (gridControlOrder != null)
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
            ltCtlCode.Text = _transfer ? "Mã Phiếu Chuyển" : "Mã Phiếu Nhận";
            switch (transfer.Status)
            {
                case (int)TransferStatusEnum.Draft:
                    Text = _transfer ? "Xử lý Phiếu Chuyển hàng" : "Xử lý Phiếu Nhận hàng";
                    grpCtlFilter.Text = "Phiếu Chuyển hàng";
                    btnFinish.Text = _transfer ? "Chuyển hàng" : "Nhận hàng";
                    txtProductCode.ReadOnly = !_transfer;
                    break;

                case (int)TransferStatusEnum.Transferred:
                    Text = _transfer ? "Xử lý Phiếu Chuyển hàng" : "Xử lý Phiếu Nhận hàng";
                    grpCtlFilter.Text = _transfer ? "Phiếu Chuyển hàng" : "Phiếu Nhận hàng";
                    grpCtlFilter.Text = "Nhận hàng";
                    btnFinish.Text = "Nhận hàng";
                    txtProductCode.ReadOnly = _transfer;
                    break;

                case (int)TransferStatusEnum.Finished:
                case (int)TransferStatusEnum.Cancelled:
                    Text = _transfer ? "Xử lý Phiếu Chuyển hàng" : "Xử lý Phiếu Nhận hàng";
                    grpCtlFilter.Text = _transfer ? "Phiếu Chuyển hàng" : "Phiếu Nhận hàng";
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
                (int)TransferStatusEnum.Draft => _transfer,
                (int)TransferStatusEnum.Transferred => !_transfer,
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