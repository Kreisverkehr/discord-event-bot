using Discord.WebSocket;
using DiscordEventBot.Model;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System.Collections.Generic;
using System.Linq;

namespace DiscordEventBot.Common.Extensions
{
    public static class EventExtensions
    {
        #region Public Methods

        public static IEnumerable<User> GetAttendingUsers(this Event evt)
        {
            foreach (var usr in evt.Attendees)
                yield return usr;

            foreach (var grp in evt.Groups)
                foreach (var usr in grp.Attendees)
                    yield return usr;
        }

        public static Calendar ToCalendar(this IEnumerable<Event> events, DiscordSocketClient client)
        {
            Calendar cal = new();
            cal.Events.AddRange(events.Select(e => e.ToCalendarEvent(client)));
            return cal;
        }

        public static CalendarEvent ToCalendarEvent(this Event evt, DiscordSocketClient client) => new()
        {
            Start = new CalDateTime(evt.Start),
            End = new CalDateTime(evt.Start + evt.Duration),
            Created = (CalDateTime)evt.CreatedOn,
            Summary = evt.Subject,
            Description = evt.Description,
            Attendees = evt.GetAttendingUsers().Distinct().Select(u => new Attendee()
            {
                CommonName = client.GetGuild(evt.Guild.GuildId).GetUser(u.UserId).Nickname,
                Value = new("https://discordapp.com/users/" + u.UserId),
            }).ToList(),
        };

        #endregion Public Methods
    }
}