using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;
using FrmMain.Utils;

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
            if (password == "!@##@!")
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageHelper.MsgBox(this,"Không có quyền truy cập", MsgType.Error);
            }
        }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            btnSubmit_Click(btnSubmit, EventArgs.Empty);
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

        private void FrmAdmin_Load(object sender, EventArgs e)
        {
            SetTextEditHeight(this, 25);
        }
    }
}