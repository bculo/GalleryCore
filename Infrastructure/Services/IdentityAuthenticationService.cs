using ApplicationCore.Entities;
using ApplicationCore.Helpers.Auth;
using ApplicationCore.Helpers.Service;
using ApplicationCore.Interfaces;
using Infrastructure.Helpers.AuthProperties;
using Infrastructure.Helpers.Http;
using Infrastructure.IdentityData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Authentication with UserManager and SignInManager
    /// </summary>
    public class IdentityAuthenticationService : HttpAccess, ApplicationCore.Interfaces.IAuthenticationService
    {
        protected readonly UserManager<GalleryUser> userManager;
        protected readonly IAsyncRepository<Uploader> repository;

        /// <summary>
        /// Add Dependency on Microsoft.AspNetCore.Identity.EntityFrameworkCore if working with SignInManager
        /// </summary>
        protected readonly SignInManager<GalleryUser> signInManager;

        public IdentityAuthenticationService(
            UserManager<GalleryUser> userManager,
            IAsyncRepository<Uploader> repository,
            SignInManager<GalleryUser> signInManager,
            IHttpContextAccessor accessor) : base(accessor)
        {
            this.userManager = userManager;
            this.repository = repository;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>user</returns>
        public virtual async Task<IUploader> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await userManager.FindByIdAsync(userId);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="userId">user email</param>
        /// <returns>user</returns>
        public virtual async Task<IUploader> GetUserByMailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="userId">user username</param>
        /// <returns>user</returns>
        public virtual async Task<IUploader> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            return await userManager.FindByNameAsync(userName);
        }

        /// <summary>
        /// Create email confirmation token
        /// </summary>
        /// <param name="uploader">user</param>
        /// <returns>confirmation token</returns>
        public virtual async Task<string> CreateConfirmationTokenAsync(IUploader uploader)
        {
            if (uploader == null)
            {
                throw new ArgumentNullException(nameof(uploader));
            }

            return await userManager.GenerateEmailConfirmationTokenAsync(uploader as GalleryUser);
        }

        /// <summary>
        /// Create password recovery token
        /// </summary>
        /// <param name="uploader">user</param>
        /// <returns>password recovery token</returns>
        public virtual async Task<string> CreatePasswordRecoveryTokenAsync(IUploader uploader)
        {
            if (uploader == null)
            {
                throw new ArgumentNullException(nameof(uploader));
            }

            return await userManager.GeneratePasswordResetTokenAsync(uploader as GalleryUser);
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>instance of ServiceResult</returns>
        public virtual async Task<ServiceResult<IUploader>> RegisterUserAsync(string userName, string email, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var resultFactory = new RequestWithResult<IUploader>();

            var instance = GetSpecificInstance<AppIdentityDbContext>();
            if (instance == null)
            {
                return resultFactory.FailedRequest("Problem with server, try again later");
            }

            var galleryUser = new GalleryUser { Email = email, UserName = userName };

            await ExecuteRegistrationTransaction(instance, galleryUser, password, resultFactory);

            return resultFactory.InstanceResult;
        }

        /// <summary>
        /// Execute transaction
        /// </summary>
        /// <param name="strategy"></param>
        /// <returns></returns>
        protected virtual async Task ExecuteRegistrationTransaction(DbContext database, IUploader user, string password,
            RequestWithResult<IUploader> serviceResult)
        {
            IExecutionStrategy databaseStrategy = database.Database.CreateExecutionStrategy();

            await databaseStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = database.Database.BeginTransaction())
                {
                    try
                    {
                        var createResult = await userManager.CreateAsync(user as GalleryUser, password); //Add user to identity database
                        if (!createResult.Succeeded) //Check status of operation
                        {
                            serviceResult.FailedRequest(createResult.Errors.Select(item => item.Description).ToList());
                            throw new Exception("Failed to add user to identity database");
                        }

                        var roleResult = userManager.AddToRoleAsync(user as GalleryUser, Role.User.Name); //Add role to user in identity database
                        var galleryDatabase = repository.AddAsync(user.ToDomainModel());

                        await Task.WhenAll(roleResult, galleryDatabase); // wait tasks to finish

                        if (!roleResult.Result.Succeeded) // Check status of adding role to user
                        {
                            serviceResult.FailedRequest("Problem with registration");
                            throw new Exception("Failed to add role to user");
                        }

                        if (!string.IsNullOrEmpty(galleryDatabase.Result.Item2)) //Check status of adding user to gallery database
                        {
                            serviceResult.FailedRequest("Problem with registration");
                            throw new Exception("Failed to add role to user");
                        }

                        transaction.Commit(); // commit transaction
                        serviceResult.SuccessRequest(user);
                    }
                    catch
                    {
                        serviceResult.FailedRequest("Problem with registration");
                    }
                }
            });
        }

        /// <summary>
        /// Verify confirmation token
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="token">user token</param>
        /// <returns>instance of DefaultServiceResult</returns>
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

            GalleryUser user = await userManager.FindByIdAsync(userId);

            var resultFactory = new RequestNoResult();

            if (user == null)
            {
                return resultFactory.FailedRequest("Used doesnt exist");
            }

            var resutlToken = await userManager.ConfirmEmailAsync(user, token);

            return resultFactory.SuccessRequest();
        }

        /// <summary>
        /// Verify password recovery token and change user password
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="token">user token</param>
        /// <param name="newPassword">new user password</param>
        /// <returns>instance of DefaultServiceResult</returns>
        public virtual async Task<ServiceNoResult> VerifyPasswordRecoveryTokenAsync(string userIdentification, string token, string newPassword)
        {
            if (string.IsNullOrEmpty(userIdentification))
            {
                throw new ArgumentNullException(nameof(userIdentification));
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException(nameof(newPassword));
            }

            GalleryUser user = await userManager.FindByIdAsync(userIdentification);

            var resultFactory = new RequestNoResult();

            if (user == null)
            {
                return resultFactory.FailedRequest("Used doesnt exist");
            }

            var resutlToken = await userManager.ResetPasswordAsync(user, token, newPassword);

            if (!resutlToken.Succeeded)
            {
                resultFactory.FailedRequest(resutlToken.Errors.Select(item => item.Description).ToList());
            }

            return resultFactory.SuccessRequest();
        }

        /// <summary>
        /// Sign in user using email or username
        /// </summary>
        /// <param name="userIdentification">email or username</param>
        /// <param name="password">password</param>
        /// <returns>instance of ServiceResult</returns>
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

            IUploader user = null;

            if (userIdentification.Contains("@"))
            {
                user = await GetUserByMailAsync(userIdentification);
            }
            else
            {
                user = await GetUserByUserNameAsync(userIdentification);
            }

            var serviceResult = new RequestWithResult<IUploader>();

            if (user == null)
            {
                return serviceResult.FailedRequest("Wrong credentials");
            }

            var signInResult = await signInManager.PasswordSignInAsync(user as GalleryUser, password, false, false);

            if (!signInResult.Succeeded)
            {
                return serviceResult.FailedRequest("Wrong credentials");
            }

            return serviceResult.SuccessRequest(user);
        }

        /// <summary>
        /// Sign out user
        /// </summary>
        /// <returns>-</returns>
        public virtual async Task SignOutUser()
        {
            await signInManager.SignOutAsync();
        }

        /// <summary>
        /// Properties for external authentication (facebook, google ...)
        /// </summary>
        /// <param name="provider">name of provider</param>
        /// <param name="redirectUrl">provider should redirect to</param>
        /// <returns>instance of IExternalAuthProperties</returns>
        public virtual async Task<ServiceResult<IExternalAuthProperties>> GetAuthProperties(string provider, string redirectUrl)
        {
            if (string.IsNullOrEmpty(provider))
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrEmpty(redirectUrl))
            {
                throw new ArgumentNullException(nameof(redirectUrl));
            }

            var result = new RequestWithResult<IExternalAuthProperties>();

            var allAuthScheme = (await signInManager.GetExternalAuthenticationSchemesAsync())
                .Select(item => item.Name)
                .ToList();

            if (!allAuthScheme.Contains(provider))
            {
                return result.FailedRequest("Requested provider doesnt exist");
            }

            AuthenticationProperties prop = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return result.SuccessRequest(new GalleryAuthenticationProperties(prop.Items, prop.RedirectUri));
        }

        /// <summary>
        /// Validate social login status
        /// </summary>
        /// <returns>true if everything good</returns>
        protected virtual async Task<Dictionary<string, string>> GetExternalAuthenticationClaims()
        {
            var result = await signInManager.GetExternalLoginInfoAsync();

            throw new NotImplementedException();
        }

        public Task<bool> ExecuteExternalLogin()
        {
            throw new NotImplementedException();
        }
    }
}
