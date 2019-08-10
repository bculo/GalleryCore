using System;
using System.Security.Claims;

namespace Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            principal.PrincipalValid();

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value ?? null;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            principal.PrincipalValid();

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value ?? null;
        }

        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            principal.PrincipalValid();

            var claim = principal.FindFirst(ClaimTypes.Role);
            return claim?.Value ?? null;
        }

        private static void PrincipalValid(this ClaimsPrincipal principal)
        {
            if(principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
        }
    }
}
