﻿using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services.Helpers.ResultServices;
using Infrastructure.IdentityData;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class IdentityAuthenticationService : IAuthenticationService
    {
        protected readonly UserManager<GalleryUser> userManager;
        protected readonly IAsyncRepository<Uploader> repository;

        public IdentityAuthenticationService(
            UserManager<GalleryUser> userManager,
            IAsyncRepository<Uploader> repository)
        {
            this.userManager = userManager;
            this.repository = repository;
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

            var galleryUser = new GalleryUser { Email = email, UserName = userName };
            var uploader = new Uploader { UserName = userName, Id = galleryUser.Id, Email = email };

            var createResult = await userManager.CreateAsync(galleryUser, password); //Add user to identity database

            var resultFactory = new ResultServiceRequest<IUploader>();

            if (!createResult.Succeeded)
            {
                return resultFactory.FailedRequest(createResult.Errors
                    .Select(item => item.Description)
                    .ToList());
            }

            //uncomment next line if you don't have a trigger in database for adding role to the user
            //var roleResult = userManager.AddToRoleAsync(user, Role.User.Name); //Adding role to the user

            //We can create trigger for this situation to
            var createGalleryUser = repository.AddAsync(uploader); // add user to image gallery database

            //uncomment roleResult variable if you adding role to the user with method AddToRoleAsync(...) 
            await Task.WhenAll(createGalleryUser/*, roleResult*/);

            return resultFactory.SuccessRequest(galleryUser);
        }

        public virtual async Task<DefaultServiceResult> VerifyConfirmationTokenAsync(string id, string token)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            GalleryUser user = await userManager.FindByIdAsync(id);

            var resultFactory = new NoResultServiceRequest();

            if (user == null)
            {
                return resultFactory.FailedRequest("Used doenst exist");
            }

            var resutlToken = await userManager.ConfirmEmailAsync(user, token);

            return resultFactory.SuccessRequest();
        }
    }
}
