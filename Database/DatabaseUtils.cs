using Appwrite;
using Appwrite.Models;
using Appwrite.Services;

namespace Database
{
    public class DatabaseUtils
    {
        public static Databases? databases;
        public static Client? client;

        public static void Init()
        {
            client = new Client();
            client
                .SetEndpoint("https://base.mindenit.tech/v1")
                .SetProject("age-of-anarchy-b1")
                .SetKey("api-key");
            databases = new Databases(client);
        }

        public static async Task CreateUser(string FirstName, string LastName, string Login, string PasswordHash)
        {
            var db = await databases.Get("databaseID");
            var dbUser = await databases.GetCollection("databaseID", "collectionID");

            await databases.CreateDocument(
                databaseId: db.Id,
                collectionId: dbUser.Id,
                documentId: ID.Unique(),
                data: new Dictionary<string, object>
                {
                    {"firstName", FirstName},
                    {"lastName", LastName},
                    {"login", Login},
                    {"password", PasswordHash},
                    {"registrationDate", DateTime.Now}
                }
            );
        }

        public static async Task PushAccessTokenAsync(string documentId, string accessToken)
        {
            var db = await databases.Get("databaseID");
            var dbUser = await databases.GetCollection("databaseID", "collectionID");

            await databases.UpdateDocument(
                databaseId: db.Id,
                collectionId: dbUser.Id,
                documentId: documentId,
                data: new Dictionary<string, object>
                {
                    {"accessToken", accessToken}
                }
            );
        }

        public static async Task<User?> GetUser(string Login, string PasswordHash)
        {
            var db = await databases.Get("databaseID");
            var dbUser = await databases.GetCollection("databaseID", "collectionID");

            var users = await databases.ListDocuments(
                databaseId: db.Id,
                collectionId: dbUser.Id
            );

            foreach (var user in users.Documents)
            {
                if (user.Data["login"].ToString() == Login)
                {
                    return new User
                    {
                        Id = user.Id,
                        FirstName = user.Data["firstName"].ToString(),
                        LastName = user.Data["lastName"].ToString(),
                        Login = user.Data["login"].ToString(),
                        Password = user.Data["password"].ToString(),
                        RegistrationDate = DateTime.Parse(user.Data["registrationDate"].ToString())
                    };
                }
            }

            return null;
        }


    }
}