using Discord;
using DiscordEventBot.Model;
using Humanizer;
using System.Collections.Generic;
using System.Linq;

namespace DiscordEventBot.Common.Messages
{
    public class EventListMessage : MessageBase
    {
        #region Private Fields

        private IEnumerable<Event> _events;

        #endregion Private Fields

        #region Public Constructors

        public EventListMessage(IEnumerable<Event> events)
        {
            _events = events;
            HasEmbed = true;
        }

        #endregion Public Constructors

        #region Protected Methods

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