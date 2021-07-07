using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Extensions
{
    public static class ParameterInfoExtensions
    {
        public static string FormatMd(this ParameterInfo parameterInfo)
        {
            string paramString = $"{parameterInfo.Name}:*{parameterInfo.Type.Name}*";
            if (parameterInfo.IsOptional) paramString = $"[{paramString}]";
            if (parameterInfo.IsMultiple) paramString += "[\\*]";
            return paramString;
        }
    }
}
