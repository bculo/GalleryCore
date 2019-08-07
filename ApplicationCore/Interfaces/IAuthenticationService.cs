using ApplicationCore.Entities;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IUploader> GetUserByIdAsync(string userId);
        Task<IUploader> GetUserByMailAsync(string email);

        Task<string> CreateConfirmationTokenAsync(IUploader uploader);
        Task<string> CreatePasswordRecoveryTokenAsync(IUploader uploader);

        Task<ServiceResult<IUploader>> RegisterUserAsync(string userName, string email, string password);
        Task<DefaultServiceResult> VerifyConfirmationTokenAsync(string userId, string token);
    }
}
