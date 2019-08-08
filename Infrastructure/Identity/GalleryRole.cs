using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class GalleryRole : IdentityRole, IDomainModel<Role>
    {
        public Role ToDomainModel()
        {
            if (Name == Role.User.Name)
            {
                return Role.User;
            }

            if (Name == Role.Moderator.Name)
            {
                return Role.Moderator;
            }

            return Role.Administrator;
        }
    }
}
