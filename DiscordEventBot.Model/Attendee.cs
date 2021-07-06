using System;

namespace DiscordEventBot.Model
{
    public class Attendee
    {
        #region Public Properties

        public Guid AttendeeID { get; set; }
        public string DiscordUserDiscriminator { get; set; }
        public ulong DiscordUserID { get; set; }

        #endregion Public Properties
    }
}