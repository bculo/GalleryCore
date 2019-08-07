using Microsoft.AspNetCore.Mvc;

namespace Web.Interfaces
{
    public interface IUrlGenerator
    {
        string CreateUrl(ControllerBase controller, string userId, string token,
            string destinationControllerName, string destinationActionName);
    }
}
