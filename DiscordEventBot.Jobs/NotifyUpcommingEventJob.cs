using Discord.WebSocket;
using DiscordEventBot.Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Jobs
{
    public class NotifyUpcommingEventJob : IJob
    {
        private readonly EventBotContext _dbContext;
        private readonly DiscordSocketClient _client;

        public NotifyUpcommingEventJob(EventBotContext dbContext, DiscordSocketClient client)
        {
            _dbContext = dbContext;
            _client = client;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Hello World from Job.");
            foreach (var usr in _dbContext.Users)
                Console.WriteLine(_client.GetUser(usr.UserId).Username + "#" + _client.GetUser(usr.UserId).Discriminator);
        }
    }
}
