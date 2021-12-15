using Discord.WebSocket;
using DiscordEventBot.Model;

namespace DiscordEventBot.Common.Messages
{
    public class EventUpdatedMessage : EventInfoMessage
    {
        #region Public Constructors

        public EventUpdatedMessage(Event evt, DiscordSocketClient client, EventBotContext context) : base(evt, client, context)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override string BuildMessageText()
        {
            return Resources.Resources.txt_msg_eventupdated;
        }

        #endregion Protected Methods
    }
}