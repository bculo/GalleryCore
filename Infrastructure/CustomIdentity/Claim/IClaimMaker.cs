using Infrastructure.CustomIdentity.EntityFramework;

namespace Infrastructure.CustomIdentity.Claim
{
    public interface IClaimMaker
    {
        ClaimsHolder SetClaims(AppUser user, string authenticaitonName);
    }
}
