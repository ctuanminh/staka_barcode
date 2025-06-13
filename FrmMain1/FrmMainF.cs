using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;

namespace FrmMain
{
    public partial class FrmMainF : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        public FrmMainF(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            InitializeComponent();
        }
        public bool OpenedForm(string fName, WuserControl parent)
        {
            Form openForm = Application.OpenForms[fName];
            if (openForm == null)
            {
                return false;
            }
            else
            {
                if (parent == WuserControl.none || openForm.AccessibleDescription == parent.ToString())
                {
                    openForm.BringToFront();
                    return true;
                }
                else
                {
                    openForm.Close();
                    return false;
                }
            }
        }
        public enum WuserControl
        {
            none = 0,
            order = 1,
            orderProccess = 2
        }


        private void FormActive(object sender, EventArgs e)
        {
            if (sender is Form frm)
            {
                frm.Refresh();

                if (frm.Tag is NavBarItem navItem)
                {
                    if (navItem.Links.Count > 0)
                    {
                        navItem.Links[0].PerformClick();
                    }

                    if (navItem.NavBar != null && navItem.NavBar.Parent != null)
                    {
                        navItem.NavBar.Parent.BringToFront();
                    }
                }
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var httpClient = _httpClientFactory.CreateClient();
            switch (e.Item.Name)
            {
                case nameof(btnOrder):
                    if (!OpenedForm(nameof(FrmOrder), WuserControl.order))
                    {
                        Form frmOrder = new FrmOrder(_httpClientFactory, httpClient, _config);
                        NewFormNew(ref frmOrder, WuserControl.order);
                    }
                    break;
                //case nameof(btnOrderProccess):
                //    if (!OpenedForm(nameof(FrmOrder), WuserControl.order))
                //    {
                //        Form frmOrder = new FrmOrder();
                //        NewFormNew(ref frmOrder, WuserControl.order);
                //    }
                //    break;
                default:
                    break;
            }
        }

        public void NewFormNew(ref Form f, WuserControl Wuser, string fName = "")
        {
            if (!string.IsNullOrEmpty(fName)) f.Name = fName;
            f.AccessibleDescription = Wuser.ToString();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.BringToFront();
            f.Show();
        }

        private void FrmMainF_Load(object sender, EventArgs e)
        {
            bool isOrderFormOpened = this.MdiChildren.Any(f => f is FrmOrder);
            if (!isOrderFormOpened)
            {
                var httpClient = _httpClientFactory.CreateClient();
                Form frmOrder = new FrmOrder(_httpClientFactory, httpClient, _config);

                frmOrder.MdiParent = this;
                
                frmOrder.Show();
            }
        }
    }
}