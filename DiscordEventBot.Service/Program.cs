using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Jobs.Extensions;
using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Threading;

namespace DiscordEventBot.Service
{
    internal class Program
    {
        #region Private Fields

        private static Settings _settings;

        #endregion Private Fields

        #region Private Methods

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => services
                    .AddHostedService<DiscordBotService>()
                    // add basic stuff
                    .AddLogging()
                    .AddSingleton<CancellationTokenSource>()
                    .AddSingleton(new DiscordSocketConfig()
                    {
                        AlwaysDownloadUsers = true,
                        LogLevel = Discord.LogSeverity.Verbose,
                        MessageCacheSize = 1000
                    })
                    .AddSingleton<DiscordSocketClient>()
                    .AddSingleton<CommandService>()
                    .AddSingleton<ISettings>(_settings)

                    // configure EventBot's services
                    .AddEventBotServices()
                    .AddEventBotJobs()

                    //configure DB
                    .AddDbContext<EventBotContext>(options => options
                        .UseSqlite($"Data Source = {_settings.SQLiteFile}")
                        .UseLazyLoadingProxies()
                    )
                    .AddEntityFrameworkProxies()
                );

        private static void Main(string[] args)
        {
            _settings = Settings.Load();

            if (!Convert.ToBoolean(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")) && string.IsNullOrWhiteSpace(_settings.Token))
            {
                Console.WriteLine("Hello there! It seems that you are running me for the first time.");
                Console.WriteLine("Just let me ask a few questions:");
                Console.Write("What's your Bot's token? ");
                _settings.Token = Console.ReadLine();
                Console.WriteLine($"What language do you want me to speak? [ENTER] for {_settings.Language}");
                var languageTmp = Console.ReadLine();
                _settings.Language = string.IsNullOrWhiteSpace(languageTmp) ? _settings.Language : languageTmp;
            }

            CultureInfo.CurrentUICulture = _settings.Culture;
            CultureInfo.CurrentCulture = _settings.Culture;
            CultureInfo.DefaultThreadCurrentCulture = _settings.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = _settings.Culture;

            CreateHostBuilder(args).Build().Run();
        }

        #endregion Private Methods
    }
}