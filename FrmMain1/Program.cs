using System;
using System.Windows.Forms;
using Be.Core.Entities.Identity;
using Be.Data.Data;
using Be.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace FrmMain
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }
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
            var connectionString = configuration.GetConnectionString("Default");
            services.AddDbContext<IdentityDbContext>(options => options.UseNpgsql(connectionString));
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole<long>>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddRoleManager<RoleManager<IdentityRole<long>>>();


            services.AddSingleton<IConfiguration>(configuration);
            services.AddHttpClient();

            // extension method
            services.RegisterServices();
            // Build ServiceProvider
            ServiceProvider = services.BuildServiceProvider();
            // Thêm các form
            services.AddTransient<FrmOrder>();
            services.AddTransient<FrmOrderProcess>();
            services.AddSingleton<FrmMainF>();
            services.AddSingleton<FrmSystem>(); 
            services.AddSingleton<IKiotVietService, KiotVietServiceImp>();

            // Xây dựng ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            // Khởi động ứng dụng với DI
            var mainForm = serviceProvider.GetRequiredService<FrmMainF>();
            Application.Run(mainForm);
        }
    }
}
