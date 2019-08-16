using ApplicationCore.Entities;
using ApplicationCore.Helpers.Auth;
using ApplicationCore.Helpers.Service;
using ApplicationCore.Interfaces;
using Infrastructure.CustomIdentity.EntityFramework;
using Infrastructure.Helpers.Auth;
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
        protected readonly IAsyncRepository<Uploader> repository;
        protected readonly IAuthenticationSchemeProvider schemeProvider;
        protected readonly IUserManager manager;

        public HttpContextAuthenticationService(
            IAsyncRepository<Uploader> repository,
            IHttpContextAccessor accessor,
            IAuthenticationSchemeProvider schemeProvider,
            IUserManager manager) : base(accessor)
        {
            this.repository = repository;
            this.schemeProvider = schemeProvider;
            this.manager = manager;
        }

        /// <summary>
        /// Create confirmationToken
        /// </summary>
        /// <param name="uploader"></param>
        /// <returns></returns>
        public virtual Task<string> CreateConfirmationTokenAsync(IUploader uploader)
        {
            if(uploader == null)
            {
                throw new ArgumentNullException(nameof(uploader));
            }

            return Task.FromResult(manager.CreateConfirmationToken(uploader as AppUser));
        }

        /// <summary>
        /// Verify confirmation token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<ServiceNoResult> VerifyConfirmationTokenAsync(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            var result = new RequestNoResult();

            var confirmationResult = await manager.VerifyConifrmationTokenAsync(userId, token);

            if (confirmationResult)
            {
                return result.GoodRequest();
            }
            else
            {
                return result.BadRequest("Token confirmation failed");
            }
        }

        /// <summary>
        /// Create password recovery token
        /// </summary>
        /// <param name="uploader"></param>
        /// <returns></returns>
        public virtual Task<string> CreatePasswordRecoveryTokenAsync(IUploader uploader)
        {
            if (uploader == null)
            {
                throw new ArgumentNullException(nameof(uploader));
            }

            return Task.FromResult(manager.CreatePasswordRecoveryToken(uploader as AppUser));
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task<IUploader> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await manager.GetUserByIdAsync(userId);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public virtual async Task<IUploader> GetUserByMailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await manager.GetUserByEmailAsync(email);
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual async Task<IUploader> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            return await manager.GetUserByNameAsync(userName);
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual async Task<ServiceResult<IUploader>> RegisterUserAsync(string userName, string email, string password)
        {
            var instance = new RequestResult<IUploader>();

            var user = new AppUser
            {
                Email = email,
                UserName = userName,
            };

            var serviceResult = new RequestResult<IUploader>();

            var createResult = await manager.CreateUserAsync(user, password);
            if(createResult != null)
            {
                return serviceResult.GoodRequest(createResult);
            }

            return serviceResult.BadRequest("Error");
        }

        /// <summary>
        /// Sign in user using HtppContext and method SignInAsync
        /// </summary>
        /// <param name="userIdentification"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual async Task<ServiceResult<IUploader>> SignInUserAsync(string userIdentification, string password)
        {
            if (string.IsNullOrEmpty(userIdentification))
            {
                throw new ArgumentNullException(nameof(userIdentification));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var serviceResult = new RequestResult<IUploader>();

            var claimHolder = await manager.CanSignInAsync(userIdentification, password);
            if(claimHolder == null)
            {
                serviceResult.BadRequest("Wrong credentials");
            }

            await Http.SignInAsync(claimHolder.AuthName, claimHolder.ClaimsPrincipal);

            return serviceResult.GoodRequest(claimHolder.Uploder);
        }

        /// <summary>
        /// Sign out user
        /// </summary>
        /// <returns>-</returns>
        public virtual async Task SignOutUser()
        {
            await Http.SignOutAsync();
        }

        /// <summary>
        /// Verify password recovery token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public virtual async Task<ServiceNoResult> VerifyPasswordRecoveryTokenAsync(string userId, string token, string newPassword)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException(nameof(newPassword));
            }

            var resultFactory = new RequestNoResult();

            bool success = await manager.VerifyPasswordRecoveryTokenAsync(userId, token, newPassword);

            if (success)
            {
                resultFactory.BadRequest("Password recovery failed");
            }

            return resultFactory.GoodRequest();
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

            var result = new RequestResult<IAuthProperties>();

            var allAuthScheme = (await schemeProvider.GetRequestHandlerSchemesAsync())
                .Select(item => item.Name)
                .ToList();

            if (!allAuthScheme.Contains(provider))
            {
                return result.BadRequest("Requested provider is not supported");
            }

            var items = new Dictionary<string, string>()
            {
                { "LoginProvider", provider }
            };

            return result.GoodRequest(new ExternalAuthProperties(redirectUrl, items));
        }

        public virtual async Task<bool> ExecuteExternalLogin()
        {
            var result = await Http.AuthenticateAsync("ExternalLogin");

            if (!result.Succeeded)
            {
                return false;
            }

            //TODO

            throw new NotImplementedException();
        }
    }
}
