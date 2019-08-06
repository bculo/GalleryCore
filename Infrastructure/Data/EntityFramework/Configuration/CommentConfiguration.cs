using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFramework.Configurations
{
    class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(item => item.Description)
                .IsRequired();

            builder.Property(item => item.Created)
                .IsRequired();

            builder.HasOne(item => item.Image)
                .WithMany(image => image.Comments)
                .HasForeignKey(item => item.ImageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(item => item.User)
                .WithMany(user => user.Comments)
                .HasForeignKey(item => item.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
