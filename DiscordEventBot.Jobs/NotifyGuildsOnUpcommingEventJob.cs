using Discord.WebSocket;
using DiscordEventBot.Common.Messages;
using DiscordEventBot.Common.RuntimeResults;
using DiscordEventBot.Model;
using Quartz;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Jobs
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class NotifyGuildsOnUpcommingEventJob : IJob
    {
        #region Private Fields

        private readonly DiscordSocketClient _client;
        private readonly EventBotContext _dbContext;
        private readonly CancellationTokenSource _tokenSource;

        #endregion Private Fields

        #region Public Constructors

        public NotifyGuildsOnUpcommingEventJob(EventBotContext dbContext, DiscordSocketClient client, CancellationTokenSource tokenSource)
        {
            _dbContext = dbContext;
            _client = client;
            _tokenSource = tokenSource;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task Execute(IJobExecutionContext context)
        {
            var upcommingEvents =
                from guild in _dbContext.Guilds.AsQueryable()
                where guild.NotificationTime > 0
                from evt in _dbContext.Events.AsQueryable()
                where evt.Start < DateTime.Now.AddMinutes(guild.NotificationTime)
                   && evt.Start > DateTime.Now
                select new { evt, guild };

            if (upcommingEvents.Any())
            {
                foreach (var notifyData in upcommingEvents)
                {
                    var channel = _client.GetGuild(notifyData.guild.GuildId).GetTextChannel(notifyData.guild.AnnouncementChannel.ChannelId);

                    var msg = ResponseMessageResult.FromMessage(new EventUpcommingMessage(notifyData.evt, _client, _dbContext)) as ResponseMessageResult;
                    await msg.SendAsync(channel, _tokenSource.Token);
                }
            }
        }

        #endregion Public Methods
    }
}