using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFramework.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Uploader>
    {
        public void Configure(EntityTypeBuilder<Uploader> builder)
        {
            builder.HasKey(item => item.Id);

            builder.Property(item => item.Id)
                .HasMaxLength(50)
                .ValueGeneratedNever();

            builder.HasIndex(item => item.UserName)
                .IsUnique();

            builder.Property(item => item.UserName)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(item => item.Email)
                .IsUnique();

            builder.Property(item => item.Email)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
