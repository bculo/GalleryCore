using ApplicationCore.Entities;
using Web.Services.Helpers;

namespace Web.Interfaces
{
    public interface IClaimFactory
    {
        ClaimsHolder SetClaims(Uploader user, string authenticationName);
    }
}
