using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Web.Interfaces;
using Web.Services.Helpers;

namespace Web.Services
{
    public class ClaimFactory : IClaimFactory
    {
        protected ClaimsHolder Holder { get; }

        public ClaimFactory() => Holder = new ClaimsHolder();

        public virtual ClaimsHolder SetClaims(Uploader user, string authenticaitonName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(authenticaitonName))
            {
                throw new ArgumentNullException(nameof(authenticaitonName));
            }

            SetClaims(user);
            CreateClaimIdentity(authenticaitonName);
            CreateClaimPrincipal();
            return Holder;
        }

        protected virtual void SetClaims(Uploader user)
        {
            Holder.Claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                /*
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user?.Role?.Name ?? Role.User.Name)
                */
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
