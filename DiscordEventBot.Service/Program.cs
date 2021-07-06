using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common;
using DiscordEventBot.Model;
using DiscordEventBot.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
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
            Settings settings = new();

            if (!await settings.LoadFromFile())
            {
                Console.WriteLine("Oops! It seems that there is no previous settings file.");
                Console.WriteLine("Fear not! I will create one quickly.");
                Console.WriteLine("Just let me ask a few questions:");
                Console.Write("Enter your Bot's token: ");
                settings.Token = Console.ReadLine();
                Console.WriteLine($"Where do you want your data to be stored? [ENTER] for {settings.DataStore}");
                var dataStoreTmp = Console.ReadLine();
                settings.DataStore = string.IsNullOrWhiteSpace(dataStoreTmp) ? settings.DataStore : dataStoreTmp;
                File.WriteAllBytes(Settings.SETTINGS_FILE, JsonSerializer.SerializeToUtf8Bytes(settings, new JsonSerializerOptions() { WriteIndented = true }));
            }

            var services = ConfigureServices(settings);

            ConsoleLogger.Filter = (logLevel, source) => logLevel != LogLevel.None && logLevel >= settings.LogLevel;

            using (var ctx = services.GetRequiredService<IDbContextFactory<EventBotContext>>().CreateDbContext())
                ctx.Database.Migrate();

            // setting discord.net's loglevel to verbose. filtering is done in ConsoleLogger
            var discordSettings = services.GetRequiredService<DiscordSocketConfig>();
            discordSettings.LogLevel = LogSeverity.Verbose;

            var _client = services.GetRequiredService<DiscordSocketClient>();
            _client.Log += ConsoleLogger.LogAsync;

            services.GetRequiredService<CommandService>().Log += ConsoleLogger.LogAsync;

            // Login and connect.
            await _client.LoginAsync(TokenType.Bot, settings.Token);
            await _client.StartAsync();

            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            // Wait infinitely so your bot actually stays connected.
            try { await Task.Delay(Timeout.Infinite, tokenSource.Token); }
            catch (TaskCanceledException) { }

            await _client.StopAsync();
            await services.DisposeAsync();
            ConsoleLogger.Log(LogLevel.Information, "Shutdown complete.");
        }

        #endregion Public Methods

        #region Private Methods

        private static ServiceProvider ConfigureServices(Settings settings) => new ServiceCollection()
            .AddSingleton<DiscordSocketConfig>()
            .AddSingleton<Settings>()
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<PictureService>()
            .AddSingleton<HttpClient>()

            //configure DB
            .AddEntityFrameworkProxies()
            .AddEntityFrameworkSqlite()
            .AddLogging()
            .AddDbContextFactory<EventBotContext>(options => options
                .UseSqlite($"Data Source = {settings.DataStore}")
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