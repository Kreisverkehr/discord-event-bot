using Discord;
using Discord.WebSocket;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Model;
using Humanizer;
using Ical.Net.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiscordEventBot.Common.Messages
{
    public class EventListMessage : MessageBase
    {
        #region Private Fields

        private DiscordSocketClient _client;
        private IEnumerable<Event> _events;
        private CalendarSerializer calendarSerializer = new();

        #endregion Private Fields

        #region Public Constructors

        public EventListMessage(IEnumerable<Event> events, DiscordSocketClient client)
        {
            _events = events;
            _client = client;
            HasEmbed = true;
            HasAttachment = true;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override Stream BuildAttachment(ref string name)
        {
            name = "upcomming.ics";
            MemoryStream data = new();
            calendarSerializer.Serialize(_events.ToCalendar(_client), data, Encoding.UTF8);
            data.Flush();
            data.Position = 0;
            return data;
        }

        protected override EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder) => base.BuildEmbed(embedBuilder)
            .WithTitle(Resources.Resources.txt_msg_eventlist_title)
            .WithFields(_events.Select(evt => new EmbedFieldBuilder()
                .WithName($"[#{evt.EventID}] {evt.Subject}")
                .WithValue($"{Resources.Resources.txt_word_starttime}: {evt.Start.Humanize()}\n{Resources.Resources.txt_word_attendee.ToQuantity(evt.Attendees?.Count ?? 0)}")
            ))
            .WithFooter(Resources.Resources.txt_msg_eventlist_footer)
            ;

        #endregion Protected Methods
    }
}