using ApplicationCore.Helpers.Auth;
using System;
using System.Collections.Generic;

namespace Infrastructure.Helpers.Auth
{
    public class ExternalAuthProperties : IAuthProperties
    {
        public string RedirectUri { get; }
        public IDictionary<string, string> Items { get; }

        public ExternalAuthProperties(string redirectUri, IDictionary<string, string> items)
        {
            if (string.IsNullOrEmpty(redirectUri))
            {
                throw new ArgumentNullException(nameof(redirectUri));
            }

            if (items == null || items.Count == 0)
            {
                throw new ArgumentNullException(nameof(redirectUri));
            }

            RedirectUri = redirectUri;
            Items = items;
        }
    }
}
