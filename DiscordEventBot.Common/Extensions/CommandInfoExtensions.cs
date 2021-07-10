using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Extensions
{
    public static class CommandInfoExtensions
    {
        public static string GetSignature(this CommandInfo command)
        {
            var sig = string.Empty;
            if (!string.IsNullOrWhiteSpace(command.Module.Group))
                sig += $"{command.Module.Group} ";
            sig += $"**{command.Name}** ";
            foreach (var param in command.Parameters)
                sig += param.FormatMd() + " ";
            return sig;
        }
    }
}
