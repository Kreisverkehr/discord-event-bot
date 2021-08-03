using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscordEventBot.Model
{
    /// <summary>
    /// represents a single discord user
    /// </summary>
    /// <remarks> for now, we don't need to save anything else besides the userid </remarks>
    public class User
    {
        #region Public Constructors

        public User()
        {
        }

        public User(ulong id) : this()
        {
            UserId = id;
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<AttendeeGroup> Groups { get; set; }

        public int MinutesBeforeEventNotify { get; set; }

        /// <summary>
        /// discords userid.
        /// </summary>
        /// <remarks> I think we can trust discord that this is unique. </remarks>
        [Key]
        public ulong UserId { get; set; }

        #endregion Public Properties
    }
}