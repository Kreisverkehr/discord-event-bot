using Discord;
using Discord.WebSocket;

namespace DiscordEventBot.Common
{
    public class DiscordClientConfig
    {
        #region Private Fields

        private DiscordSocketConfig _config = new();

        #endregion Private Fields

        #region Public Properties

        public bool AlwaysDownloadUsers
        {
            get { return _config.AlwaysDownloadUsers; }
            set { _config.AlwaysDownloadUsers = value; }
        }

        public int ConnectionTimeout
        {
            get { return _config.ConnectionTimeout; }
            set { _config.ConnectionTimeout = value; }
        }

        public RetryMode DefaultRetryMode
        {
            get { return _config.DefaultRetryMode; }
            set { _config.DefaultRetryMode = value; }
        }

        public bool? ExclusiveBulkDelete
        {
            get { return _config.ExclusiveBulkDelete; }
            set { _config.ExclusiveBulkDelete = value; }
        }

        public string GatewayHost
        {
            get { return _config.GatewayHost; }
            set { _config.GatewayHost = value; }
        }

        public GatewayIntents? GatewayIntents
        {
            get { return _config.GatewayIntents; }
            set { _config.GatewayIntents = value; }
        }

        public bool GuildSubscriptions
        {
            get { return _config.GuildSubscriptions; }
            set { _config.GuildSubscriptions = value; }
        }

        public int? HandlerTimeout
        {
            get { return _config.HandlerTimeout; }
            set { _config.HandlerTimeout = value; }
        }

        public int IdentifyMaxConcurrency
        {
            get { return _config.IdentifyMaxConcurrency; }
            set { _config.IdentifyMaxConcurrency = value; }
        }

        public int LargeThreshold
        {
            get { return _config.LargeThreshold; }
            set { _config.LargeThreshold = value; }
        }

        public LogSeverity LogLevel
        {
            get { return _config.LogLevel; }
            set { _config.LogLevel = value; }
        }

        public int MaxWaitBetweenGuildAvailablesBeforeReady
        {
            get { return _config.MaxWaitBetweenGuildAvailablesBeforeReady; }
            set { _config.MaxWaitBetweenGuildAvailablesBeforeReady = value; }
        }

        public int MessageCacheSize
        {
            get { return _config.MessageCacheSize; }
            set { _config.MessageCacheSize = value; }
        }

        public RateLimitPrecision RateLimitPrecision
        {
            get { return _config.RateLimitPrecision; }
            set { _config.RateLimitPrecision = value; }
        }

        public int? ShardId
        {
            get { return _config.ShardId; }
            set { _config.ShardId = value; }
        }

        public int? TotalShards
        {
            get { return _config.TotalShards; }
            set { _config.TotalShards = value; }
        }

        public bool UseSystemClock
        {
            get { return _config.UseSystemClock; }
            set { _config.UseSystemClock = value; }
        }

        #endregion Public Properties

        #region Public Methods

        public static implicit operator DiscordSocketConfig(DiscordClientConfig config)
        {
            return config._config;
        }

        #endregion Public Methods
    }
}