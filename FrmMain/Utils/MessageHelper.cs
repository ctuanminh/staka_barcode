using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace FrmMain.Utils
{
    public enum MsgType
    {
        Error_,
        Information,
        YesNo
    }
    public static class MessageHelper
    {
        public static DialogResult MsgBox(string msgContent, MsgType type)
        {
            return type switch
            {
                MsgType.Error_ => XtraMessageBox.Show(msgContent, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1),
                MsgType.Information => XtraMessageBox.Show(msgContent, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1),
                MsgType.YesNo => XtraMessageBox.Show(msgContent, "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1),
                _ => XtraMessageBox.Show(msgContent, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1),
            };
        }
    }
}
