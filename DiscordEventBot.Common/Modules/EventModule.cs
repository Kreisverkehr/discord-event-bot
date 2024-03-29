﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Common.Messages;
using DiscordEventBot.Common.RuntimeResults;
using DiscordEventBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    [Group("event")]
    [Alias("evt")]
    [LocalizedName("txt_mod_event_name")]
    [LocalizedSummary("txt_mod_event_sum")]
    public class EventModule : ModuleBase<SocketCommandContext>
    {
        #region Private Fields

        private static readonly int MAX_GROUPS = 11;

        #endregion Private Fields

        #region Public Properties

        public DiscordSocketClient Client { get; set; }

        public EventBotContext DbContext { get; set; }

        public CancellationTokenSource TokenSource { get; set; }

        #endregion Public Properties

        #region Public Methods

        [Command("add-group")]
        [Alias("agrp")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_add-group_sum")]
        public async Task<RuntimeResult> AddGroupToEventsAsync(
            [LocalizedSummary("txt_mod_event_cmd_addgroup_param_eventid_sum")]
            ulong eventId,
            [LocalizedSummary("txt_mod_event_cmd_addgroup_param_groupname_sum")]
            string groupName,
            [LocalizedSummary("txt_mod_event_cmd_addgroup_param_capacity_sum")]
            int capacity = 0)
        {
            Event evt = await DbContext.Events.FindAsync(eventId);

            if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_eventnotfound);

            if (evt.Groups.Count >= MAX_GROUPS)
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_groupsfull);

            evt.Groups.Add(new AttendeeGroup()
            {
                Name = groupName,
                MaxCapacity = capacity == 0 ? null : capacity
            });

            await DbContext.SaveChangesAsync();
            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("create")]
        [Alias("cr", "new")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_create_sum")]
        public async Task<RuntimeResult> CreateEventAsync(
            [LocalizedSummary("txt_mod_event_cmd_create_param_subject_sum")]
            string subject,
            [LocalizedSummary("txt_mod_event_cmd_create_param_startdate_sum")]
            DateTime startDate,
            [LocalizedSummary("txt_mod_event_cmd_create_param_duration_sum")]
            TimeSpan duration,
            [Remainder]
            [LocalizedSummary("txt_mod_event_cmd_create_param_description_sum")]
            string description = null)
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

            var eventAnnouncement = await ResponseMessageResult.FromMessageAsync(new EventCreatedMessage(evt, Client, DbContext)) as ResponseMessageResult;
            if (evt.Guild.AnnouncementChannel != null)
            {
                var channel = Context.Guild.GetTextChannel(evt.Guild.AnnouncementChannel.ChannelId);
                await eventAnnouncement.SendAsync(channel, TokenSource.Token);
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
            }

            return eventAnnouncement;
        }

        [Command("create-from-template")]
        [Alias("crftpl", "newftpl")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_createftpl_sum")]
        public async Task<RuntimeResult> CreateEventFromTeplateAsync(
            [LocalizedSummary("txt_mod_event_cmd_createftpl_param_template_sum")]
            EventTemplate template,
            [LocalizedSummary("txt_mod_event_cmd_createftpl_param_startdate_sum")]
            DateTime startDate)
        {
            var evt = new Event()
            {
                Creator = await DbContext.Users.FindOrCreateAsync(Context.User.Id),
                Subject = template.Subject,
                Start = startDate,
                Duration = template.Duration,
                Guild = await DbContext.Guilds.FindOrCreateAsync(Context.Guild.Id),
                Description = template.Description
            };

            foreach (var grp in template.Groups)
                evt.Groups.Add(new()
                {
                    Name = grp.Name,
                    MaxCapacity = grp.MaxCapacity
                });

            DbContext.Events.Add(evt);
            await DbContext.SaveChangesAsync();

            return await ResponseMessageResult.FromMessageAsync(new EventCreatedMessage(evt, Client, DbContext));
        }

        [Command("delete")]
        [Alias("remove", "rm", "del")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_delete_sum")]
        public async Task<RuntimeResult> DeleteEventAsync(
            [LocalizedSummary("txt_mod_event_cmd_delete_param_eventid_sum")]
            ulong eventId
            )
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
        public async Task<RuntimeResult> JoinEventAsync(
            [LocalizedSummary("txt_mod_event_cmd_join_param_eventid_sum")]
            ulong eventId,
            [LocalizedSummary("txt_mod_event_cmd_join_param_user_sum")]
            IGuildUser user = null)
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
        public async Task<RuntimeResult> JoinEventGroupAsync(
            [LocalizedSummary("txt_mod_event_cmd_joingroup_param_eventid_sum")]
            ulong eventId,
            [LocalizedSummary("txt_mod_event_cmd_joingroup_param_groupname_sum")]
            string groupName,
            [LocalizedSummary("txt_mod_event_cmd_joingroup_param_user_sum")]
            IGuildUser user = null)
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
        public async Task<RuntimeResult> LeaveEventAsync(
            [LocalizedSummary("txt_mod_event_cmd_leave_param_eventid_sum")]
            ulong eventId,
            [LocalizedSummary("txt_mod_event_cmd_leave_param_user_sum")]
            IGuildUser user = null)
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
        public async Task<RuntimeResult> LeaveEventGroupAsync(
            [LocalizedSummary("txt_mod_event_cmd_leavegroup_param_eventid_sum")]
            ulong eventId,
            [LocalizedSummary("txt_mod_event_cmd_leavegroup_param_groupname_sum")]
            string groupName,
            [LocalizedSummary("txt_mod_event_cmd_leavegroup_param_user_sum")]
            IGuildUser user = null)
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
        public async Task<RuntimeResult> ShowEventAsync(
            [LocalizedSummary("txt_mod_event_cmd_show_param_eventid_sum")]
            ulong eventId)
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
        public async Task<RuntimeResult> ShowUpcommingEventsAsync(
            [LocalizedSummary("txt_mod_event_cmd_shownext_param_count_sum")]
            int count = 10)
        {
            IEnumerable<Event> upcommingEvents =
                from evt in DbContext.Events.AsQueryable()
                where evt.Guild.GuildId == Context.Guild.Id
                   && evt.Start > DateTime.Now
                orderby evt.Start descending
                select evt;

            upcommingEvents = upcommingEvents.Take(count);
            return await ResponseMessageResult.FromMessageAsync(new EventListMessage(upcommingEvents, Client));
        }

        [Command("update")]
        [Alias("mod", "ch")]
        [RequireContext(ContextType.Guild)]
        [LocalizedSummary("txt_mod_event_cmd_update_sum")]
        public async Task<RuntimeResult> UpdateEventAsync(
            [LocalizedSummary("txt_mod_event_cmd_update_param_eventid_sum")]
            ulong eventId,
            [LocalizedSummary("txt_mod_event_cmd_update_param_subject_sum")]
            string subject = null,
            [LocalizedSummary("txt_mod_event_cmd_update_param_startdate_sum")]
            DateTime? startDate = null,
            [LocalizedSummary("txt_mod_event_cmd_update_param_duration_sum")]
            TimeSpan? duration = null,
            [Remainder]
            [LocalizedSummary("txt_mod_event_cmd_update_param_description_sum")]
            string description = null)
        {
            Event evt = await DbContext.Events.FindAsync(eventId);

            if (!string.IsNullOrWhiteSpace(subject))
                evt.Subject = subject;
            if (!string.IsNullOrWhiteSpace(description))
                evt.Description = description;
            if (startDate.HasValue)
                evt.Start = startDate.Value;
            if (duration.HasValue)
                evt.Duration = duration.Value;

            await DbContext.SaveChangesAsync();

            var eventAnnouncement = await ResponseMessageResult.FromMessageAsync(new EventUpdatedMessage(evt, Client, DbContext)) as ResponseMessageResult;
            if (evt.Guild.AnnouncementChannel != null)
            {
                var channel = Context.Guild.GetTextChannel(evt.Guild.AnnouncementChannel.ChannelId);
                await eventAnnouncement.SendAsync(channel, TokenSource.Token);
            }

            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        #endregion Public Methods

        #region Public Classes

        [Group("template")]
        [Alias("tpl")]
        public class TemplateModule : ModuleBase<SocketCommandContext>
        {
            #region Public Properties

            public DiscordSocketClient Client { get; set; }

            public EventBotContext DbContext { get; set; }

            #endregion Public Properties

            #region Public Methods

            [Command("add-group")]
            [Alias("agrp")]
            [RequireContext(ContextType.Guild)]
            [LocalizedSummary("txt_mod_eventtpl_cmd_add-group_sum")]
            public async Task<RuntimeResult> AddGroupToEventsAsync(
                [LocalizedSummary("txt_mod_eventtpl_cmd_addgroup_param_template_sum")]
                EventTemplate template,
                [LocalizedSummary("txt_mod_eventtpl_cmd_addgroup_param_groupname_sum")]
                string groupName,
                [LocalizedSummary("txt_mod_eventtpl_cmd_addgroup_param_capacity_sum")]
                int? capacity = null)
            {
                if (template.Groups.Count >= MAX_GROUPS)
                    return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_groupsfull);

                template.Groups.Add(new AttendeeGroupTemplate()
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
            [LocalizedSummary("txt_mod_eventtpl_cmd_create_sum")]
            public async Task<RuntimeResult> CreateEventTemplateAsync(
                [LocalizedSummary("txt_mod_eventtpl_cmd_create_param_name_sum")]
                string name,
                [LocalizedSummary("txt_mod_eventtpl_cmd_create_param_subject_sum")]
                string subject,
                [LocalizedSummary("txt_mod_eventtpl_cmd_create_param_duration_sum")]
                TimeSpan duration,
                [Remainder]
                [LocalizedSummary("txt_mod_eventtpl_cmd_create_param_description_sum")]
                string description = null)
            {
                if (DbContext.EventTemplates.AsQueryable().Select(t => t.Name).Contains(name))
                    return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.Unsuccessful, Resources.Resources.txt_msg_templateexists);

                var evt = new EventTemplate()
                {
                    Name = name,
                    Subject = subject,
                    Duration = duration,
                    Guild = await DbContext.Guilds.FindOrCreateAsync(Context.Guild.Id),
                    Description = description
                };

                DbContext.EventTemplates.Add(evt);
                await DbContext.SaveChangesAsync();

                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
            }

            [Command("delete")]
            [Alias("remove", "rm", "del")]
            [RequireContext(ContextType.Guild)]
            [LocalizedSummary("txt_mod_eventtpl_cmd_delete_sum")]
            [LocalizedRemarks("txt_mod_eventtpl_cmd_delete_rem")]
            public async Task<RuntimeResult> DeleteEventTemplateAsync(
                [LocalizedSummary("txt_mod_eventtpl_cmd_delete_param_template_sum")]
                EventTemplate template)
            {
                DbContext.EventTemplates.Remove(template);
                await DbContext.SaveChangesAsync();

                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
            }

            [Command("delete-group")]
            [Alias("remove-group", "rmgrp", "delgrp")]
            [RequireContext(ContextType.Guild)]
            [LocalizedSummary("txt_mod_eventtpl_cmd_deletegrp_sum")]
            [LocalizedRemarks("txt_mod_eventtpl_cmd_deletegrp_rem")]
            public async Task<RuntimeResult> DeleteEventTemplateGroupAsync(
                [LocalizedSummary("txt_mod_eventtpl_cmd_deletegrp_param_template_sum")]
                EventTemplate template,
                [LocalizedSummary("txt_mod_eventtpl_cmd_deletegrp_param_groupindex_sum")]
                uint groupIndex)
            {
                template.Groups.Remove(template.Groups.ToArray()[groupIndex - 1]);
                await DbContext.SaveChangesAsync();

                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
            }

            [Command("list")]
            [Alias("ls")]
            [RequireContext(ContextType.Guild)]
            [LocalizedSummary("txt_mod_eventtpl_cmd_list_sum")]
            public async Task<RuntimeResult> ListTemplatesAsync()
            {
                return await ResponseMessageResult.FromMessageAsync(new EventTemplateListMessage(
                    from tpl in DbContext.EventTemplates.AsQueryable()
                    where tpl.Guild.GuildId == Context.Guild.Id
                    select tpl
                    ));
            }

            [Command("update")]
            [Alias("upd")]
            [RequireContext(ContextType.Guild)]
            [LocalizedSummary("txt_mod_eventtpl_cmd_update_sum")]
            public async Task<RuntimeResult> UpdateEventTemplateAsync(
                [LocalizedSummary("txt_mod_eventtpl_cmd_update_param_template_sum")]
                EventTemplate template,
                [LocalizedSummary("txt_mod_eventtpl_cmd_update_param_subject_sum")]
                string subject = null,
                [LocalizedSummary("txt_mod_eventtpl_cmd_update_param_duration_sum")]
                TimeSpan? duration = null,
                [Remainder]
                [LocalizedSummary("txt_mod_eventtpl_cmd_update_param_description_sum")]
                string description = null)
            {
                if (!String.IsNullOrWhiteSpace(subject))
                    template.Subject = subject;
                if (duration.HasValue)
                    template.Duration = duration.Value;
                if (!string.IsNullOrWhiteSpace(description))
                    template.Description = description;

                await DbContext.SaveChangesAsync();

                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
            }

            #endregion Public Methods
        }

        #endregion Public Classes
    }
}