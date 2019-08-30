using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUserService
    {
        Task<string> AuthenticateAsync(string username, string password);
        Task<IEnumerable<IUploader>> GetAllAsync();
    }
}
