using Discord.WebSocket;
using DiscordEventBot.Model;
using Quartz;
using System;
using System.Threading.Tasks;

namespace DiscordEventBot.Jobs
{
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
            Console.WriteLine("Hello World from Job.");
            foreach (var usr in _dbContext.Users)
                Console.WriteLine(_client.GetUser(usr.UserId).Username + "#" + _client.GetUser(usr.UserId).Discriminator);
        }

        #endregion Public Methods
    }
}