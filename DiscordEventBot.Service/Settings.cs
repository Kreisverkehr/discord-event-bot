using Microsoft.Extensions.Logging;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiscordEventBot.Service
{
    public class Settings
    {
        #region Public Fields

        public const string SETTINGS_FILE = "settings.json";

        #endregion Public Fields

        #region Public Properties

        [JsonIgnore]
        public CultureInfo Culture { get; set; } = CultureInfo.CurrentUICulture;

        public string DataStore { get; set; } = "DiscordEventBot.db";

        public string Language
        {
            get { return Culture.Name; }
            set
            {
                Culture = new CultureInfo(value);
            }
        }

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
            Culture = tmp.Culture;
            LogLevel = tmp.LogLevel;
            return true;
        }

        #endregion Public Methods
    }
}