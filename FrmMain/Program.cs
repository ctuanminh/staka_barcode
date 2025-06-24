using AutoMapper;
using Be.Core.Entities.Identity;
using Be.Data.Data;
using Be.Services;
using Be.Services.AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Windows.Forms;

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

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

            services.AddDbContext<Be.Data.Data.IdentityDbContext>(options => options.UseNpgsql(connectionString));
            
            services.AddIdentity<ApplicationUser, IdentityRole<long>>()
                .AddEntityFrameworkStores<Be.Data.Data.IdentityDbContext>()
                .AddRoleManager<RoleManager<IdentityRole<long>>>()
                .AddDefaultTokenProviders();


            services.AddSingleton<IConfiguration>(configuration);
            services.AddHttpClient();

            // extension method
            services.RegisterServices();
            // Build ServiceProvider
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AllowNullCollections = true;
                mc.AddProfile(new MappingProfile());
            });
            ServiceProvider = services.BuildServiceProvider();
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Thêm các form
            services.AddTransient<FrmOrder>();
            services.AddTransient<FrmOrderProcess>();
            services.AddSingleton<FrmMainF>();
            services.AddTransient<FrmSystem>(); 
            services.AddTransient<FrmLogin>(); 
            services.AddTransient<FrmPurchase>(); 
            services.AddTransient<FrmTransfer>(); 
            services.AddTransient<FrmPurchaseProcess>();
            services.AddTransient<FrmTransferProcess>();
            services.AddTransient<FrmReceiverList>();
            services.AddTransient<FrmAddPurchase>();
            // Auto Mapper Configurations
            

            // Xây dựng ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            // Khởi động ứng dụng với DI
            var mainForm = serviceProvider.GetRequiredService<FrmMainF>();
            Application.Run(mainForm);
        }
    }
}
