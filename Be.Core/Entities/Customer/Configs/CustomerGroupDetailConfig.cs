using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Be.Core.Entities.Customer.Configs
{
    public class CustomerGroupDetailConfig : IEntityTypeConfiguration<CustomerGroupDetail>
    {
        public void Configure(EntityTypeBuilder<CustomerGroupDetail> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.Customer)
                   .WithMany(c => c.CustomerGroupDetails)
                   .HasForeignKey(d => d.CustomerId);

            builder.HasOne(d => d.CustomerGroup)
                   .WithMany(g => g.CustomerGroupDetails)
                   .HasForeignKey(d => d.GroupId);
        }
    }
}
