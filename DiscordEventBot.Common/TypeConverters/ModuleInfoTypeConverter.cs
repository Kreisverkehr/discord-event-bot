using Discord;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.TypeConverters
{
    public class ModuleInfoTypeConverter : TypeConverter
    {
        #region Public Methods

        public override bool CanConvertTo(Type type) => type == typeof(ModuleInfo);

        public override ApplicationCommandOptionType GetDiscordType() => ApplicationCommandOptionType.String;

        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
        {
            var commandService = services.GetRequiredService<InteractionService>();
            var input = option.Value.ToString();
            var modules =
                from mod in commandService.Modules
                where mod.Name.ToUpperInvariant() == input.ToUpperInvariant()
                select mod;

            var module = modules.FirstOrDefault();

            return module != default(ModuleInfo) ?
                Task.FromResult(TypeConverterResult.FromSuccess(module)) :
                Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"Module \"{input}\" cannot be found."));
        }

        #endregion Public Methods
    }
}