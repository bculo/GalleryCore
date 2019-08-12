using System.Threading.Tasks;

namespace Infrastructure.CustomIdentity.Interfaces
{
    public interface IHasher
    {
        Task<string> Hash(string content);
        Task<(bool Verified, bool NeedsUpgrade)> CheckHash(string hashedContent, string unhashedContent);
    }
}
