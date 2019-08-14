using ApplicationCore.Exceptions;
using System.Security.Claims;

namespace Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if(claim.Value == null)
            {
                throw new InvalidUserException();
            }

            return claim.Value;
        }

        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(ClaimTypes.Role);

            if (claim.Value == null)
            {
                throw new InvalidUserException();
            }

            return claim.Value;
        }
    }
}
