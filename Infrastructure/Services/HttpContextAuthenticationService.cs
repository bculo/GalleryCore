using ApplicationCore.Entities;
using ApplicationCore.Helpers.Auth;
using ApplicationCore.Helpers.Generator;
using ApplicationCore.Helpers.Security;
using ApplicationCore.Helpers.Service;
using ApplicationCore.Interfaces;
using Infrastructure.Helpers.Auth;
using Infrastructure.Helpers.Claim;
using Infrastructure.Helpers.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Another way of implementing IAuthenticationService using IHttpContextAccessor
    /// We dont need to use microsoft identity for authentication
    /// </summary>
    public class HttpContextAuthenticationService : HttpAccess, ApplicationCore.Interfaces.IAuthenticationService
    {
        //TODO
        //REMOVE IHasher and IUniqueStringGenerator in customIdentityFolder

        protected readonly IAsyncRepository<Uploader> repository;
        protected readonly IUniqueStringGenerator generator; //move
        protected readonly IHasher hasher; //move
        protected readonly IAuthenticationSchemeProvider schemeProvider;

        private IClaimMaker maker;

        protected IClaimMaker Maker
        {
            get
            {
                if(maker == null)
                {
                    maker = new ClaimsMaker();
                }

                return maker;
            }
        }

        public HttpContextAuthenticationService(
            IAsyncRepository<Uploader> repository,
            IHttpContextAccessor accessor,
            IUniqueStringGenerator generator, //move
            IHasher hasher, //move
            IAuthenticationSchemeProvider schemeProvider) : base(accessor)
        {
            this.repository = repository;
            this.generator = generator;
            this.hasher = hasher;
            this.schemeProvider = schemeProvider;
        }

        public virtual Task<string> CreateConfirmationTokenAsync(IUploader uploader)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string> CreatePasswordRecoveryTokenAsync(IUploader uploader)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IUploader> GetUserByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IUploader> GetUserByMailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IUploader> GetUserByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public virtual Task<ServiceResult<IUploader>> RegisterUserAsync(string userName, string email, string password)
        {
            throw new NotImplementedException();
        }

        public virtual Task<ServiceResult<IUploader>> SignInUserAsync(string userIdentification, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sign out user
        /// </summary>
        /// <returns>-</returns>
        public virtual async Task SignOutUser()
        {
            await Http.SignOutAsync();
        }

        public virtual Task<ServiceNoResult> VerifyConfirmationTokenAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public virtual Task<ServiceNoResult> VerifyPasswordRecoveryTokenAsync(string userId, string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Configure authentication configuration for external login
        /// </summary>
        /// <param name="provider">name of social provider</param>
        /// <param name="redirectUrl">where should social provider redirect</param>
        /// <returns>Instance of ServiceResult</returns>
        public virtual async Task<ServiceResult<IAuthProperties>> GetAuthProperties(string provider, string redirectUrl)
        {
            if (string.IsNullOrEmpty(provider))
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrEmpty(redirectUrl))
            {
                throw new ArgumentNullException(nameof(redirectUrl));
            }

            var result = new RequestWithResult<IAuthProperties>();

            var allAuthScheme = (await schemeProvider.GetRequestHandlerSchemesAsync())
                .Select(item => item.Name)
                .ToList();

            if (!allAuthScheme.Contains(provider))
            {
                return result.FailedRequest("Requested provider is not supported");
            }

            var items = new Dictionary<string, string>()
            {
                { "LoginProvider", provider }
            };

            return result.SuccessRequest(new ExternalAuthProperties(redirectUrl, items));
        }

        public virtual async Task<bool> ExecuteExternalLogin()
        {
            var result = await Http.AuthenticateAsync("ExternalLogin");

            if (!result.Succeeded)
            {
                return false;
            }

            throw new NotImplementedException();
        }
    }
}
