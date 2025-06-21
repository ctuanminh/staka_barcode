using System.Collections.Generic;
using Be.Common.System;

namespace FrmMain.App
{
    public static class AppGlobals
    {
        public static ICollection<AppSettingDto>  AppSetting { get; set; }
        public static UserInfo UserInfo { get; set; }
        public static int BranchId { get; set; }
        public static string? ComputerName { get; set; }
    }
}
