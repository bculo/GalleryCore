using Infrastructure.CustomIdentity.Claim;
using Infrastructure.CustomIdentity.EntityFramework;
using Infrastructure.CustomIdentity.Interfaces;
using Infrastructure.CustomIdentity.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.CustomIdentity.Extensions
{
    /// <summary>
    /// Custom identity extension
    /// </summary>
    public static class CustomIdentityExtension
    {
        /// <summary>
        /// Add services for custom identity
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomIdentity(this IServiceCollection services)
        {
            services.AddDataProtection(); // now we can use IDataProtection

            services.AddTransient<IHasher, PasswordHasher>();
            services.AddTransient<IClaimMaker, ClaimMaker>();
            services.AddScoped<IUserManager, UserManager>();
        }
    }
}
