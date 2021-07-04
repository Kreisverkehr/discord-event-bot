using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscordEventBot.Model
{
    public class Event
    {
        [Key]
        public Guid EventID { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset Start { get; set; }
        public TimeSpan Duration { get; set; }
        public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
        public virtual ICollection<AttendeeGroup> Groups { get; set; } = new List<AttendeeGroup>();
    }
}
