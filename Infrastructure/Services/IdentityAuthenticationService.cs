using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.IdentityData;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public virtual async Task<string> CreateConfirmationTokenAsync(IUploader uploader)
        {
            if (uploader == null)
            {
                throw new ArgumentNullException(nameof(uploader));
            }

            return await userManager.GenerateEmailConfirmationTokenAsync(uploader as GalleryUser);
        }

        public virtual async Task<string> CreatePasswordRecoveryTokenAsync(IUploader uploader)
        {
            if (uploader == null)
            {
                throw new ArgumentNullException(nameof(uploader));
            }

            return await userManager.GeneratePasswordResetTokenAsync(uploader as GalleryUser);
        }

        public virtual async Task<(IUploader, IEnumerable<ErrorMessage>)> RegisterUserAsync(string userName, string email, string password)
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

            if (!createResult.Succeeded)
            {
                return (galleryUser, CreateListOfErrors(createResult.Errors));
            }

            //uncomment next line if you don't have a trigger in database for adding role to the user
            //var roleResult = userManager.AddToRoleAsync(user, Role.User.Name); //Adding role to the user

            //We can create trigger for this situation to
            var createGalleryUser = repository.AddAsync(uploader); // add user to image gallery database

            //uncomment roleResult variable if you adding role to the user with method AddToRoleAsync(...) 
            await Task.WhenAll(createGalleryUser/*, roleResult*/);

            return (galleryUser, new List<ErrorMessage>()); //pass empty list if everything allright
        }

        public virtual async Task<IEnumerable<ErrorMessage>> VerifyConfirmationTokenAsync(string id, string token)
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

            if(user == null)
            {
                return new List<ErrorMessage>
                {
                    new ErrorMessage{ Key = "NoUser", Description = "User doesnt exist" }
                };
            }

            var resutlToken = await userManager.ConfirmEmailAsync(user, token);

            return CreateListOfErrors(resutlToken.Errors);
        }

        private IEnumerable<ErrorMessage> CreateListOfErrors(IEnumerable<IdentityError> errors)
        {
            return errors.Select(item => new ErrorMessage
            {
                Key = item.Code,
                Description = item.Description
            });
        }
    }
}
