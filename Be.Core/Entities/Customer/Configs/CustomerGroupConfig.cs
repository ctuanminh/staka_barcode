using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Be.Core.Entities.Customer.Configs
{
    public class CustomerGroupConfig : IEntityTypeConfiguration<CustomerGroup>
    {
        public void Configure(EntityTypeBuilder<CustomerGroup> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
            builder.Property(g => g.Description).HasMaxLength(500);

            builder.HasMany(g => g.CustomerGroupDetails)
                   .WithOne(d => d.CustomerGroup)
                   .HasForeignKey(d => d.GroupId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
