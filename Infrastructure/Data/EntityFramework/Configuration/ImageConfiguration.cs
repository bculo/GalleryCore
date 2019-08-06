using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFramework.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(item => item.Id);

            builder.Property(item => item.Id)
                .ValueGeneratedOnAdd();

            builder.Property(item => item.Url)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(item => item.Description)
                .HasMaxLength(200);

            builder.HasOne(item => item.User)
                .WithMany(user => user.UploadedImages)
                .HasForeignKey(item => item.UserId)
                .IsRequired();

            builder.HasOne(item => item.Category)
                .WithMany(category => category.Images)
                .HasForeignKey(item => item.CategoryId)
                .IsRequired();
        }
    }
}
