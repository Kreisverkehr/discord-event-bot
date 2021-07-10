using Discord;
using Discord.WebSocket;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Model;
using Humanizer;
using System.Linq;

namespace DiscordEventBot.Common.Messages
{
    public class EventInfoMessage : MessageBase
    {
        #region Private Fields

        private DiscordSocketClient _client;
        private Event _event;

        #endregion Private Fields

        #region Public Constructors

        public EventInfoMessage(Event evt, DiscordSocketClient client)
        {
            _event = evt;
            _client = client;
            HasEmbed = true;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder) => base.BuildEmbed(embedBuilder)
                .WithTitle($"[#{_event.EventID}] {_event.Subject}")
                .WithDescription(_event.Description)
                .WithColor(Color.Orange)
                .WithTimestamp(_event.CreatedOn)
                .WithAuthor(_client.GetUser(_event.Creator.UserId))
                .AddField(fb => fb
                    .WithName(Resources.Resources.txt_word_starttime)
                    .WithValue(_event.Start.ToString("f"))
                )
                .AddFieldIf(() => _event.Attendees != null && _event.Attendees.Count > 0, fb => fb
                    .WithName(Resources.Resources.txt_word_attendee.ToQuantity(_event.Attendees.Count, ShowQuantityAs.None))
                    .WithValue(string.Join('\n', _event.Attendees.Select(at => _client.GetUser(at.UserId).Mention)))
                )
                .WithFields(_event.Groups.Select(grp => new EmbedFieldBuilder()
                    .WithIsInline(true)
                    .WithName(grp.GetTitle())
                    .WithValue(string.Join('\n', grp.Attendees.Select(at => _client.GetUser(at.UserId).Mention)) + ".")
                ))
                ;

        #endregion Protected Methods
    }
}