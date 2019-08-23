using Infrastructure.Identity;
using Infrastructure.IdentityData;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Web.Configuration
{
    /// <summary>
    /// Authentication configuration
    /// </summary>
    public static class AuthenticationConfiguration
    {
        /// <summary>
        /// Configure application authentication
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.UsingIdentity()) //Aplication using identity ?
            {
                CreateIdentity(services);
                IdentitySetup(services);
                AddExternalAuthentication(services, configuration);
            }
            else
            {
                SetAuthenticationWithNoIdentity(services, configuration);
            }

            //Common authentication configuration
            SetCookiePolicyOptions(services);
        }

        #region Custom authentication configuration

        /// <summary>
        /// Set cookie authentication, and social authentication
        /// If not using identity
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        /// <param name="configuration">IConfiguration instance</param>
        private static void SetAuthenticationWithNoIdentity(IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Authentication/Login/";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.Cookie.Expiration = TimeSpan.FromMinutes(10);
            })
            .AddCookie("ExternalLogin")
            .AddFacebook(options =>
            {
                options.SignInScheme = "ExternalLogin";
                options.AppId = configuration["Authentication:Facebook:AppId"];
                options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            })
            .AddGoogle(options =>
            {
                options.SignInScheme = "ExternalLogin";
                options.ClientId = configuration["Authentication:Google:AppId"];
                options.ClientSecret = configuration["Authentication:Google:AppSecret"];
            });
        }

        #endregion

        #region Identity authentication configuration

        /// <summary>
        /// Set identity and identity context
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        private static void CreateIdentity(IServiceCollection services)
        {
            services.AddIdentity<GalleryUser, GalleryRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();
        }

        /// <summary>
        /// Registration and login settings with identity
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        private static void IdentitySetup(IServiceCollection services)
        {
            //Login requirements
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            //Registration password and username requirements
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            // Cookie settings
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.LoginPath = "/Authentication/Login/";
                options.AccessDeniedPath = "/Authentication/Login/";
                options.SlidingExpiration = true;
            });
        }

        /// <summary>
        /// Set external authentication using identity
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        /// <param name="configuration">IConfiguration instance</param>
        private static void AddExternalAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication()
            .AddFacebook(options =>
            {
                options.AppId = configuration["Authentication:Facebook:AppId"];
                options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            })
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:AppId"];
                options.ClientSecret = configuration["Authentication:Google:AppSecret"];
            });
        }

        #endregion

        #region Common authentication configuration

        /// <summary>
        /// Set cookie policy options
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        private static void SetCookiePolicyOptions(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        #endregion
    }
}
