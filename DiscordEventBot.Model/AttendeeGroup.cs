using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscordEventBot.Model
{
    public class AttendeeGroup
    {
        #region Public Properties

        public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

        [Key]
        public Guid GroupID { get; set; }

        public int? MaxCapacity { get; set; }
        public string Name { get; set; }

        #endregion Public Properties
    }
}