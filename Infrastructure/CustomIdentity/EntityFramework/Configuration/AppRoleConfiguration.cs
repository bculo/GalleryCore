using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.CustomIdentity.EntityFramework.Configuration
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasKey(item => item.Id);

            builder.Property(item => item.Id)
                .ValueGeneratedNever();

            builder.Property(item => item.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(item => item.Name)
                .IsUnique();

            List<AppRole> identityRoles = Role.GetRoles().Select(item => new AppRole
            {
                Id = item.Id,
                Name = item.Name,
            }).ToList();

            builder.HasData(identityRoles);
        }
    }
}
