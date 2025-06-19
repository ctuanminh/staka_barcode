using Be.Data.BaseApp;
using Be.Data.Data;
using Be.Data.Repository;
using Be.Services.CashFlow;
using Be.Services.Catalog;
using Be.Services.Crm;
using Be.Services.customer;
using Be.Services.Identity;
using Be.Services.KiotViet;
using Be.Services.Order;
using Be.Services.Pos;
using Be.Services.PurchaseOrder;
using Be.Services.System;
using Microsoft.Extensions.DependencyInjection;

namespace Be.Services
{
    public static class ServiceCollectionExtensions
	{
		public static IServiceCollection RegisterServices(this IServiceCollection services)
		{
			services.AddDistributedMemoryCache();
			services.AddScoped<IRepository, Repository<AppDbContext>>();
			services.AddScoped<IProductService, ProductServiceImp>();
			services.AddScoped<IUserService, UserServiceImp>();
			services.AddScoped<IJwtService, JwtServiceImp>();
			services.AddScoped<ICategoryService, CategoryServiceImp>();
			services.AddScoped<ICurrentUserService, CurrentUserServiceImp>();
			services.AddScoped<ICustomerService, CustomerServiceImp>();
			services.AddScoped<IOrderService, OrderServiceImp>();
			services.AddScoped<ICashFlowService, CashFlowServiceImp>();
			services.AddScoped<Gateway.IKiotVietService, Gateway.KiotVietServiceImp>();
			services.AddScoped<IReportService, ReportServiceImp>();
			services.AddScoped<IBranchService, BranchServiceImp>();
			services.AddScoped<IPurchaseOrderService, PurchaseOrderServiceImp>();
			services.AddScoped<IKiotVietService, KiotVietServiceImp>();
			services.AddScoped<ISystemService, SystemServiceImp>();
			return services;
		}
	}
}
