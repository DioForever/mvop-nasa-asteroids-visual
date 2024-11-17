using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nasa_asteroid_visualizer
{
    public static class APIKeyHandler
    {
        // Save API key in a file
        private static string apiKeyPath() {
            appDataPath = FileSystem.AppDataDirectory;
            return Path.Combine(appDataPath, "APIKey.txt");
        }
        public static string APIKEY = "";
        public static string appDataPath = FileSystem.AppDataDirectory;

        public static void LoadAPIKey()
        {
            if (File.Exists(apiKeyPath())) APIKEY = File.ReadAllText(apiKeyPath());
            else APIKEY = "";
        }

        public static void SaveAPIKey(string key)
        {
            APIKEY = key;
            File.WriteAllText(apiKeyPath(), key);
        }

        public static string Get(){
            LoadAPIKey();
            return APIKEY;
        }
    }
}
