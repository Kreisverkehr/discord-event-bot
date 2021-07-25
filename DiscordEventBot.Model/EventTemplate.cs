using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscordEventBot.Model
{
    [Index("GuildId", IsUnique = false)]
    public class EventTemplate
    {
        #region Public Properties

        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        [Key]
        public ulong EventTemplateID { get; set; }

        public virtual ICollection<AttendeeGroupTemplate> Groups { get; set; } = new List<AttendeeGroupTemplate>();

        [Required]
        public virtual Guild Guild { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        #endregion Public Properties
    }
}