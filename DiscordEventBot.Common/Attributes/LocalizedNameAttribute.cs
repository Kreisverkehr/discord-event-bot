using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common
{
    public class LocalizedNameAttribute : NameAttribute
    {
        public LocalizedNameAttribute(string resourceName)
            : base(Resources.Resources.ResourceManager.GetString(resourceName))
        {
        }
    }
}
