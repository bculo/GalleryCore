using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IPasswordHasher
    {
        byte[] Salt { get; }
        string SaltAs64String { get; }
        string HashedPassword { get; }
        Task HashPasswordAsync(string password, string salt = null);
        void HashPassword(string password, string salt = null);
    }
}
