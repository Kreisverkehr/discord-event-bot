using Discord;
using Discord.WebSocket;
using DiscordEventBot.Model;
using Humanizer;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Messages
{
    public class EventUpcommingMessage : EventInfoMessage
    {
        #region Public Constructors

        public EventUpcommingMessage(Event evt, DiscordSocketClient client, EventBotContext context) : base(evt, client, context)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override Task Sent(IUserMessage message, CancellationToken token) => Task.CompletedTask;

        #endregion Public Methods

        #region Protected Methods

        protected override string BuildMessageText()
        {
            return string.Format(Resources.Resources.txt_msg_eventupcomming, Event.Start.Humanize(utcDate: false));
        }

        #endregion Protected Methods
    }
}