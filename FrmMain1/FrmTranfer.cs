using Be.Common.Tranfer.Request;
using Be.Common.Tranfer.Response;
using Be.Core.Entities;
using Be.Services.KiotViet;
using Be.Services.Pos;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using FrmMain.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static FrmMain.FrmMainF;
using Exception = System.Exception;

namespace FrmMain
{
    public partial class FrmTranfer : XtraForm
    {
        private readonly FrmMainF _mainForm;
        private readonly IKiotVietService _kiotVietService;
        private const string TranferUrl = "https://public.kiotapi.com/transfers";
        private List<int> _statusList;
        private readonly IBranchService _branchService;
        private int _branchId = 631782;
        private List<Branch> branches;
        public FrmTranfer(FrmMainF mainForm, IKiotVietService kiotVietService, IBranchService branchService)
        {
            _mainForm = mainForm;
            _kiotVietService = kiotVietService;
            _branchService = branchService;
            InitializeComponent();
        }

        private async void FrmOrder_Shown(object sender, EventArgs e)
        {
            _statusList = [1];
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(LoadingForm), true, true);
                SplashScreenManager.Default.SetWaitFormCaption("Đang lấy tải dữ liệu");
                SplashScreenManager.Default.SetWaitFormDescription("Vui lòng đợi...");
                layoutControlTop.Enabled = false;
                grdControlOrders.Enabled = false;
                var request = new SearchTranferRequest()
                {
                    FromBranchIds = [_branchId],
                    Status = _statusList.ToArray(),
                    //FromTransferDate = new DateTime(2025, 6, 1),
                    //ToTransferDate = new DateTime(2025, 6, 30),
                    //FromReceivedDate = new DateTime(2025, 6, 1),
                    //ToReceivedDate = new DateTime(2025, 6, 30),
                    PageSize = 100,
                    CurrentItem = 1
                };

                var (success, content) = await _kiotVietService.CallApiAsync(TranferUrl, request, "GET");

                if (!success || content == null) return;
                var tranferPagedResponse = JsonConvert.DeserializeObject<TranferPagedResponse>(content);

                var branches = await _branchService.GetAllBranches();
                foreach (var transfer in tranferPagedResponse.Data)
                {
                    var fromBranch = branches.FirstOrDefault(b => b.BranchId == transfer.FromBranchId);
                    var toBranch = branches.FirstOrDefault(b => b.BranchId == transfer.ToBranchId);

                    transfer.FromBranchName = fromBranch != null ? fromBranch.BranchName : "";
                    transfer.ToBranchName = toBranch != null ? toBranch.BranchName : "";
                }
                grdControlOrders.DataSource = tranferPagedResponse.Data;
            }
            catch (Exception exception)
            {
                MessageHelper.MsgBox("Lỗi gọi API: " + exception, MsgType.Error_);
            }
            finally
            {
                // Ẩn màn hình chờ
                SplashScreenManager.CloseForm();
                layoutControlTop.Enabled = true;
                grdControlOrders.Enabled = true;
            }
        }

        private void grdViewOrders_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (sender is not GridView { FocusedRowHandle: >= 0 } view) return;
                var code = view.GetRowCellValue(view.FocusedRowHandle, "Code");
                if (code == null) return;
                {
                    if (FormHelper.OpenedForm(nameof(FrmOrderProcess), WuserControl.Order, out var openForm))
                    {
                        if (openForm is FrmOrderProcess processForm)
                        {
                            processForm.ReloadData(code.ToString());
                        }
                    }
                    else
                    {
                        FrmOrderProcess.CurrentCode = code.ToString();
                        var frmOrderInstance = _mainForm.ServiceProvider.GetRequiredService<FrmOrderProcess>();
                        Form frmOrder = frmOrderInstance;
                        FormHelper.NewFormNew(_mainForm, frmOrder, WuserControl.Order, nameof(FrmOrderProcess));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.MsgBox("Lỗi khi chuyển dữ liệu", MsgType.Error_);
            }
        }

        private void SetTextEditHeight(Control control, int height)
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

        private async void FrmOrder_Load(object sender, EventArgs e)
        {
            SetTextEditHeight(this, 25);
            branches = await _branchService.GetAllBranches();
            lkupFromBranch.Properties.DataSource = branches;
            lkupToBranch.Properties.DataSource = branches;
            lkupFromBranch.Properties.AutoHeight = false;
            lkupToBranch.Properties.AutoHeight = false;
            lkupFromBranch.Height = 45;
            lkupToBranch.Height = 45;
            chkFinish.BackColor = Color.LightGreen;
            chkDraft.BackColor = Color.Green;
            chkDraft.ForeColor = Color.White;
            chkCancel.BackColor = Color.OrangeRed;
            chkCancel.ForeColor = Color.White;
            chkStatusTranfer.BackColor = Color.Cyan;
            chkStatusTranfer.ForeColor = Color.Black;

            //Set ngày mặc định
            chkFromTranfer.Checked = true; //Check Ngày chuyển
            chkFromReceived.Checked = true; // Check Ngày nhận
            var dateNow = DateTime.Now.Year;
            fromDate.Text = 
        }

        private void Handler_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is not CheckEdit checkEdit) return;

            var statusValue = checkEdit.Name switch
            {
                "chkDraft" => 1,
                "chkTranfer" => 2,
                "chkFinish" => 3,
                "chkCancel" => 4,
                _ => 0
            };

            if (statusValue == 0) return;

            if (checkEdit.Checked)
            {
                if (!_statusList.Contains(statusValue))
                    _statusList.Add(statusValue);
            }
            else
            {
                _statusList.Remove(statusValue);
                if (_statusList.Count == 0)
                {
                    chkDraft.CheckedChanged -= Handler_CheckedChanged;
                    chkDraft.Checked = true;
                    _statusList.Add(1);
                    chkDraft.CheckedChanged += Handler_CheckedChanged;
                }
            }
            LoadData();
        }

        private void lkupBranch_EditValueChanged(object sender, EventArgs e)
        {
            var branchId = (int)lkupFromBranch.EditValue;
            _branchId = branchId;
            LoadData();
        }
    }
}