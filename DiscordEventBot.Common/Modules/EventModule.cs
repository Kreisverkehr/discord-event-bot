using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Common.Messages;
using DiscordEventBot.Common.RuntimeResults;
using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    [Group("event")]
    [Alias("evt")]
    [LocalizedName("txt_mod_event_name")]
    public class EventModule : ModuleBase<SocketCommandContext>
    {
        #region Public Properties

        public DiscordSocketClient Client { get; set; }

        public IDbContextFactory<EventBotContext> DbContextFactory { get; set; }

        #endregion Public Properties

        #region Public Methods

        [Command("create")]
        [Alias("cr", "new")]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> CreateEventAsync(string subject, DateTime startDate, TimeSpan duration, [Remainder] string description = null)
        {
            using (Context.Channel.EnterTypingState())
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                var evt = new Event()
                {
                    Creator = await dbContext.Users.FindOrCreateAsync(Context.User.Id),
                    Subject = subject,
                    Start = startDate,
                    Duration = duration,
                    Guild = await dbContext.Guilds.FindOrCreateAsync(Context.Guild.Id),
                    Description = description
                };

                dbContext.Events.Add(evt);
                await dbContext.SaveChangesAsync();

                return await ResponseMessageResult.FromMessageAsync(new EventCreatedMessage(evt, Client));
            }
        }

        [Command("join")]
        [Alias("attend", "jn")]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> JoinEventAsync(ulong eventId, IGuildUser user = null)
        {
            IGuildUser discUser = user ?? Context.User as IGuildUser;
            using (Context.Channel.EnterTypingState())
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                Event evt = await dbContext.Events.FindAsync(eventId);
                User dbUser = await dbContext.Users.FindOrCreateAsync(discUser.Id);

                if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_eventnotfound);

                if (evt.Attendees.Contains(dbUser))
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_alreadyoined);

                evt.Attendees.Add(dbUser);

                await dbContext.SaveChangesAsync();
            }

            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("leave")]
        [Alias("unattend", "lv")]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> LeaveEventAsync(ulong eventId, IGuildUser user = null)
        {
            IGuildUser discUser = user ?? Context.User as IGuildUser;
            using (Context.Channel.EnterTypingState())
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                Event evt = await dbContext.Events.FindAsync(eventId);
                User dbUser = await dbContext.Users.FindOrCreateAsync(discUser.Id);

                if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_eventnotfound);

                if (!evt.Attendees.Contains(dbUser))
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_notjoined);

                evt.Attendees.Remove(dbUser);

                await dbContext.SaveChangesAsync();
            }

            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("join-group")]
        [Alias("attend-group", "jngrp")]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> JoinEventGroupAsync(ulong eventId, string groupName, IGuildUser user = null)
        {
            IGuildUser discUser = user ?? Context.User as IGuildUser;
            using (Context.Channel.EnterTypingState())
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                Event evt = await dbContext.Events.FindAsync(eventId);
                User dbUser = await dbContext.Users.FindOrCreateAsync(discUser.Id);

                if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_eventnotfound);

                AttendeeGroup grp = evt.Groups.Where(grp => grp.Name.ToUpperInvariant() == groupName.ToUpperInvariant()).FirstOrDefault();

                if (grp == default(AttendeeGroup))
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_eventgroupnotfound);

                if (grp.Attendees.Contains(dbUser))
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_alreadyoined);

                grp.Attendees.Add(dbUser);

                await dbContext.SaveChangesAsync();
            }

            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("leave-group")]
        [Alias("unattend-group", "lvgrp")]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> LeaveEventGroupAsync(ulong eventId, string groupName, IGuildUser user = null)
        {
            IGuildUser discUser = user ?? Context.User as IGuildUser;
            using (Context.Channel.EnterTypingState())
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                Event evt = await dbContext.Events.FindAsync(eventId);
                User dbUser = await dbContext.Users.FindOrCreateAsync(discUser.Id);

                if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_eventnotfound);

                AttendeeGroup grp = evt.Groups.Where(grp => grp.Name.ToUpperInvariant() == groupName.ToUpperInvariant()).FirstOrDefault();

                if (grp == default(AttendeeGroup))
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_eventgroupnotfound);

                if (!grp.Attendees.Contains(dbUser))
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_notjoined);

                grp.Attendees.Remove(dbUser);

                await dbContext.SaveChangesAsync();
            }

            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("show")]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> ShowEventAsync(ulong eventId)
        {
            using (Context.Channel.EnterTypingState())
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                Event evt = await dbContext.Events.FindAsync(eventId);

                if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_eventnotfound);

                return await ResponseMessageResult.FromMessageAsync(new EventInfoMessage(evt, Client));
            }
        }

        [Command("delete")]
        [Alias("remove", "rm", "del")]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> DeleteEventAsync(ulong eventId)
        {
            using (Context.Channel.EnterTypingState())
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                Event evt = await dbContext.Events.FindAsync(eventId);

                if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_eventnotfound);

                dbContext.Events.Remove(evt);
                await dbContext.SaveChangesAsync();
            }
            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        [Command("show-next")]
        [Alias("upcomming")]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> ShowUpcommingEventsAsync(int count = 10)
        {
            using (Context.Channel.EnterTypingState())
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                IEnumerable<Event> upcommingEvents =
                    from evt in dbContext.Events.AsQueryable()
                    where evt.Guild.GuildId == Context.Guild.Id
                       && evt.Start > DateTime.Now
                    orderby evt.Start descending
                    select evt;

                upcommingEvents = upcommingEvents.Take(count);
                return await ResponseMessageResult.FromMessageAsync(new EventListMessage(upcommingEvents));
            }
        }

        [Command("add-group")]
        [Alias("agrp")]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> AddGroupToEventsAsync(ulong eventId, string groupName, int? capacity = null)
        {
            using(Context.Channel.EnterTypingState())
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                Event evt = await dbContext.Events.FindAsync(eventId);

                if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_eventnotfound);

                evt.Groups.Add(new AttendeeGroup()
                {
                    Name = groupName,
                    MaxCapacity = capacity
                });

                await dbContext.SaveChangesAsync();
            }
            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        #endregion Public Methods
    }
}