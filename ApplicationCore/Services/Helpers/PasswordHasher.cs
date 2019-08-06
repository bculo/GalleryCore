using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ApplicationCore.Services.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        private byte[] salt;
        public byte[] Salt
        {
            get
            {
                if (salt == null)
                    throw new ArgumentNullException();
                return salt;
            }
            private set { salt = value; }
        }

        
        public string SaltAs64String
        {
            get
            {
                if (Salt == null)
                    throw new ArgumentNullException();
                return Convert.ToBase64String(Salt);
            }
        }

        private string hashedPassword;
        public string HashedPassword
        {
            get
            {
                if (hashedPassword == null)
                    throw new ArgumentNullException();
                return hashedPassword;
            }
            set { hashedPassword = value; }
        }

        protected virtual void GenerateSalt()
        {
            Salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(Salt);
            }
        }

        public virtual void HashPassword(string password, string salt = null)
        {
            if(salt == null)
                GenerateSalt();
            else
                Salt = Convert.FromBase64String(salt);

            HashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        public async Task HashPasswordAsync(string password, string salt = null)
        {
            await Task.Run(() => HashPassword(password, salt));
        }

    }
}
