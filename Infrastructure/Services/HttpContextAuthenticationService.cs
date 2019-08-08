using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Helpers.Claim;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Another way of implementing IAuthenticationService using IHttpContextAccessor
    /// </summary>
    public class HttpContextAuthenticationService : IAuthenticationService
    {
        protected readonly IAsyncRepository<Uploader> repository;
        protected readonly IHttpContextAccessor accessor;

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

        protected HttpContext Http
        {
            get
            {
                return accessor?.HttpContext;
            }
        }

        public HttpContextAuthenticationService(
            IAsyncRepository<Uploader> repository,
            IHttpContextAccessor accessor)
        {
            this.repository = repository;
            this.accessor = accessor;
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

        public virtual Task SignOutUser()
        {
            throw new NotImplementedException();
        }

        public virtual Task<DefaultServiceResult> VerifyConfirmationTokenAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public virtual Task<DefaultServiceResult> VerifyPasswordRecoveryTokenAsync(string userId, string token, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
