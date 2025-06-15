using Be.Core.Entities;
using Be.Core.Entities.Crm;
using Be.Core.Entities.Customer;
using Be.Core.Entities.Customer.Configs;
using Be.Core.Entities.Identity;
using Be.Core.Entities.KiotViet;
using Be.Core.Gateway;
using Microsoft.EntityFrameworkCore;

namespace Be.Data.Data
{
	public class AppDbContext : DbContext
	{
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<ApiLog> ApiLogs { get; set; }
        public DbSet<WorkQueue> WorkQueues { get; set; }
        public DbSet<Card> Card { get; set; }
        public DbSet<TaskList> TaskList { get; set; }
        public DbSet<CardAssignment> CardAssignment { get; set; }
        public DbSet<Department> Department { get; set; }        
        public DbSet<Customer> Customers { get; set; }        
        public DbSet<CustomerGroup> CustomerGroups { get; set; }        
        public DbSet<CustomerGroupDetail> CustomerGroupDetails { get; set; }        
        public DbSet<BankAccountEntity> BankAccounts { get; set; }        
        //Chưa cần tạo db
        //public DbSet<BankAccount> BankAccounts { get; set; }        

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{	
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema("Catalog");
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new CustomerGroupConfig());
            modelBuilder.ApplyConfiguration(new CustomerGroupDetailConfig());
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<ApplicationUser>();            
        }
	}
}
