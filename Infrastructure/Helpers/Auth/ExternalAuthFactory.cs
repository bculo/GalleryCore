using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Helpers.Auth
{
    public abstract class ExternalAuthFactory
    {
        protected Dictionary<string, string> Items { private get; set; } = new Dictionary<string, string>();

        private bool ItemsSetCorrectly()
        {
            if(string.IsNullOrEmpty(Identifier))
            {
                return false;
            }

            if (string.IsNullOrEmpty(UserName))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Email))
            {
                return false;
            }

            return true;
        }

        public bool CanAuthenticate(List<System.Security.Claims.Claim> claims)
        {
            PrepareClaims(claims);
            return ItemsSetCorrectly();
        }

        public bool CanAuthenticate(IEnumerable<System.Security.Claims.Claim> claims)
        {
            return CanAuthenticate(claims.ToList());
        }

        public string Identifier
        {
            get => Items[nameof(Identifier)] ?? null;
            protected set => Items.TryAdd(nameof(Identifier), value ?? null);
        }

        public string UserName
        {
            get => Items[nameof(UserName)] ?? null;
            protected set
            {
                int index = value.IndexOf('@');
                Items.TryAdd(nameof(UserName), value.Substring(0, index));
            }
        }

        public string Email
        {
            get => Items [nameof(Email)] ?? null;
            protected set => Items.TryAdd(nameof(Email), value ?? null);
        }

        protected virtual void PrepareClaims(List<System.Security.Claims.Claim> claims)
        {
            foreach (var item in claims)
            {
                if (item.Type.Contains(IdentifierCriteria))
                {
                    Identifier = item.Value;
                }
                else if (item.Type.Contains(EmailCriteria))
                {
                    Email = item.Value;
                    UserName = item.Value;
                }
            }
        }

        protected abstract string IdentifierCriteria { get; }
        protected abstract string EmailCriteria { get; }

        public static ExternalAuthFactory GetInstance(string provider)
        {
            switch (provider)
            {
                case "Facebook":
                    return new FacebookAuthFactory();
                case "Google":
                    return new GoogleAuthFactory();
                default:
                    return null;
            }
        }
    }
}
