using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Role : Enumeration.Enumeration
    {
        public static Role User = new Role(1, "User");
        public static Role Moderator = new Role(2, "Moderator");
        public static Role Administrator = new Role(3, "Administrator");

        private Role(int id, string name) : base(id, name) { }

        public static List<Role> GetRoles()
        {
            return new List<Role>
            {
                User,
                Moderator,
                Administrator
            };
        }
    }
}
