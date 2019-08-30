using ApplicationCore.Interfaces;
using Infrastructure.Helpers.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly TokenSecret secret;

        public UserService(IOptions<TokenSecret> appSettings)
        {
            this.secret = appSettings.Value;
        }

        public virtual async Task<string> AuthenticateAsync(string username, string password)
        {
            if(username != "leram" || password != "password")
            {
                return null;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secret.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, "UNIQUEID")
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public virtual async Task<IEnumerable<IUploader>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
