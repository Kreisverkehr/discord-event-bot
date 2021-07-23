using Discord.Commands;
using DiscordEventBot.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordEventBot.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        #region Public Methods

        public static IServiceCollection AddEventBotServices(this IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<ResultReasonService>()
            ;

        #endregion Public Methods
    }
}