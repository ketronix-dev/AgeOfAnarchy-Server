using Appwrite.Models;
using Crypto;
using Database;

namespace UserManagment
{
    public class UserManager
    {
        public static async Task CreateUser(string FirstName, string LastName, string Login, string Password)
        {
            await DatabaseUtils.CreateUser(FirstName, LastName, Login, LibCrypto.Encrypt(Password));
        }

        public static async Task<string?> Login(string Login, string Password)
        {
            var user = await DatabaseUtils.GetUser(Login, Password);
            if (user != null && LibCrypto.Verify(Password, user.Password))
            {
                var accessToken = GenerateAccessToken();
                await DatabaseUtils.PushAccessTokenAsync(user.Id, accessToken);

                Console.WriteLine(accessToken); 

                return accessToken;
            }
            return null;
        }

        public static async Task<User?> GetUser(string token)
        {
            return await DatabaseUtils.GetUserByToken(token);
        }


        public static string GenerateAccessToken(int length = 100)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be a positive integer.");

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var token = new char[length];

            for (int i = 0; i < length; i++)
            {
                token[i] = chars[random.Next(chars.Length)];
            }

            return new string(token);
        }

    }
}