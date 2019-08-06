using ApplicationCore.Entities;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUserRepository : IAsyncRepository<Uploader>
    {
        Task<Uploader> GetUserByUserNameAsync(string username);
        Task<Uploader> LoginValidationAsync(string username, string hashedPassword);
    }
}
