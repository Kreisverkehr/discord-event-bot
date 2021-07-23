using Discord.WebSocket;
using DiscordEventBot.Model;

namespace DiscordEventBot.Common.Messages
{
    public class EventCreatedMessage : EventInfoMessage
    {
        #region Public Constructors

        public EventCreatedMessage(Event evt, DiscordSocketClient client, EventBotContext context) : base(evt, client, context)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override string BuildMessageText()
        {
            return Resources.Resources.txt_msg_eventcreated;
        }

        #endregion Protected Methods
    }
}