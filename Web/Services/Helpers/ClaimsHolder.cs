using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Web.Services.Helpers
{
    public class ClaimsHolder
    {
        public IEnumerable<Claim> Claims { get; set; }
        public IIdentity ClaimsIdentity { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}
