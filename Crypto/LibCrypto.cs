using BC = BCrypt.Net;

namespace Crypto
{
    public class LibCrypto
    {
        public static string Encrypt(string password)
        {
            return BC.BCrypt.HashPassword(password);
        }

        public static bool Verify(string password, string hash)
        {
            return BC.BCrypt.Verify(password, hash);
        }
        
    }
}