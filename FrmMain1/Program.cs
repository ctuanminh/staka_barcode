using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace FrmMain
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Cấu hình DI
            var services = new ServiceCollection();
            // Thêm IConfiguration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Application.StartupPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddHttpClient();
            // Thêm các form
            services.AddTransient<FrmOrder>();
            services.AddTransient<FrmOrderProcess>();
            services.AddSingleton<FrmMainF>(); // Singleton cho MDI Parent
            services.AddSingleton<IKiotVietService, KiotVietServiceImp>();

            // Xây dựng ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            // Khởi động ứng dụng với DI
            var mainForm = serviceProvider.GetRequiredService<FrmMainF>();
            Application.Run(mainForm);
        }
    }
}
