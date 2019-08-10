using ApplicationCore.Helpers.Auth;
using System.Collections.Generic;

namespace Infrastructure.Helpers.Auth
{
    public abstract class ExternalAuthenticationFactory
    {
        protected Dictionary<string, string> Items { private get; set; } = new Dictionary<string, string>();

        public abstract bool SetClaims(List<System.Security.Claims.Claim> claims);

        public string Identifier {
            get
            {
                return Items[nameof(Identifier)] ?? null;
            }
            set
            {
                Items.TryAdd(nameof(Identifier), value ?? null);
            }
        }

    }
}
