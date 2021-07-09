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
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    [Group("event")]
    [Alias("evt")]
    [LocalizedName("txt_mod_event_name")]
    public class EventModule : ModuleBase<SocketCommandContext>
    {
        public IDbContextFactory<EventBotContext> DbContextFactory { get; set; }
        public DiscordSocketClient Client { get; set; }

        [Command("create")]
        [Alias("cr", "new")]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> CreateEventAsync(string subject, DateTimeOffset startDate, TimeSpan duration, [Remainder] string description = null)
        {
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
        public async Task<RuntimeResult> JoinEventAsync(ulong eventId)
        {
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                Event evt = await dbContext.Events.FindAsync(eventId);
                User user = await dbContext.Users.FindOrCreateAsync(Context.User.Id);

                if (evt == null || evt.Guild.GuildId != Context.Guild.Id) 
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_eventnotfound);

                if (evt.Attendees.Contains(user))
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_alreadyoined);

                evt.Attendees.Add(user);

                dbContext.SaveChanges();
            }

            return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_ok);
        }

        [Command("show")]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> ShowEventAsync(ulong eventId)
        {
            using (var dbContext = DbContextFactory.CreateDbContext())
            {
                Event evt = await dbContext.Events.FindAsync(eventId);

                if (evt == null || evt.Guild.GuildId != Context.Guild.Id)
                    return await ResponseMessageResult.FromMessageAsync(Resources.Resources.txt_msg_eventnotfound);

                return await ResponseMessageResult.FromMessageAsync(new EventInfoMessage(evt, Client));
            }
        }
    }
}
