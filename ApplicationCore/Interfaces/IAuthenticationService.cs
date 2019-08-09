using ApplicationCore.Entities;
using ApplicationCore.Helpers.Service;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IUploader> GetUserByIdAsync(string userId);
        Task<IUploader> GetUserByUserNameAsync(string userName);
        Task<IUploader> GetUserByMailAsync(string email);

        Task<string> CreateConfirmationTokenAsync(IUploader uploader);
        Task<string> CreatePasswordRecoveryTokenAsync(IUploader uploader);

        Task<ServiceResult<IUploader>> RegisterUserAsync(string userName, string email, string password);
        Task<ServiceResult<IUploader>> SignInUserAsync(string userIdentification, string password);
        Task<ServiceNoResult> VerifyConfirmationTokenAsync(string userId, string token);
        Task<ServiceNoResult> VerifyPasswordRecoveryTokenAsync(string userId, string token, string newPassword);

        Task SignOutUser();
    }
}
