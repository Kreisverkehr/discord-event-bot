using Discord.Commands;
using Discord.Interactions;
using DiscordEventBot.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordEventBot.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        #region Public Methods

        public static IServiceCollection AddEventBotServices(this IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton(new CommandServiceConfig()
            {
                LogLevel = Discord.LogSeverity.Verbose
            })
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<ResultReasonService>()
            .AddSingleton(new InteractionServiceConfig()
            {
                LogLevel = Discord.LogSeverity.Verbose,
            })
            .AddSingleton<InteractionService>()
            .AddSingleton<InteractionHandlingService>()
            ;

        #endregion Public Methods
    }
}