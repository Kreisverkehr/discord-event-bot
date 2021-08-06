using Discord;
using Discord.Commands;
using DiscordEventBot.Model;
using System;
using System.Globalization;

namespace DiscordEventBot.Common.Extensions
{
    public static class ParameterInfoExtensions
    {
        #region Private Fields

        private static Random random = new();

        #endregion Private Fields

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

        public static string GetExample(this ParameterInfo parameterInfo)
        {
            #region Basic types

            // Just text, return a static text here.
            if (parameterInfo.Type == typeof(string) && !parameterInfo.IsRemainder)
                return $"\"Sample {parameterInfo.Name}\"";
            if (parameterInfo.Type == typeof(string) && parameterInfo.IsRemainder)
                return $"Sample {parameterInfo.Name}";

            // Just a number, return a random number here
            if (parameterInfo.Type == typeof(int)
                || parameterInfo.Type == typeof(uint)
                || parameterInfo.Type == typeof(long)
                || parameterInfo.Type == typeof(ulong)
                || parameterInfo.Type == typeof(short)
                || parameterInfo.Type == typeof(ushort))
                return random.Next(short.MaxValue).ToString();

            // Just a number, but with decimal places.
            if (parameterInfo.Type == typeof(decimal)
                || parameterInfo.Type == typeof(float)
                || parameterInfo.Type == typeof(double))
                return random.NextDouble().ToString();

            #endregion Basic types

            #region Time & Dates

            // Return a sample DateTime in the format that the standard typereader expects
            if (parameterInfo.Type == typeof(DateTime))
            {
                string date = DateTime.Now.AddMilliseconds(random.NextDouble()).ToString("g");
                if (!parameterInfo.IsRemainder) return $"\"{date}\"";
                return date;
            }

            // Return a sample TimeSpan in the format that the standard typereader expects
            if (parameterInfo.Type == typeof(TimeSpan))
                return new TimeSpan(random.Next(8), random.Next(1) * 30, 0).ToString("h\\Hm\\M");

            #endregion Time & Dates

            #region Environment specific

            if (parameterInfo.Type == typeof(CultureInfo))
                return CultureInfo.CurrentCulture.Name;

            if (parameterInfo.Type == typeof(ModuleInfo))
                return parameterInfo.Command.Module.Service.GetRandomModule().Aliases[0];

            if (parameterInfo.Type == typeof(CommandInfo))
                return parameterInfo.Command.Module.Service.GetRandomModule().GetRandomCommand().Aliases[0];

            if (parameterInfo.Type == typeof(EventTemplate))
                return "myawesometemplate";

            #endregion Environment specific

            #region special strings

            if (parameterInfo.Type == typeof(IRole))
                return "rolename";

            if (parameterInfo.Type == typeof(IGuildChannel))
                return "mybotchannel";

            if (parameterInfo.Type == typeof(IGuildUser))
                return "@someuser";

            #endregion special strings

            return string.Empty;
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