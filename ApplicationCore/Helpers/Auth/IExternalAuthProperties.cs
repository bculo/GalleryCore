using System.Collections.Generic;

namespace ApplicationCore.Helpers.Auth
{
    public interface IExternalAuthProperties
    {
        string RedirectUri { get; set; }
        IDictionary<string, string> Items { get; }
    }
}
