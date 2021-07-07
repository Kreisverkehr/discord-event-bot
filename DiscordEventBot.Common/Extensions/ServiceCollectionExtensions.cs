using Discord.Commands;
using DiscordEventBot.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBotServices(this IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<PictureService>()
            .AddSingleton<HttpClient>()
            ;
    }
}
