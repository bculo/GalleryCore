using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.IdentityData
{
    public sealed class GalleryUser : IdentityUser, IUploader
    {
        private Uploader Uploader { get; set; }

        public bool IsExternal { get; set; }

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
            if (Uploader == null)
            {
                Uploader = new Uploader
                {
                    Id = Id,
                    UserName = UserName,
                    Email = Email,
                };
            }

            return Uploader;
        }

        public override string ToString() => UserName;
    }
}
