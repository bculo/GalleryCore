using ApplicationCore.Helpers.Security;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CustomIdentity.Security
{
    /// <summary>
    /// Uses Rfc2898DeriveBytes with SHA256
    /// Working with format {iterations}.{salt}.{key}
    /// </summary>
    public class PasswordHasher
    {
        private HashOptions Options { get; }

        private int SaltSize { get; } = 16; // 128 bit 
        private int KeySize { get; } = 32; // 256 bit

        public PasswordHasher(IOptions<HashOptions> options)
        {
            Options = options.Value;
        }

        public virtual async Task<(bool Verified, bool NeedsUpgrade)> CheckHash(string hashedContent, string content)
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

                bool needsUpgrade = iterations != Options.Iterations;

                using (var algorithm = new Rfc2898DeriveBytes(
                  content,
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

        public virtual async Task<string> Hash(string content)
        {
            string result = await Task.Run(() =>
            {
                using (var algorithm = new Rfc2898DeriveBytes(
                  content,
                  SaltSize,
                  Options.Iterations,
                  HashAlgorithmName.SHA256))
                {
                    string key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                    string salt = Convert.ToBase64String(algorithm.Salt);

                    return $"{Options.Iterations}.{salt}.{key}";
                }
            });

            return result;
        }
    }
}
