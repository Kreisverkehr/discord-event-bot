using System.ComponentModel.DataAnnotations;

namespace DiscordEventBot.Model
{
    /// <summary>
    /// represents a single discord user
    /// </summary>
    /// <remarks> for now, we don't need to save anything else besides the userid </remarks>
    public class Guild
    {
        #region Public Constructors

        public Guild()
        {
        }

        public Guild(ulong id) : this()
        {
            GuildId = id;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// discords userid.
        /// </summary>
        /// <remarks> I think we can trust discord that this is unique. </remarks>
        [Key]
        public ulong GuildId { get; set; }

        public virtual Role AdminRole { get; set; }
        public virtual Channel BotChannel { get; set; }

        #endregion Public Properties
    }
}