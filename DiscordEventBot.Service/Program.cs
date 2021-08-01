using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Common.Services;
using DiscordEventBot.Jobs.Extensions;
using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Globalization;
using System.Threading;

namespace DiscordEventBot.Service
{
    internal class Program
    {
        #region Private Fields

        private static CancellationTokenSource tokenSource = new();

        #endregion Private Fields

        #region Private Methods

        private static IHostBuilder CreateHostBuilder(string[] args, Settings settings) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => services
                    .AddHostedService<DiscordBotService>()
                    // add basic stuff
                    .AddSingleton<DiscordSocketConfig>()
                    .AddSingleton<DiscordSocketClient>()
                    .AddSingleton<ISettings>(settings)
                    .AddSingleton<CommandService>()

                    // configure EventBot's services
                    .AddEventBotServices()

                    //configure DB
                    .AddDbContext<EventBotContext>(options => options
                        .UseSqlite($"Data Source = {settings.SQLiteFile}")
                        .UseLazyLoadingProxies()
                    )
                    .AddEntityFrameworkProxies()
                    .AddLogging()
                    .AddJobs()
                );

        private static void Main(string[] args)
        {
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

            IHost host = CreateHostBuilder(args, settings).Build();
            host.Services.GetRequiredService<ShutdownService>().Shutdown = host.StopAsync;
            host.Run();
        }
        #endregion Private Methods
    }
}