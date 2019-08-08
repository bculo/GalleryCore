using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.IdentityData;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Infrastructure.Helpers.Claim
{
    public class ClaimsMaker : IClaimMaker
    {
        protected ClaimsHolder Holder { get; }

        public ClaimsMaker() => Holder = new ClaimsHolder();

        public virtual ClaimsHolder SetClaims(IUploader user, IDomainModel<Role> role, string authenticaitonName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (string.IsNullOrEmpty(authenticaitonName))
            {
                throw new ArgumentNullException(nameof(authenticaitonName));
            }

            SetClaims(user, role);
            CreateClaimIdentity(authenticaitonName);
            CreateClaimPrincipal();
            return Holder;
        }

        protected virtual void SetClaims(IUploader user, IDomainModel<Role> role)
        {
            GalleryUser userGallery = user as GalleryUser;
            Role userRole = role.ToDomainModel();

            Holder.Claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, userGallery.Id.ToString()),
                new System.Security.Claims.Claim(ClaimTypes.Name, userGallery.UserName),
                new System.Security.Claims.Claim(ClaimTypes.Role, userRole.Name),
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
