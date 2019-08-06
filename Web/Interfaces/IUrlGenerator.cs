using ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Web.Interfaces
{
    public interface IUrlGenerator
    {
        string CreateActivationUrl(ControllerBase controller, string userId, string token, string destinationControllerName, string destinationActionName);
        string CreatePasswordRecoveryUrl(ControllerBase controller, Uploader user, string destinationControllerName, string destinationActionName);
    }
}
