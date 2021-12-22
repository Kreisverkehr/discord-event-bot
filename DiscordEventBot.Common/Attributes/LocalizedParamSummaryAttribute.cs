using Discord.Interactions;

namespace DiscordEventBot.Common
{
    public class LocalizedParamSummaryAttribute : SummaryAttribute
    {
        #region Public Constructors

        public LocalizedParamSummaryAttribute(string name, string resourceDescription)
            : base(name, Resources.Resources.ResourceManager.GetString(resourceDescription))
        {
        }

        #endregion Public Constructors
    }
}