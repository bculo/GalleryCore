using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Infrastructure.Helpers.Claim
{
    public interface IClaimMaker
    {
        ClaimsHolder SetClaims(IUploader user, IDomainModel<Role> role, string authenticaitonName);
    }
}
