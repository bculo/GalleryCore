using Infrastructure.CustomIdentity.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CustomIdentity.EntityFramework
{
    public class CustomIdentityDbContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }

        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new AppRoleConfiguration());
        }
    }
}
