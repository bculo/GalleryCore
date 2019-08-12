using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;

namespace Infrastructure.CustomIdentity.EntityFramework
{
    public class AppUser : IUploader
    {
        public AppUser()
        {
            Id = Guid.NewGuid().ToString(); //Add unique string ID to user
            SecurityStamp = Guid.NewGuid().ToString("D"); //Add security stamp
            AppRoleId = Role.User.Id; // Add role to user
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public bool IsExternal { get; set; }
        public string SecurityStamp { get; set; }

        public int AppRoleId { get; private set; }
        public AppRole AppRole { get; private set; }

        public string UserId => Id.ToString();
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
