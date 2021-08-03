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

        public static string GetSummary(this ParameterInfo parameterInfo)
        {
            return $"**{Resources.Resources.txt_word_name}**: {parameterInfo.Name}\n" +
                $"**{Resources.Resources.txt_word_datatype}**: {parameterInfo.Type.Name}\n" +
                $"**{Resources.Resources.txt_phrase_isoptional}** {parameterInfo.IsOptional.ToYesNo()}\n" +
                $"{parameterInfo.Summary}\n";
        }

        #endregion Public Methods
    }
}