using Discord;
using Microsoft.Extensions.Logging;

namespace DiscordEventBot.Common.Extensions
{
    public static class LogSeverityExtensions
    {
        #region Public Methods

        public static LogLevel ToLogLevel(this LogSeverity logSeverity)
        {
            switch (logSeverity)
            {
                case LogSeverity.Debug:
                    return LogLevel.Trace;

                case LogSeverity.Verbose:
                    return LogLevel.Debug;

                case LogSeverity.Info:
                    return LogLevel.Information;

                case LogSeverity.Warning:
                    return LogLevel.Warning;

                case LogSeverity.Error:
                    return LogLevel.Error;

                case LogSeverity.Critical:
                    return LogLevel.Critical;
            }

            return LogLevel.None;
        }

        #endregion Public Methods
    }
}