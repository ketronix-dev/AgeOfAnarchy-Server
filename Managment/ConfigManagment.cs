using System.Text.Json;

namespace ConfigManagment
{
    public class ConfigManager
    {
        public static string? baseUrl;
        public static string? apiKey;
        public static string? databaseId;
        public static string? collectionId;

        public static void LoadConfig()
        {
            var json = File.ReadAllText("config.json");
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            baseUrl = config["baseUrl"];
            apiKey = config["apiKey"];
            databaseId = config["databaseId"];
            collectionId = config["collectionId"];
        }

    }
}