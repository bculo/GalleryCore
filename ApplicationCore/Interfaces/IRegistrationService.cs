using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRegistrationService
    {
        Task<(Uploader, IEnumerable<string>)> RegisterUserAsync(string email, string userName, string password = "");
        Task<string> GenerateRegistrationToken(Uploader uploader);
        Task<string> ConfirmRegistrationToken(string userId, string token);
        Task<string> PasswordyRecoveryToken(Uploader uploader);
    }
}
