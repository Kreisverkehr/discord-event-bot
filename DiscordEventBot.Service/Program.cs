using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Common.Services;
using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Service
{
    internal class Program
    {
        #region Private Fields

        private CancellationTokenSource tokenSource = new();

        #endregion Private Fields

        #region Public Methods

        public static void Main(string[] args)
                    => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Console.CancelKeyPress += ShutdownRequested;
            Settings settings = Settings.Load();

            if (!Convert.ToBoolean(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")) && string.IsNullOrWhiteSpace(settings.Token))
            {
                Console.WriteLine("Hello there! It seems that you are running me for the first time.");
                Console.WriteLine("Just let me ask a few questions:");
                Console.Write("What's your Bot's token? ");
                settings.Token = Console.ReadLine();
                Console.WriteLine($"What language do you want me to speak? [ENTER] for {settings.Language}");
                var languageTmp = Console.ReadLine();
                settings.Language = string.IsNullOrWhiteSpace(languageTmp) ? settings.Language : languageTmp;
            }

            CultureInfo.CurrentUICulture = settings.Culture;
            CultureInfo.CurrentCulture = settings.Culture;
            CultureInfo.DefaultThreadCurrentCulture = settings.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = settings.Culture;

            var services = ConfigureServices(settings);

            ConsoleLogger.Filter = (logLevel, source) => logLevel != LogLevel.None && logLevel >= settings.LogLevel;

            using (var ctx = services.GetRequiredService<IDbContextFactory<EventBotContext>>().CreateDbContext())
                ctx.Database.Migrate();

            // setting discord.net's loglevel to verbose. filtering is done in ConsoleLogger
            var discordSettings = services.GetRequiredService<DiscordSocketConfig>();
            discordSettings.LogLevel = LogSeverity.Verbose;
            discordSettings.MessageCacheSize = 1000;

            var _client = services.GetRequiredService<DiscordSocketClient>();
            _client.Log += ConsoleLogger.LogAsync;

            services.GetRequiredService<CommandService>().Log += ConsoleLogger.LogAsync;

            // Login and connect.
            await _client.LoginAsync(TokenType.Bot, settings.Token);
            await _client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            // Wait until process is asked to terminate so the bot actually stays connected.
            try { await Task.Delay(Timeout.Infinite, tokenSource.Token); }
            catch (TaskCanceledException) { }

            await _client.StopAsync();
            await services.DisposeAsync();
            ConsoleLogger.Log(LogLevel.Information, "Shutdown complete.");
        }

        #endregion Public Methods

        #region Private Methods

        private ServiceProvider ConfigureServices(Settings settings) => new ServiceCollection()
            // add basic stuff
            .AddSingleton<DiscordSocketConfig>()
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<ISettings>(settings)
            .AddSingleton<CommandService>()
            .AddSingleton(tokenSource)

            // configure EventBot's services
            .AddEventBotServices()

            //configure DB
            .AddEntityFrameworkProxies()
            .AddEntityFrameworkSqlite()
            .AddLogging()
            .AddDbContextFactory<EventBotContext>(options => options
                .UseSqlite($"Data Source = {settings.SQLiteFile}")
                .UseLazyLoadingProxies()
                .LogTo((eventId, logLevel) => true, ConsoleLogger.Log)
            )

            // build provider
            .BuildServiceProvider(validateScopes: false)
            ;

        private void ShutdownRequested(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            ConsoleLogger.Log(LogLevel.Information, "Shutdown requested. Please wait.");
            tokenSource.Cancel();
        }

        #endregion Private Methods
    }
}