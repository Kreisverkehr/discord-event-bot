using Discord;
using DiscordEventBot.Common.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DiscordEventBot.Service
{
    public static class ConsoleLogger
    {
        #region Public Properties

        public static Func<LogLevel, string, bool> Filter { get; set; } = (logLevel, source) => true;

        #endregion Public Properties

        #region Public Methods

        public static void Log(EventData eventData) =>
            WriteLog(eventData.LogLevel, eventData.EventId.ToString(), eventData.ToString());

        public static void Log(LogLevel logLevel, string message) =>
            WriteLog(logLevel, "Terminal", message);

        public static Task LogAsync(LogMessage discordLogMessage)
        {
            WriteLog(discordLogMessage.Severity.ToLogLevel(), discordLogMessage.Source, $"{discordLogMessage.Message} {discordLogMessage.Exception}");

            return Task.CompletedTask;
        }

        #endregion Public Methods

        #region Private Methods

        private static void WriteLog(LogLevel logLevel, string source, string message)
        {
            if (!Filter(logLevel, source)) return;

            switch (logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case LogLevel.Trace:
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19:yyyy-MM-dd HH:mm:ss} [{logLevel,11}] {source}: {message}");
            Console.ResetColor();
        }

        #endregion Private Methods
    }
}