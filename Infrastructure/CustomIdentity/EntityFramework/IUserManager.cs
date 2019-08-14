using Infrastructure.CustomIdentity.Claim;
using System.Threading.Tasks;

namespace Infrastructure.CustomIdentity.EntityFramework
{
    public interface IUserManager
    {
        Task<AppUser> GetUserByIdAsync(string userId);
        Task<AppUser> GetUserByNameAsync(string userName);
        Task<AppUser> GetUserByEmailAsync(string email);

        Task<AppUser> CreateUserAsync(AppUser user, string password);
        Task<ClaimsHolder> CanSignInAsync(string identificator, string password);

        string CreatePasswordRecoveryToken(AppUser user);
        Task<bool> VerifyPasswordRecoveryTokenAsync(string id, string token, string newPassword);
        string CreateConfirmationToken(AppUser user);
        Task<bool> VerifyConifrmationTokenAsync(string id, string token);
    }
}
