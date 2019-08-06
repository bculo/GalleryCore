using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IAuthenticationService
    {
        Task<(IUploader, IEnumerable<ErrorMessage>)> RegisterUserAsync(string userName, string email, string password);
        Task<string> CreateConfirmationTokenAsync(IUploader uploader);
        Task<IEnumerable<ErrorMessage>> VerifyConfirmationTokenAsync(string id, string token);
        Task<string> CreatePasswordRecoveryTokenAsync(IUploader uploader);
    }
}
