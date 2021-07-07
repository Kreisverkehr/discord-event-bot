using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscordEventBot.Service
{
    public class Settings
    {
        #region Public Fields

        public const string SETTINGS_FILE = "settings.json";

        #endregion Public Fields

        #region Public Properties

        public string DataStore { get; set; } = "DiscordEventBot.db";
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        public string Token { get; set; }

        #endregion Public Properties

        #region Public Methods

        public async Task<bool> LoadFromFile()
        {
            if (!File.Exists(SETTINGS_FILE))
                return false;

            Settings tmp;
            using (Stream settingsFileStream = File.OpenRead(SETTINGS_FILE))
                tmp = await JsonSerializer.DeserializeAsync<Settings>(settingsFileStream);
            Token = tmp.Token;
            DataStore = tmp.DataStore;
            return true;
        }

        #endregion Public Methods
    }
}