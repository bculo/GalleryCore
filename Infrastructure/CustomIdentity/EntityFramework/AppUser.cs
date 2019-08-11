using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Infrastructure.CustomIdentity.EntityFramework
{
    public class AppUser : BaseEntity<string>, IUploader
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public bool IsExternal { get; set; }
        public string SecurityStamp { get; set; }

        public int AppRoleId { get; set; }
        public AppRole AppRole { get; set; }

        public string UserId => Id;
        public string UserMail => Email;

        private Uploader Uploader { get; set; }

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
    }
}
