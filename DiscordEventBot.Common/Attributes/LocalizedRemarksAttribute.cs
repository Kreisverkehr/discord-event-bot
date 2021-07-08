using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common
{
    public class LocalizedRemarksAttribute : RemarksAttribute
    {
        public LocalizedRemarksAttribute(string resourceName)
            : base(Resources.Resources.ResourceManager.GetString(resourceName))
        {
        }
    }
}
