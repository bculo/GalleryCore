using Infrastructure.CustomIdentity.EntityFramework;
using Infrastructure.Data.EntityFramework;
using Infrastructure.IdentityData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Web.Configuration
{
    public static class DatabaseConfiguration
    {
        /// <summary>
        /// Set up a database and resilient connection
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        /// <param name="configuration">IConfiguration instance</param>
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            //Read settings from appsettings.json for resilient connection
            var settings = configuration.GetSection("ResilientConnection").Get<ResilientConnectionSqlServer>();

            //Gallery database
            services.AddDbContext<ImageGalleryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: settings.MaxRetryCount,
                        maxRetryDelay: TimeSpan.FromSeconds(settings.MaxRetryDelay),
                        errorNumbersToAdd: settings.ErrorNumbersToAdd);
                })
            );

            //Identity database
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: settings.MaxRetryCount,
                        maxRetryDelay: TimeSpan.FromSeconds(settings.MaxRetryDelay),
                        errorNumbersToAdd: settings.ErrorNumbersToAdd);
                })
            );

            //Custom identity database
            services.AddDbContext<CustomIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CustomIdentityConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: settings.MaxRetryCount,
                        maxRetryDelay: TimeSpan.FromSeconds(settings.MaxRetryDelay),
                        errorNumbersToAdd: settings.ErrorNumbersToAdd);
                })
            );
        }
    }
}
