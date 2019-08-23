using ApplicationCore.Helpers.Generator;
using ApplicationCore.Helpers.Images;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Data.EntityFramework;
using Infrastructure.Data.EntityFramework.Repository;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
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
        /// <param name="configuration">IConfiguration instance</param>
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Core project
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPaginationMaker, PaginationMaker>();
            services.AddTransient<IUniqueStringGenerator, GuidStringGenerator>();
            services.AddTransient<IImageNameGenerator, ImageNameGenerator>();
            services.AddScoped<IImageService, ImageService>();

            //Infrastracture project
            services.AddScoped(typeof(ISpecificationEvaluator<>), typeof(EfSpecificationEvaluator<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            if (configuration.UsingIdentity())
            {
                services.AddScoped<IAuthenticationService, IdentityAuthenticationService>();
            }
            else
            {
                services.AddScoped<IAuthenticationService, HttpContextAuthenticationService>();
            }

            //Web project
            services.AddScoped<IUrlGenerator, UrlGenerator>();
        }
    }
}
