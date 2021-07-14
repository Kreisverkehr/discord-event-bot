using Microsoft.Extensions.Logging;
using System.Globalization;

namespace DiscordEventBot.Common
{
    public interface ISettings
    {
        #region Public Properties

        CultureInfo Culture { get; set; }

        LogLevel LogLevel { get; set; }

        #endregion Public Properties
    }
}