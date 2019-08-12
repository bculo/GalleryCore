using Infrastructure.CustomIdentity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Infrastructure.CustomIdentity.Claim
{
    public class ClaimMaker : IClaimMaker
    {
        protected ClaimsHolder Holder { get; }

        public ClaimMaker() => Holder = new ClaimsHolder();

        public virtual ClaimsHolder SetClaims(AppUser user, string authenticaitonName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(authenticaitonName))
            {
                throw new ArgumentNullException(nameof(authenticaitonName));
            }

            Holder.AuthName = authenticaitonName;
            Holder.Uploder = user;
            SetClaims(user);
            CreateClaimIdentity(authenticaitonName);
            CreateClaimPrincipal();
            return Holder;
        }

        protected virtual void SetClaims(AppUser user)
        {
            Holder.Claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.Id),
                new System.Security.Claims.Claim(ClaimTypes.Name, user.UserName),
                new System.Security.Claims.Claim(ClaimTypes.Role, user.AppRole.Name),
            };
        }

        protected virtual void CreateClaimIdentity(string authName)
        {
            Holder.ClaimsIdentity = new ClaimsIdentity(Holder.Claims, authName);
        }

        protected virtual void CreateClaimPrincipal()
        {
            Holder.ClaimsPrincipal = new ClaimsPrincipal(Holder.ClaimsIdentity);
        }
    }
}
