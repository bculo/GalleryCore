using Infrastructure.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Configuration
{
    public static class MailConfiguration
    {
        /// <summary>
        /// Prepare options for sending mails
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        /// <param name="configuration">IConfiguration instance</param>
        public static void ConfigureMail(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailOptions>(configuration);
        }
    }
}
