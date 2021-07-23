using Discord.Commands;

namespace DiscordEventBot.Common
{
    public class LocalizedNameAttribute : NameAttribute
    {
        #region Public Constructors

        public LocalizedNameAttribute(string resourceName)
            : base(Resources.Resources.ResourceManager.GetString(resourceName))
        {
        }

        #endregion Public Constructors
    }
}