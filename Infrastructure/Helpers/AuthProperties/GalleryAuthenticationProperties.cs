using ApplicationCore.Helpers.Auth;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

namespace Infrastructure.Helpers.AuthProperties
{
    public class GalleryAuthenticationProperties : AuthenticationProperties, IExternalAuthProperties
    {
        public GalleryAuthenticationProperties(IDictionary<string, string> items, string redirectUrl)
            : base(items)
        {
            RedirectUri = redirectUrl;
        }
    }
}
