using DiscordEventBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Extensions
{
    public static class AttendeeGroupExtensions
    {
        public static string GetTitle(this AttendeeGroup group)
        {
            string result = $"{group.Name} [{group.Attendees?.Count ?? 0}";
            if (group.MaxCapacity.HasValue)
                result += $"/{group.MaxCapacity.Value}";
            result += "]";
            return result;
        }
    }
}
