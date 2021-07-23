using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Common.Messages;
using DiscordEventBot.Common.RuntimeResults;
using DiscordEventBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    [Group("event")]
    [Alias("evt")]
    [LocalizedName("txt_mod_event_name")]
    [LocalizedSummary("txt_mod_event_sum")]
    public class EventModule : ModuleBase<SocketCommandContext>
    {
        #region Public Properties

        public DiscordSocketClient Client { get; set; }

        public EventBotContext DbContext { get; set; }

        #endregion Public Properties

        #region Public Methods

        [Command("add-group")]
        [Alias("agrp")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_add-group_sum")]
        public async Task<RuntimeResult> AddGroupToEventsAsync(ulong eventId, string groupName, int? capacity = null)
        {
            Event evt = await DbContext.Events.FindAsync(eventId);

            if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_eventnotfound);

            if (evt.Groups.Count >= 11)
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_groupsfull);

            evt.Groups.Add(new AttendeeGroup()
            {
                Name = groupName,
                MaxCapacity = capacity
            });

            await DbContext.SaveChangesAsync();
            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("create")]
        [Alias("cr", "new")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_create_sum")]
        public async Task<RuntimeResult> CreateEventAsync(string subject, DateTime startDate, TimeSpan duration, [Remainder] string description = null)
        {
            var evt = new Event()
            {
                Creator = await DbContext.Users.FindOrCreateAsync(Context.User.Id),
                Subject = subject,
                Start = startDate,
                Duration = duration,
                Guild = await DbContext.Guilds.FindOrCreateAsync(Context.Guild.Id),
                Description = description
            };

            DbContext.Events.Add(evt);
            await DbContext.SaveChangesAsync();

            return await ResponseMessageResult.FromMessageAsync(new EventCreatedMessage(evt, Client, DbContext));
        }

        [Command("delete")]
        [Alias("remove", "rm", "del")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_delete_sum")]
        public async Task<RuntimeResult> DeleteEventAsync(ulong eventId)
        {
            Event evt = await DbContext.Events.FindAsync(eventId);

            if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_eventnotfound);

            DbContext.Events.Remove(evt);
            await DbContext.SaveChangesAsync();

            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("join")]
        [Alias("attend", "jn")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_join_sum")]
        public async Task<RuntimeResult> JoinEventAsync(ulong eventId, IGuildUser user = null)
        {
            IGuildUser discUser = user ?? Context.User as IGuildUser;
            Event evt = await DbContext.Events.FindAsync(eventId);
            User dbUser = await DbContext.Users.FindOrCreateAsync(discUser.Id);

            if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_eventnotfound);

            if (evt.Attendees.Contains(dbUser))
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_alreadyoined);

            evt.Attendees.Add(dbUser);

            await DbContext.SaveChangesAsync();

            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("join-group")]
        [Alias("attend-group", "jngrp")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_join-group_sum")]
        public async Task<RuntimeResult> JoinEventGroupAsync(ulong eventId, string groupName, IGuildUser user = null)
        {
            IGuildUser discUser = user ?? Context.User as IGuildUser;
            Event evt = await DbContext.Events.FindAsync(eventId);
            User dbUser = await DbContext.Users.FindOrCreateAsync(discUser.Id);

            if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_eventnotfound);

            AttendeeGroup grp = evt.Groups.Where(grp => grp.Name.ToUpperInvariant() == groupName.ToUpperInvariant()).FirstOrDefault();

            if (grp == default(AttendeeGroup))
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_eventgroupnotfound);

            if (grp.Attendees.Contains(dbUser))
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_alreadyoined);

            if (grp.MaxCapacity.HasValue && grp.Attendees.Count == grp.MaxCapacity.Value)
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_groupfull);

            grp.Attendees.Add(dbUser);

            await DbContext.SaveChangesAsync();

            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("leave")]
        [Alias("unattend", "lv")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_leave_sum")]
        public async Task<RuntimeResult> LeaveEventAsync(ulong eventId, IGuildUser user = null)
        {
            IGuildUser discUser = user ?? Context.User as IGuildUser;
            Event evt = await DbContext.Events.FindAsync(eventId);
            User dbUser = await DbContext.Users.FindOrCreateAsync(discUser.Id);

            if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_eventnotfound);

            if (!evt.Attendees.Contains(dbUser))
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_notjoined);

            evt.Attendees.Remove(dbUser);

            await DbContext.SaveChangesAsync();

            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("leave-group")]
        [Alias("unattend-group", "lvgrp")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_leave-group_sum")]
        public async Task<RuntimeResult> LeaveEventGroupAsync(ulong eventId, string groupName, IGuildUser user = null)
        {
            IGuildUser discUser = user ?? Context.User as IGuildUser;
            Event evt = await DbContext.Events.FindAsync(eventId);
            User dbUser = await DbContext.Users.FindOrCreateAsync(discUser.Id);

            if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_eventnotfound);

            AttendeeGroup grp = evt.Groups.Where(grp => grp.Name.ToUpperInvariant() == groupName.ToUpperInvariant()).FirstOrDefault();

            if (grp == default(AttendeeGroup))
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_eventgroupnotfound);

            if (!grp.Attendees.Contains(dbUser))
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_notjoined);

            grp.Attendees.Remove(dbUser);

            await DbContext.SaveChangesAsync();

            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("show")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_show_sum")]
        public async Task<RuntimeResult> ShowEventAsync(ulong eventId)
        {
            Event evt = await DbContext.Events.FindAsync(eventId);

            if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_eventnotfound);

            return await ResponseMessageResult.FromMessageAsync(new EventInfoMessage(evt, Client, DbContext));
        }

        [Command("show-next")]
        [Alias("upcomming")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_show-next_sum")]
        public async Task<RuntimeResult> ShowUpcommingEventsAsync(int count = 10)
        {
            IEnumerable<Event> upcommingEvents =
                from evt in DbContext.Events.AsQueryable()
                where evt.Guild.GuildId == Context.Guild.Id
                   && evt.Start > DateTime.Now
                orderby evt.Start descending
                select evt;

            upcommingEvents = upcommingEvents.Take(count);
            return await ResponseMessageResult.FromMessageAsync(new EventListMessage(upcommingEvents));
        }

        #endregion Public Methods
    }
}