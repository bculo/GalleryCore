using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.CustomIdentity.EntityFramework.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(item => item.Id);

            builder.Property(item => item.Id)
                .ValueGeneratedNever();

            builder.Property(item => item.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(item => item.Email)
                .IsUnique();

            builder.Property(item => item.UserName)
                .HasMaxLength(25)
                .IsRequired();

            builder.HasIndex(item => item.UserName)
                .IsUnique();

            builder.Property(item => item.EmailConfirmed)
                .HasDefaultValue(false);

            builder.Property(item => item.IsExternal)
                .HasDefaultValue(false);

            builder.Property(item => item.PasswordHash)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(item => item.SecurityStamp)
                .HasMaxLength(40)
                .IsRequired();

            builder.Ignore(item => item.UserId);

            builder.Ignore(item => item.UserMail);

            builder.HasOne(item => item.AppRole)
                    .WithMany(approle => approle.Users)
                    .HasForeignKey(item => item.AppRoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
