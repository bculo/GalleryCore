using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using ApplicationCore.Services.Helpers;
using Infrastructure.Data.EntityFramework;
using Infrastructure.Data.EntityFramework.Repository;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Web.Interfaces;
using Web.Services;

namespace Web.Configuration
{
    public static class ServicesConfiguration
    {
        /// <summary>
        /// Configure dependencies
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        public static void ConfigureServices(this IServiceCollection services)
        {
            //Core project
            services.AddScoped<IUniqueStringGenerator, GuidStringGenerator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ICategoryService, CategoryService>();

            //Infrastracture project
            services.AddScoped(typeof(ISpecificationEvaluator<>), typeof(EfSpecificationEvaluator<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.AddScoped<IAuthenticationService, IdentityAuthenticationService>();

            //Web project
            services.AddScoped<IUrlGenerator, UrlGenerator>();
        }
    }
}
