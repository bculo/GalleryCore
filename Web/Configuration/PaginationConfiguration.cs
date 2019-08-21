using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Configuration
{
    public static class PaginationConfiguration
    {
        public static void ConfigurePagination(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PaginationSettings>(configuration.GetSection("Pagination")); 
        }
    }
}
