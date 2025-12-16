using Be.Core.Entities.InvoiceList;
using Microsoft.EntityFrameworkCore;

namespace Be.Data.Data
{
	public class LocalDbContext : DbContext
	{
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<TaxAccountConfig> TaxAccountConfigs { get; set; }     

        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options)
		{	
		}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=invoice_local.db");
        }

	}
}
