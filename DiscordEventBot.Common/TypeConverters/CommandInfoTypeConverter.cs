using Discord;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.TypeConverters
{
    public class CommandInfoTypeConverter : TypeConverter
    {
        #region Public Methods

        public override bool CanConvertTo(Type type) => type == typeof(ICommandInfo);

        public override ApplicationCommandOptionType GetDiscordType() => ApplicationCommandOptionType.String;

        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
        {
            var commandService = services.GetRequiredService<InteractionService>();
            var input = option.Value.ToString();
            var commands =
                from module in commandService.Modules
                from cmd in module.SlashCommands.OfType<ICommandInfo>().Union(module.ContextCommands.OfType<ICommandInfo>()).Union(module.ComponentCommands.OfType<ICommandInfo>())
                where cmd.Name.ToUpperInvariant() == input.ToUpperInvariant()
                select cmd;

            var command = commands.FirstOrDefault();

            return command != default(ICommandInfo) ?
                Task.FromResult(TypeConverterResult.FromSuccess(command)) :
                Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"Command \"{input}\" cannot be found."));
        }

        #endregion Public Methods
    }
}