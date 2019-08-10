﻿using Infrastructure.Identity;
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
    public static class AuthenticationConfiguration
    {
        /// <summary>
        /// Configure application authentication
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            SetCookiePolicyOptions(services);
            //SetAuthenticationWithNoIdentity(services, configuration); //No Identity
            AddExternalAuthenticationWithIdentity(services, configuration); //Identity
            CreateIdentity(services); //Identity
            IdentitySettingSetup(services); //Identity
        }

        private static void AddExternalAuthenticationWithIdentity(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication()
            .AddFacebook(options =>
            {
                options.AppId = configuration["Authentication:Facebook:AppId"];
                options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            });
        }

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

        /// <summary>
        /// Set cookie authentication, and social authentication
        /// If not using identity
        /// </summary>
        /// <param name="services"></param>
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
            });

            //Read information from HttpContext => await HttpContext.AuthenticateAsync("ExternalLogin");
        }

        /// <summary>
        /// Set identity and identity context
        /// </summary>
        /// <param name="services"></param>
        private static void CreateIdentity(IServiceCollection services)
        {
            services.AddIdentity<GalleryUser, GalleryRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();
        }

        /// <summary>
        /// Registration and login settings
        /// </summary>
        /// <param name="services"></param>
        private static void IdentitySettingSetup(IServiceCollection services)
        {
            //Login requirements
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            //Registration password requirements and username requirements
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

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
                options.LoginPath = "/Authentication/Login/";
                options.AccessDeniedPath = "/Authentication/Login/";
                options.SlidingExpiration = true;
            });
        }
    }
}
