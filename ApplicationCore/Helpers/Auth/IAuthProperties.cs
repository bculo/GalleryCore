using System.Collections.Generic;

namespace ApplicationCore.Helpers.Auth
{
    public interface IAuthProperties
    {
        string RedirectUri { get; }
        IDictionary<string, string> Items { get; }
    }
}
