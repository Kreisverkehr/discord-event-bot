using System.Text.Json.Serialization;

namespace DiscordEventBot.Common
{
    public class Settings
    {
        #region Public Fields

        public const string SETTINGS_FILE = "settings.json";

        #endregion Public Fields

        #region Public Properties

        public string DataStore { get; set; } = "DiscordEventBot.db";

        public DiscordClientConfig DiscordClientConfig { get; set; } = new();

        [JsonIgnore]
        public bool IsLoadedFromFile { get; set; } = false;

        public string Token { get; set; }

        #endregion Public Properties
    }
}