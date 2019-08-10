using System.Threading.Tasks;

namespace ApplicationCore.Helpers.Security
{
    public interface IHasher
    {
        Task<string> Hash(string content);
        Task<(bool Verified, bool NeedsUpgrade)> CheckHash(string content, string salt);
    }
}
