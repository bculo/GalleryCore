using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace Infrastructure.CustomIdentity.EntityFramework
{
    public class AppRole : IDomainModel<Role>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<AppUser> Users { get; set; }

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
