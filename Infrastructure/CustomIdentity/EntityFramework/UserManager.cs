using ApplicationCore.Helpers.Service;
using Infrastructure.CustomIdentity.Claim;
using Infrastructure.CustomIdentity.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.CustomIdentity.EntityFramework
{
    public class UserManager : IUserManager
    {
        protected readonly CustomIdentityDbContext context;
        protected readonly IHasher hasher;
        protected readonly IDataProtectionProvider protectionProvider;
        protected readonly IClaimMaker maker;

        public UserManager(CustomIdentityDbContext context,
            IHasher hasher,
            IDataProtectionProvider protectionProvider,
            IClaimMaker maker)
        {
            this.context = context;
            this.hasher = hasher;
            this.protectionProvider = protectionProvider;
            this.maker = maker;
        }

        /// <summary>
        /// Create confirmation token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string CreateConfirmationToken(AppUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IDataProtector protector = protectionProvider.CreateProtector(ProtectorPurpse.MailConfirmation);

            var ms = new MemoryStream();
            using (var writer = new StreamWriter(ms))
            {
                writer.WriteLine(user.Id);
                writer.WriteLine(ProtectorPurpse.MailConfirmation);
                writer.WriteLine(user.SecurityStamp);
                writer.Flush();
            }

            byte[] protectedData = protector.Protect(ms.ToArray());
            return Convert.ToBase64String(protectedData);
        }

        /// <summary>
        /// Confirm email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<bool> VerifyConifrmationTokenAsync(string id, string token)
        {
            try
            {
                AppUser user = await context.AppUsers
                    .FirstOrDefaultAsync(item => item.Id == id && !item.EmailConfirmed);

                if (user == null) //User doesnt exist in database 
                {
                    return false;
                }

                IDataProtector protector = protectionProvider.CreateProtector(ProtectorPurpse.MailConfirmation); //Create protector
                byte[] unprotectedData = protector.Unprotect(Convert.FromBase64String(token)); //Unprotect data

                var ms = new MemoryStream(unprotectedData);
                using (var reader = new StreamReader(ms))
                {
                    string userId = await reader.ReadLineAsync();
                    if(userId != id)
                    {
                        return false;
                    }

                    string protectionPurpose = await reader.ReadLineAsync();
                    if(protectionPurpose != ProtectorPurpse.MailConfirmation)
                    {
                        return false;
                    }

                    string securityStamp = await reader.ReadLineAsync();
                    if(securityStamp != user.SecurityStamp)
                    {
                        return false;
                    }

                    if(reader.Peek() != -1) //More content for reading
                    {
                        return false;
                    }

                    user.EmailConfirmed = true;
                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Create password recovery token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual string CreatePasswordRecoveryToken(AppUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IDataProtector protector = protectionProvider.CreateProtector(ProtectorPurpse.PasswordRecovery);

            var ms = new MemoryStream();
            using (var writer = new StreamWriter(ms))
            {
                writer.WriteLine(DateTime.UtcNow);
                writer.WriteLine(user.Id);
                writer.WriteLine(ProtectorPurpse.MailConfirmation);
                writer.WriteLine(user.SecurityStamp);
                writer.Flush();
            }

            byte[] protectedData = protector.Protect(ms.ToArray());
            return Convert.ToBase64String(protectedData);
        }

        /// <summary>
        /// Verify password recovery token
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<bool> VerifyPasswordRecoveryTokenAsync(string id, string token, string newPassword)
        {
            try
            {
                AppUser user = await context.AppUsers
                    .FirstOrDefaultAsync(item => item.Id == id && !item.EmailConfirmed);

                if (user == null) //User doesnt exist in database 
                {
                    return false;
                }

                IDataProtector protector = protectionProvider.CreateProtector(ProtectorPurpse.PasswordRecovery); //Create protector
                byte[] unprotectedData = protector.Unprotect(Convert.FromBase64String(token)); //Unprotect data

                var ms = new MemoryStream(unprotectedData);
                using (var reader = new StreamReader(ms))
                {
                    DateTime creationTime = DateTime.Parse(await reader.ReadLineAsync());
                    DateTime expirationTime = creationTime.AddDays(1);
                    if (expirationTime < DateTime.UtcNow) //check for 24 hour span
                    {
                        return false;
                    }

                    string userId = await reader.ReadLineAsync();
                    if (userId != id)
                    {
                        return false;
                    }

                    string protectionPurpose = await reader.ReadLineAsync();
                    if (protectionPurpose != ProtectorPurpse.MailConfirmation)
                    {
                        return false;
                    }

                    string securityStamp = await reader.ReadLineAsync();
                    if (securityStamp != user.SecurityStamp)
                    {
                        return false;
                    }

                    if (reader.Peek() != -1) //More content for reading
                    {
                        return false;
                    }

                    user.SecurityStamp = Guid.NewGuid().ToString();
                    user.EmailConfirmed = true;
                    user.PasswordHash = await hasher.Hash(newPassword);
                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual async Task<AppUser> CreateUserAsync(AppUser user, string password)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if(password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            user.PasswordHash = await hasher.Hash(password);

            var result = new RequestResult<AppUser>();

            try
            {
                context.AppUsers.Add(user);
                await context.SaveChangesAsync();
                return user;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Check if user can sign in
        /// </summary>
        /// <param name="identificator"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual async Task<ClaimsHolder> CanSignInAsync(string identificator, string password)
        {
            AppUser user = null;

            if (identificator.Contains("@"))
            {
                user = await GetUserByEmailAsync(identificator);
            }
            else
            {
                user = await GetUserByNameAsync(identificator);
            }

            if(user == null || !user.EmailConfirmed)
            {
                return null;
            }

            var (Verified, NeedsUpgrade) = await hasher.CheckHash(user.PasswordHash, password);
            if (!Verified)
            {
                return null;
            }

            return maker.SetClaims(user, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await context.AppUsers.Where(item => item.Id == userId)
                .Include(item => item.AppRole)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual async Task<AppUser> GetUserByNameAsync(string userName)
        {
            return await context.AppUsers.Where(item => item.UserName == userName)
                .Include(item => item.AppRole)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public virtual async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await context.AppUsers.Where(item => item.Email == email)
                .Include(item => item.AppRole)
                .FirstOrDefaultAsync();
        }
    }
}
