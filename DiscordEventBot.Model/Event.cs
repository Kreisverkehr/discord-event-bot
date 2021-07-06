using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscordEventBot.Model
{
    public class Event
    {
        #region Public Properties

        public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

        public TimeSpan Duration { get; set; }

        [Key]
        public Guid EventID { get; set; }

        public virtual ICollection<AttendeeGroup> Groups { get; set; } = new List<AttendeeGroup>();
        public DateTimeOffset Start { get; set; }
        public string Subject { get; set; }

        #endregion Public Properties
    }
}