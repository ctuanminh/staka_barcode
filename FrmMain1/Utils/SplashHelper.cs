using DevExpress.XtraSplashScreen;
using System;
using System.Windows.Forms;

namespace FrmMain.Utils
{
    public static class SplashHelper
    {
        public static void ShowSplash(Form parent, Type splashType)
        {
            if (!(SplashScreenManager.Default?.IsSplashFormVisible ?? false))
            {
                SplashScreenManager.ShowForm(parent, splashType, true, true);
            }
        }

        public static void CloseSplash()
        {
            if (SplashScreenManager.Default?.IsSplashFormVisible ?? false)
            {
                SplashScreenManager.CloseForm();
            }
        }
    }
}
