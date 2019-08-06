using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.IdentityData
{
    public sealed class GalleryUser : IdentityUser, IUploader
    {
        public string UserId
        {
            get
            {
                return Id;
            }
        }

        public string UserMail
        {
            get
            {
                return Email;
            }
        }

        /// <summary>
        /// Convert IdentityUser into Uploader
        /// </summary>
        /// <returns>Uploader instance</returns>
        public Uploader ToDomainModel()
        {
            return new Uploader
            {
                Id = Id,
                UserName = UserName,
                Email = Email,
            };
        }

        public override string ToString() => UserName;
    }
}
