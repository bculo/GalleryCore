using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Infrastructure.Helpers.Claim
{
    public class ClaimsHolder
    {
        public IEnumerable<System.Security.Claims.Claim> Claims { get; set; }
        public IIdentity ClaimsIdentity { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}
