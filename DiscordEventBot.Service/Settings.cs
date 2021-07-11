using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace DiscordEventBot.Service
{
    public class Settings
    {
        #region Public Properties

        public CultureInfo Culture
        {
            get
            {
                string lang;
                if (!string.IsNullOrWhiteSpace(lang = Environment.GetEnvironmentVariable($"DiscordEventBot{nameof(Culture)}")))
                    return new CultureInfo(lang);
                return CultureInfo.CurrentCulture;
            }
            set
            {
                Environment.SetEnvironmentVariable($"DiscordEventBot{nameof(Culture)}", value.Name);
            }
        }

        public string DataStore
        {
            get
            {
                string ds;
                if (!string.IsNullOrWhiteSpace(ds = Environment.GetEnvironmentVariable($"DiscordEventBot{nameof(DataStore)}")))
                    return ds;
                return "DiscordEventBot.db";
            }
            set
            {
                Environment.SetEnvironmentVariable($"DiscordEventBot{nameof(DataStore)}", value);
            }
        }

        public LogLevel LogLevel
        {
            get
            {
                string loglevel;
                if (!string.IsNullOrWhiteSpace(loglevel = Environment.GetEnvironmentVariable($"DiscordEventBot{nameof(LogLevel)}")))
                    return (LogLevel)Enum.Parse(typeof(LogLevel), loglevel, ignoreCase: true);
                return LogLevel.Information;
            }
            set
            {
                Environment.SetEnvironmentVariable($"DiscordEventBot{nameof(LogLevel)}", value.ToString());
            }
        }

        public string Token
        {
            get
            {
                return Environment.GetEnvironmentVariable($"DiscordEventBot{nameof(Token)}");
            }
            set
            {
                Environment.SetEnvironmentVariable($"DiscordEventBot{nameof(Token)}", value);
            }
        }

        #endregion Public Properties
    }
}