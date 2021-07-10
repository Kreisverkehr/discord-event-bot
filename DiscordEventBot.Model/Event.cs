using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordEventBot.Model
{
    [Index("GuildId", IsUnique = false)]
    public class Event
    {
        #region Public Properties

        public virtual ICollection<User> Attendees { get; set; } = new List<User>();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Required]
        public virtual User Creator { get; set; }

        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        [Key]
        public ulong EventID { get; set; }

        public virtual ICollection<AttendeeGroup> Groups { get; set; } = new List<AttendeeGroup>();

        [Required]
        public virtual Guild Guild { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public DateTime Start { get; set; }

        public string Subject { get; set; }

        #endregion Public Properties
    }
}