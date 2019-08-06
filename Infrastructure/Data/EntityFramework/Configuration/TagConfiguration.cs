using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFramework.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(item => item.Description)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(item => item.Image)
                .WithMany(image => image.Tags)
                .HasForeignKey(item => item.ImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
