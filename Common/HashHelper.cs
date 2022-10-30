using Isopoh.Cryptography.Argon2;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class HashHelper
    {
        public static string GetHash(string value)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(value);
            return Argon2.Hash(password: passwordBytes, RandomNumberGenerator.GetBytes(16), timeCost: 10, parallelism: Environment.ProcessorCount);
        }
    }
}
