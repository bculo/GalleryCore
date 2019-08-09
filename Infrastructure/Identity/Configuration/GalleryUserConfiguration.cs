using Infrastructure.IdentityData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Identity.Configuration
{
    public class GalleryUserConfiguration : IEntityTypeConfiguration<GalleryUser>
    {
        public void Configure(EntityTypeBuilder<GalleryUser> builder)
        {
            builder.Ignore(item => item.UserId);
            builder.Ignore(item => item.UserMail);
        }
    }
}
