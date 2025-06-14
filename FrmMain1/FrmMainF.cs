using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;
using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace FrmMain
{
    public partial class FrmMainF : RibbonForm
    {
        private Timer _clockTimer;
        public IServiceProvider ServiceProvider { get; }
        public FrmMainF(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            InitializeComponent();
        }
        public bool OpenedForm(string fName, WuserControl parent)
        {
            var openForm = Application.OpenForms[fName];
            if (openForm == null)
            {
                return false;
            }

            if (parent == WuserControl.none || openForm.AccessibleDescription == parent.ToString())
            {
                openForm.BringToFront();
                return true;
            }

            openForm.Close();
            return false;
        }
        public enum WuserControl
        {
            none = 0,
            order = 1,
            orderProccess = 2
        }

        private void FormActive(object sender, EventArgs e)
        {
            if (sender is not Form frm) return;
            frm.Refresh();

            if (frm.Tag is not NavBarItem navItem) return;
            if (navItem.Links.Count > 0)
            {
                navItem.Links[0].PerformClick();
            }

            if (navItem.NavBar is { Parent: not null })
            {
                navItem.NavBar.Parent.BringToFront();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case nameof(btnOrder):
                    if (!OpenedForm(nameof(FrmOrder), WuserControl.order))
                    {
                       var frmOrder =  ServiceProvider.GetRequiredService<FrmOrder>();
                        NewFormNew(frmOrder, WuserControl.order);
                    }
                    break;
                default:
                    break;
            }
        }

        public void NewFormNew(Form f, WuserControl wuser, string fName = "")
        {
            if (!string.IsNullOrEmpty(fName)) f.Name = fName;
            f.AccessibleDescription = wuser.ToString();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.BringToFront();
            f.Show();
        }

        private void FrmMainF_Load(object sender, EventArgs e)
        {
            _clockTimer = new Timer();
            _clockTimer.Interval = 1000; // 1 giây
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();
            var isOrderFormOpened = this.MdiChildren.Any(f => f is FrmOrder);
            if (isOrderFormOpened) return;
            var frmOrder = ServiceProvider.GetRequiredService<FrmOrder>();
            frmOrder.MdiParent = this;
            frmOrder.Show();
        }
        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            lblTimer.Caption = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }
    }
}