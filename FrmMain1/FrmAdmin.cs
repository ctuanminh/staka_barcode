using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace FrmMain
{
    public partial class FrmAdmin : XtraForm
    {
        public FrmAdmin()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var password = txtPassword.Text;
            if (password == "Tr0ngMynh132!@##@!")
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}