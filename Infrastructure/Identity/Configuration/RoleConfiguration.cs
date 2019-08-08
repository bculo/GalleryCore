using ApplicationCore.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.IdentityData.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<GalleryRole>
    {
        public void Configure(EntityTypeBuilder<GalleryRole> builder)
        {
            builder.Property(item => item.Id)
                .ValueGeneratedNever();

            List<GalleryRole> identityRoles = Role.GetRoles().Select(item => new GalleryRole
            {
                Id = item.ToString(),
                Name = item.Name,
                NormalizedName = item.Name.ToUpper()
            }).ToList();

            builder.HasData(identityRoles);
        }
    }
}
