using Infrastructure.CustomIdentity.Interfaces;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Infrastructure.CustomIdentity.Security
{
    /// <summary>
    /// Uses Rfc2898DeriveBytes with SHA256
    /// Working with format {iterations}.{salt}.{key}
    /// </summary>
    public class PasswordHasher : IHasher
    {
        private int Iterations { get; } = 1000;
        private int SaltSize { get; } = 16; // 128 bit 
        private int KeySize { get; } = 32; // 256 bit

        /// <summary>
        /// Verify hashed content
        /// </summary>
        /// <param name="hashedContent">Hashed content</param>
        /// <param name="unhashedContent">Unhashed contetn</param>
        /// <returns>true if they match</returns>
        public virtual async Task<(bool Verified, bool NeedsUpgrade)> CheckHash(string hashedContent, string unhashedContent)
        {
            var result = await Task.Run(() =>
            {
                string[] parts = hashedContent.Split('.', 3);

                if (parts.Length != 3)
                {
                    throw new FormatException("Unexpected hash format");
                }

                var iterations = Convert.ToInt32(parts[0]);
                var salt = Convert.FromBase64String(parts[1]);
                var key = Convert.FromBase64String(parts[2]);

                bool needsUpgrade = iterations != Iterations;

                using (var algorithm = new Rfc2898DeriveBytes(
                  unhashedContent,
                  salt,
                  iterations,
                  HashAlgorithmName.SHA256))
                {
                    byte[] keyToCheck = algorithm.GetBytes(KeySize);

                    bool verified = keyToCheck.SequenceEqual(key);

                    return (verified, needsUpgrade);
                }
            });

            return result;
        }

        /// <summary>
        /// Hash content
        /// </summary>
        /// <param name="content">content to hash</param>
        /// <returns>hashed content</returns>
        public virtual async Task<string> Hash(string content)
        {
            string result = await Task.Run(() =>
            {
                using (var algorithm = new Rfc2898DeriveBytes(
                  content,
                  SaltSize,
                  Iterations,
                  HashAlgorithmName.SHA256))
                {
                    string key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                    string salt = Convert.ToBase64String(algorithm.Salt);

                    return $"{Iterations}.{salt}.{key}";
                }
            });

            return result;
        }
    }
}
