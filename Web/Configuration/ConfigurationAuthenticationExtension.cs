using Microsoft.Extensions.Configuration;

namespace Web.Configuration
{
    /// <summary>
    /// Checking type of authentication
    /// </summary>
    public static class ConfigurationAuthenticationExtension
    {
        private static bool? usingIdentity;

        /// <summary>
        /// Using identity ?
        /// Read from appsettings.json => "UsingIdentityAuthentication" :  true / false
        /// </summary>
        /// <param name="configuration">Instance of IConfiguration</param>
        /// <returns>Are we using identity in this application</returns>
        public static bool UsingIdentity(this IConfiguration configuration)
        {
            if (!usingIdentity.HasValue)
            {
                usingIdentity = configuration.GetSection("UsingIdentityAuthentication").Get<bool>();
            }

            return usingIdentity.Value;
        }

    }
}
