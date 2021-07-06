using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common;
using DiscordEventBot.Model;
using DiscordEventBot.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Service
{
    class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Console.CancelKeyPress += ShutdownRequested;
            using (var services = ConfigureServices())
            {
                var _settings = services.GetRequiredService<Settings>();

                if (!File.Exists(Settings.SETTINGS_FILE))
                {
                    Console.WriteLine("Oops! It seems that there is no previous settings file.");
                    Console.WriteLine("Fear not! I will create one quickly.");
                    Console.WriteLine("Just let me ask a few questions:");
                    Console.Write("Enter your Bot's token: ");
                    _settings.Token = Console.ReadLine();
                    Console.WriteLine($"Where do you want your data to be stored? [ENTER] for {_settings.DataStore}");
                    var dataStoreTmp = Console.ReadLine();
                    _settings.DataStore = string.IsNullOrWhiteSpace(dataStoreTmp) ? _settings.DataStore : dataStoreTmp;
                    File.WriteAllBytes(Settings.SETTINGS_FILE, JsonSerializer.SerializeToUtf8Bytes(_settings, new JsonSerializerOptions() { WriteIndented = true }));
                }
                else
                {
                    byte[] settingBytes = File.ReadAllBytes(Settings.SETTINGS_FILE);
                    Settings tmp = JsonSerializer.Deserialize<Settings>(settingBytes);
                    _settings.DiscordClientConfig = tmp.DiscordClientConfig;
                    _settings.Token = tmp.Token;
                    _settings.DataStore = tmp.DataStore;
                    _settings.IsLoadedFromFile = true;
                }

                using (EventBotContext DbCtx = new())
                    DbCtx.Database.Migrate();

                var _client = services.GetRequiredService<DiscordSocketClient>();
                _client.Log += LogAsync;

                services.GetRequiredService<CommandService>().Log += LogAsync;

                // Login and connect.
                await _client.LoginAsync(TokenType.Bot, _settings.Token);
                await _client.StartAsync();

                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                // Wait infinitely so your bot actually stays connected.
                try { await Task.Delay(Timeout.Infinite, tokenSource.Token); }
                catch (TaskCanceledException) { }

                await _client.StopAsync();
            }
        }
        private CancellationTokenSource tokenSource = new();
        private void ShutdownRequested(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Console.WriteLine("Shutdown requested. Please wait.");
            tokenSource.Cancel();
        }

        private static ServiceProvider ConfigureServices() => new ServiceCollection()
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<Settings>()
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<PictureService>()
            .AddSingleton<HttpClient>()
            .AddDbContext<EventBotContext>()
            .AddEntityFrameworkProxies()
            .AddEntityFrameworkSqlite()
            .BuildServiceProvider(validateScopes: true)
            ;

        private static Task LogAsync(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
