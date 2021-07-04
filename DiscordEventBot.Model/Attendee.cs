using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Model
{
    public class Attendee
    {
        public Guid AttendeeID { get; set; }
        public string DiscordUserDiscriminator { get; set; }
        public ulong DiscordUserID { get; set; }
    }
}
