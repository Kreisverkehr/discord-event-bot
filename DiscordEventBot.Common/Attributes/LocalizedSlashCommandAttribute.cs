using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common
{
    public class LocalizedSlashCommandAttribute: SlashCommandAttribute
    {
        public LocalizedSlashCommandAttribute(string name, string resourceDescription, bool ignoreGroupNames = false, RunMode runMode = RunMode.Default)
            : base(name, Resources.Resources.ResourceManager.GetString(resourceDescription), ignoreGroupNames, runMode)
        { }
    }
}
