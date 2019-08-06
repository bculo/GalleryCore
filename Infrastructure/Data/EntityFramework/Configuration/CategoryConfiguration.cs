using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFramework.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(item => item.Id);

            builder.Property(item => item.Id)
                .ValueGeneratedOnAdd();

            builder.Property(item => item.Url)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(item => item.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
