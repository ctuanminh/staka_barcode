using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Be.Core.Entities.Customer.Configs
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(255);
            builder.Property(c => c.ContactNumber).HasMaxLength(50);
            builder.Property(c => c.SubNumber).HasMaxLength(50);
            builder.Property(c => c.IdentificationNumber).HasMaxLength(50);
            builder.Property(c => c.Address).HasMaxLength(500);
            builder.Property(c => c.LocationName).HasMaxLength(255);
            builder.Property(c => c.Email).HasMaxLength(255);
            builder.Property(c => c.Organization).HasMaxLength(255);
            builder.Property(c => c.TaxCode).HasMaxLength(100);
            builder.Property(c => c.Comments).HasMaxLength(1000);
            builder.HasMany(c => c.CustomerGroupDetails) // Thiết lập quan hệ một-nhiều với CustomerGroupDetail
                   .WithOne(d => d.Customer) // Chỉ định thuộc tính điều hướng
                   .HasForeignKey(d => d.CustomerId) // Khóa ngoại trong CustomerGroupDetail
                   .OnDelete(DeleteBehavior.Cascade); // Xóa chi tiết nhóm khi xóa khách hàng
        }
    }
}
