using System.Windows.Forms;
using static FrmMain.FrmMainF;

namespace FrmMain.Utils
{
    public static class FormHelper
    {
        public static void NewFormNew(Form mdiParent, Form f, WuserControl wuser, string fName = "")
        {
            if (!string.IsNullOrEmpty(fName)) f.Name = fName;
            f.AccessibleDescription = wuser.ToString();
            f.MdiParent = mdiParent;
            f.Dock = DockStyle.Fill;
            f.BringToFront();
            f.Show();
        }
        public static bool OpenedForm(string fName, WuserControl parent, out Form openForm)
        {
            openForm = Application.OpenForms[fName];
            if (openForm == null)
            {
                return false;
            }

            if (parent == WuserControl.None || openForm.AccessibleDescription == parent.ToString())
            {
                openForm.BringToFront();
                return true;
            }

            openForm.Close();
            return false;
        }
        public static bool OpenedKeyForm(string formName, string tabKey, out Form openForm)
        {
            openForm = null;

            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == formName && frm.AccessibleDescription == tabKey)
                {
                    openForm = frm;
                    openForm.BringToFront();
                    return true;
                }
            }

            return false;
        }
        public static void NewManyForm(Form mainForm, Form formToOpen, WuserControl wuser, string tabKey = "")
        {
            formToOpen.MdiParent = mainForm;
            formToOpen.Name = formToOpen.GetType().Name;
            formToOpen.AccessibleDescription = tabKey;
            formToOpen.Dock = DockStyle.Fill;
            formToOpen.Show();
        }
    }
}
