using Discord.Commands;

namespace DiscordEventBot.Common
{
    public class LocalizedSummaryAttribute : SummaryAttribute
    {
        #region Public Constructors

        public LocalizedSummaryAttribute(string resourceName)
            : base(Resources.Resources.ResourceManager.GetString(resourceName))
        {
        }

        #endregion Public Constructors
    }
}