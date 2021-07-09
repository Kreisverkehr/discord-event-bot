using Discord;
using Discord.WebSocket;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Model;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Messages
{
    public class EventInfoMessage : MessageBase
    {
        private Event _event;
        DiscordSocketClient _client;
        public EventInfoMessage(Event evt, DiscordSocketClient client)
        {
            _event = evt;
            _client = client;
            HasEmbed = true;
        }

        protected override EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder)
        {
            return base.BuildEmbed(embedBuilder)
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
                ;
        }
    }
}
