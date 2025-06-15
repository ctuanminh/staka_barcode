using Be.Core.Entities;
using Be.Core.Entities.Crm;
using Be.Core.Entities.Identity;
using Be.Infrastructure.Converters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Be.Data.Data
{
    /// <summary>
    /// IdentityDbContext được asp.net identity cung cấp để
    /// làm việc với csdl, lưu trữ thông tin user, role, đăng nhập ...
    /// </summary>    
    public class IdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
        {
        }

        public DbSet<SubRole> SubRoles { get; set; }
        public DbSet<UserSubRole> UserSubRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRoleClaim<long>>(entity =>
            {
                entity.ToTable("RoleClaims", "Identity");
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(255);
            });
            builder.Entity<SubRole>(entity => { entity.ToTable("SubRole", "Identity"); });

            builder.Entity<UserSubRole>(entity => { entity.ToTable("UserSubRole", "Identity"); });

            builder.Entity<IdentityRole<long>>(entity =>
            {
                entity.ToTable("Roles", "Identity");                
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).HasMaxLength(300);
                entity.Property(e => e.NormalizedName).HasMaxLength(300);
            });

            builder.Entity<IdentityUserClaim<long>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            builder.Entity<IdentityUserLogin<long>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.ToTable("UserLogins", "Identity");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            builder.Entity<IdentityUserRole<long>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("UserRoles", "Identity");
            });

            builder.Entity<IdentityUserToken<long>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.ToTable("UserTokens", "Identity");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users", "Identity");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.Property(x => x.EmployeeRoles).HasConversion<StringListConverter>();
            });

            builder.Ignore<Card>();
            builder.Ignore<TaskList>();
            builder.Ignore<CardAssignment>();
            builder.Ignore<Department>();
        }
    }
}