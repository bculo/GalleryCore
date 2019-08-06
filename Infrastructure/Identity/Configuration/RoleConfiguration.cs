using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.IdentityData.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            List<IdentityRole> identityRoles = Role.GetRoles().Select(item =>
            new IdentityRole()
            {
                Name = item.Name,
                NormalizedName = item.Name.ToUpper()
            }).ToList();

            builder.HasData(identityRoles);
        }
    }
}
