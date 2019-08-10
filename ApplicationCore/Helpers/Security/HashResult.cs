using System;

namespace ApplicationCore.Helpers.Security
{
    public sealed class HashResult
    {
        public string HashedContent { get; private set; }
        public string Salt { get; private set; }
        public int Iterations { get; private set; }

        public static explicit operator HashResult(string result)
        {
            if(result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            string[] parts = result.Split(".");

            if(parts.Length != 3)
            {
                throw new FormatException("Wrong format");
            }

            return new HashResult
            {
                Iterations = Convert.ToInt32(parts[0]),
                Salt = parts[1],
                HashedContent = parts[2],
            };
        }

        public static explicit operator string(HashResult result)
        {
            return result.ToString();
        }

        public override string ToString() => $"{Iterations}.{Salt}.{HashedContent}";
    }
}
