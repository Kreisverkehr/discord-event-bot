using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Common.Services;
using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Service
{
    internal class DiscordBotService : IHostedService
    {
        #region Private Fields

        private readonly IHostApplicationLifetime _appLifetime;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly EventBotContext _dbContext;
        private readonly CommandHandlingService _handlingService;
        private readonly ILogger _logger;
        private readonly Settings _settings;
        private readonly CancellationTokenSource _tokenSource;

        #endregion Private Fields

        #region Public Constructors

        public DiscordBotService(
            ILogger<DiscordBotService> logger,
            IHostApplicationLifetime appLifetime,
            ISettings settings,
            DiscordSocketClient client,
            CommandService commandService,
            CommandHandlingService handlingService,
            EventBotContext dbContext,
            CancellationTokenSource tokenSource)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _settings = settings as Settings;
            _dbContext = dbContext;
            _commandService = commandService;
            _handlingService = handlingService;
            _client = client;
            _tokenSource = tokenSource;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _dbContext.Database.Migrate();
            _client.Log += Log;
            _commandService.Log += Log;

            // Login and connect.
            await _client.LoginAsync(TokenType.Bot, _settings.Token);
            await _client.StartAsync();

            await _handlingService.InitializeAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSource.Cancel();
            await _client.StopAsync();
            _logger.LogInformation("Shutdown complete");
        }

        #endregion Public Methods

        #region Private Methods

        private Task Log(LogMessage msg)
        {
            _logger.Log(msg.Severity.ToLogLevel(), $"{msg.Source}: {msg.Message}");
            return Task.CompletedTask;
        }

        #endregion Private Methods
    }
}