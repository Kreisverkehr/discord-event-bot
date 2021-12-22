using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordEventBot.Jobs
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class NotifyOwnerOnNewRelease : IJob
    {
        #region Private Fields

        private readonly Version _currentVersion = new("0.3.0");
        private readonly DiscordSocketClient _discordClient;

        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new("https://api.github.com/")
        };

        #endregion Private Fields

        #region Public Constructors

        public NotifyOwnerOnNewRelease(DiscordSocketClient discordClient)
        {
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new("discord-event-bot", _currentVersion.ToString()));
            _httpClient.DefaultRequestHeaders.Accept.Add(new("application/vnd.github.v3+json"));
            _discordClient = discordClient;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task Execute(IJobExecutionContext context)
        {
            var resp = await _httpClient.GetStringAsync("repos/Kreisverkehr/discord-event-bot/releases/latest");
            var respObj = JsonConvert.DeserializeObject(resp) as JObject;
            Version latestVersion = respObj["tag_name"].ToObject<Version>();

            if (!context.JobDetail.JobDataMap.GetBooleanValue(latestVersion.ToString()) && latestVersion > _currentVersion)
            {
                var appInfo = await _discordClient.GetApplicationInfoAsync();
                var ownerDmChannel = await appInfo.Owner.CreateDMChannelAsync();
                var latestVersionInfo = new EmbedBuilder()
                    .WithTitle(respObj["name"].ToObject<string>())
                    .WithTimestamp(respObj["published_at"].ToObject<DateTime>())
                    .WithDescription(respObj["body"].ToObject<string>())
                    //.WithAuthor(_discordClient.GetUser("Kreisverkehr", "5046"))
                    .Build();
                await ownerDmChannel.SendMessageAsync(text: "Hey!\nThere is a new version of me. Check it out. \nhttps://github.com/Kreisverkehr/discord-event-bot/releases/latest", embed: latestVersionInfo);
                context.JobDetail.JobDataMap.Put(latestVersion.ToString(), true);
            }
        }

        #endregion Public Methods
    }
}