using Discord.Commands;

namespace DiscordEventBot.Common.Extensions
{
    public static class ParameterInfoExtensions
    {
        #region Public Methods

        public static string FormatMd(this ParameterInfo parameterInfo, bool withType = true)
        {
            string paramString = $"{parameterInfo.Name}";
            if (withType)
                paramString += $":*{parameterInfo.Type.Name}*";
            if (parameterInfo.IsOptional) paramString = $"[{paramString}]";
            if (parameterInfo.IsMultiple) paramString += "[\\*]";
            return paramString;
        }

        #endregion Public Methods
    }
}