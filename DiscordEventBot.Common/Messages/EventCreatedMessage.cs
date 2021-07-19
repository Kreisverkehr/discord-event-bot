using Discord.WebSocket;
using DiscordEventBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Messages
{
    public class EventCreatedMessage : EventInfoMessage
    {
        public EventCreatedMessage(Event evt, DiscordSocketClient client, EventBotContext context) : base(evt, client, context)
        {
        }
        protected override string BuildMessageText()
        {
            return Resources.Resources.txt_msg_eventcreated;
        }
    }
}
