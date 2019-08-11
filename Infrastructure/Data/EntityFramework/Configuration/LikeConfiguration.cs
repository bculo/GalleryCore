using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFramework.Configurations
{
    class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(item => new
            {
                item.UserId,
                item.ImageId
            });

            builder.HasOne(item => item.Image)
                .WithMany(image => image.Likes)
                .HasForeignKey(item => item.ImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(item => item.User)
                .WithMany(user => user.LikedImages)
                .HasForeignKey(item => item.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(item => item.Created)
                .IsRequired();

            builder.Property(item => item.Liked)
                .HasDefaultValue(true)
                .IsRequired();
        }
    }
}
