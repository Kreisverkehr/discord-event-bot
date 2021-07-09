using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscordEventBot.Model
{
    public class AttendeeGroup
    {
        #region Public Properties

        public virtual ICollection<User> Attendees { get; set; } = new List<User>();

        [Key]
        public ulong GroupID { get; set; }

        public int? MaxCapacity { get; set; }
        public string Name { get; set; }

        #endregion Public Properties
    }
}