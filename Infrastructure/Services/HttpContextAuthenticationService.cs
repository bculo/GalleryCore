using ApplicationCore.Entities;
using ApplicationCore.Helpers.Auth;
using ApplicationCore.Helpers.Generator;
using ApplicationCore.Helpers.Security;
using ApplicationCore.Helpers.Service;
using ApplicationCore.Interfaces;
using Infrastructure.Helpers.Claim;
using Infrastructure.Helpers.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Another way of implementing IAuthenticationService using IHttpContextAccessor
    /// We dont need to use microsoft identity for authentication
    /// </summary>
    public class HttpContextAuthenticationService : HttpAccess, ApplicationCore.Interfaces.IAuthenticationService
    {
        protected readonly IAsyncRepository<Uploader> repository;
        protected readonly IUniqueStringGenerator generator;
        protected readonly IHasher hasher;

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
            IUniqueStringGenerator generator,
            IHasher hasher) : base(accessor)
        {
            this.repository = repository;
            this.generator = generator;
            this.hasher = hasher;
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

        public Task<ServiceResult<IExternalAuthProperties>> GetAuthProperties(string provider, string redirectUrl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validate social login status
        /// </summary>
        /// <returns>true if everything good</returns>
        public virtual async Task<bool> ValidteExternalAuthentication()
        {
            var result = await Http.AuthenticateAsync("ExternalLogin");

            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public Task<bool> ExecuteExternalLogin()
        {
            throw new NotImplementedException();
        }
    }
}
