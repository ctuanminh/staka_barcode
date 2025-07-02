using DevExpress.XtraEditors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
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
        public static void ShowManyForm(Form mainForm, Form formToOpen, WuserControl wuser, string tabKey = "")
        {
            formToOpen.MdiParent = mainForm;
            formToOpen.Name = formToOpen.GetType().Name;
            formToOpen.AccessibleDescription = tabKey;
            formToOpen.Dock = DockStyle.Fill;
            formToOpen.Show();
        }

        //Mở 1 Form duy nhất
        public static async Task OpenFormWithScope<TForm>(
            Form mdiParent, IServiceProvider provider,
            string code, long id,
            string tabKey, WuserControl tabEnum)
            where TForm : FrmBasePos, IReloadableForm
        {
            if (OpenedForm(typeof(TForm).Name, tabEnum, out var openForm))
            {
                if (openForm is TForm reloadable)
                {
                    await reloadable.ReLoadData(code, id);
                }
                return;
            }

            var scope = provider.CreateScope();
            var form = scope.ServiceProvider.GetRequiredService<TForm>();
            form.Tag = scope;
            await form.ReLoadData(code, id);

            form.FormClosed += (s, _) =>
            {
                if (s is Form { Tag: IServiceScope scopeToDispose })
                    scopeToDispose.Dispose();
            };

            NewFormNew(mdiParent, form, tabEnum, typeof(TForm).Name);
        }
        //Mở nhiều Form thêm mới.
        public static async Task OpenManyFormWithScope<TForm>(
            Form mdiParent, IServiceProvider provider,
            string code, long id,
            string tabKeyPrefix, WuserControl tabEnum)
            where TForm : XtraForm, IReloadableForm
        {
            var scope = provider.CreateScope();
            var form = scope.ServiceProvider.GetRequiredService<TForm>();
            form.Tag = scope;
            await form.ReLoadData(code, id);

            form.FormClosed += (_, _) =>
            {
                if (form.Tag is IServiceScope scopeToDispose)
                    scopeToDispose.Dispose();
            };

            var tabKey = $"{tabKeyPrefix}_{Guid.NewGuid():N}";

            ShowManyForm(mdiParent, form, tabEnum, tabKey);
        }
    }
}
