using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common
{
    internal class LocalizedGroupAttribute : GroupAttribute
    {
        public LocalizedGroupAttribute(string name, string resourceDescription)
            : base(name, Resources.Resources.ResourceManager.GetString(resourceDescription)) { }
    }
}
