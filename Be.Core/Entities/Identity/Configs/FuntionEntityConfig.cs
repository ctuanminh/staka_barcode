using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Be.Core.Entities.Identity.Configs
{
    public class FuntionEntityConfig : IEntityTypeConfiguration<FunctionEntity> 
    {
        public void Configure(EntityTypeBuilder<FunctionEntity> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(f => f.Name).IsRequired().HasMaxLength(255);
            builder.Property(f => f.ParentId).HasDefaultValue(0);
            builder.Property(f => f.IsDeleted).HasDefaultValue(0);
        }
    }
}
