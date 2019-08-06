using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Configuration
{
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Configure automapper
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
