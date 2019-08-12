using Infrastructure.CustomIdentity.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CustomIdentity.EntityFramework
{
    /// <summary>
    /// Dont forget to add db context in web application
    /// </summary>
    public class CustomIdentityDbContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }

        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> options) : base(options) { }

        /// <summary>
        /// Configure CustomIdentityDbContext models
        /// </summary>
        /// <param name="builder">Instance of ModelBuilder</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new AppRoleConfiguration());
        }
    }
}
