using DiscordEventBot.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DiscordEventBot.Service
{
    public class Settings : ISettings
    {
        #region Private Fields

        private static readonly XmlSerializer settingsSerializer = new XmlSerializer(typeof(Settings));

        #endregion Private Fields

        #region Public Constructors

        private Settings()
        {
            Token = Environment.GetEnvironmentVariable($"DiscordEventBot{nameof(Token)}");

            LogLevel = LogLevel.Information;
            string loglevel;
            if (!string.IsNullOrWhiteSpace(loglevel = Environment.GetEnvironmentVariable($"DiscordEventBot{nameof(LogLevel)}")))
                LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), loglevel, ignoreCase: true);

            DataStore = "./";
            string ds;
            if (!string.IsNullOrWhiteSpace(ds = Environment.GetEnvironmentVariable($"DiscordEventBot{nameof(DataStore)}")))
                DataStore = ds;

            Language = CultureInfo.CurrentCulture.Name;
            string lang;
            if (!string.IsNullOrWhiteSpace(lang = Environment.GetEnvironmentVariable($"DiscordEventBot{nameof(Language)}")))
                Language = lang;
        }

        #endregion Public Constructors

        #region Public Properties

        [XmlIgnore]
        public CultureInfo Culture
        {
            get
            {
                return new(Language);
            }
            set
            {
                Language = value.Name;
            }
        }

        [XmlIgnore]
        public string DataStore
        {
            get;
            set;
        }

        public string Language
        {
            get;
            set;
        }

        public LogLevel LogLevel
        {
            get;
            set;
        }

        public string SQLiteFile
        {
            get
            {
                return Path.Combine(DataStore, "EventBot.db");
            }
        }

        public string Token
        {
            get;
            set;
        }

        #endregion Public Properties

        #region Public Methods

        public static Settings Load()
        {
            string settingsFile = new Settings().GetSettingsFileName();
            if (File.Exists(settingsFile))
                using (Stream settingsStream = File.OpenRead(settingsFile))
                    return settingsSerializer.Deserialize(settingsStream) as Settings;
            else return new Settings();
        }

        public void Save()
        {
            using (XmlWriter settingsWriter = XmlWriter.Create(GetSettingsFileName()))
                settingsSerializer.Serialize(settingsWriter, this);
        }

        #endregion Public Methods

        #region Private Methods

        private string GetSettingsFileName()
        {
            return Path.Combine(Path.GetFullPath(DataStore), "settings.xml");
        }

        #endregion Private Methods
    }
}