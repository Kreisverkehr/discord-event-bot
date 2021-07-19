using System.ComponentModel.DataAnnotations;

namespace DiscordEventBot.Model
{
    public class Channel
    {
        #region Public Constructors

        public Channel()
        {
        }

        public Channel(ulong id) : this()
        {
            ChannelId = id;
        }

        #endregion Public Constructors

        #region Public Properties

        [Key]
        public ulong ChannelId { get; set; }

        #endregion Public Properties
    }
}