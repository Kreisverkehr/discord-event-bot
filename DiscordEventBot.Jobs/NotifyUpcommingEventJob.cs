using Discord.WebSocket;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Common.Resources;
using DiscordEventBot.Model;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordEventBot.Jobs
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class NotifyUpcommingEventJob : IJob
    {
        #region Private Fields

        private readonly DiscordSocketClient _client;
        private readonly EventBotContext _dbContext;

        #endregion Private Fields

        #region Public Constructors

        public NotifyUpcommingEventJob(EventBotContext dbContext, DiscordSocketClient client)
        {
            _dbContext = dbContext;
            _client = client;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task Execute(IJobExecutionContext context)
        {
            var usersWithNotifyEnabled =
                from user in _dbContext.Users.AsQueryable()
                where user.MinutesBeforeEventNotify > 0
                let discordUser = _client.GetUser(user.UserId)
                select new { user, discordUser };

            foreach (var usr in usersWithNotifyEnabled)
            {
                var upcommingEvents =
                    from evt in _dbContext.Events.AsQueryable()
                    where evt.Start < DateTime.Now.AddMinutes(usr.user.MinutesBeforeEventNotify)
                       && evt.Start > DateTime.Now
                    select evt;
                var eventsToNotify =
                    from evt in upcommingEvents.AsEnumerable()
                    let hasBeenNotified = context.JobDetail.JobDataMap.Get($"{evt.EventID}-{usr.user.UserId}") as bool?
                    where (!hasBeenNotified.HasValue || !hasBeenNotified.Value) && evt.GetAttendingUsers().Contains(usr.user)
                    select evt;
                eventsToNotify = eventsToNotify.ToList();

                if (eventsToNotify.Count() > 0)
                {
                    var channel = await usr.discordUser.GetOrCreateDMChannelAsync();
                    using (channel.EnterTypingState())
                        foreach (var evt in eventsToNotify)
                        {
                            await channel.SendMessageAsync(text: String.Format(Resources.txt_msg_eventnotification, evt.Subject, _client.GetGuild(evt.Guild.GuildId).Name, evt.Start));
                            context.JobDetail.JobDataMap.Put($"{evt.EventID}-{usr.user.UserId}", true);
                        } 
                }
            }
        }

        #endregion Public Methods
    }
}