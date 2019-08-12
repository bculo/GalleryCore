using ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Infrastructure.CustomIdentity.Claim
{
    public class ClaimsHolder
    {
        public IEnumerable<System.Security.Claims.Claim> Claims { get; set; }
        public IIdentity ClaimsIdentity { get; set; }
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
        public IUploader Uploder { get; set; }
        public string AuthName { get; set; }
    }
}
