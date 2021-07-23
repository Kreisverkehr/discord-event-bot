using Discord.Commands;

namespace DiscordEventBot.Common
{
    public class LocalizedRemarksAttribute : RemarksAttribute
    {
        #region Public Constructors

        public LocalizedRemarksAttribute(string resourceName)
            : base(Resources.Resources.ResourceManager.GetString(resourceName))
        {
        }

        #endregion Public Constructors
    }
}