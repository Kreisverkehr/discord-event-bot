using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.TypeReaders
{
    public class CommandInfoTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var commandService = services.GetRequiredService<CommandService>();
            var commands =
                from module in commandService.Modules
                from cmd in module.Commands
                from alias in cmd.Aliases
                where alias.ToUpperInvariant() == input.ToUpperInvariant()
                select cmd;

            var command = commands.FirstOrDefault();

            return command != default(CommandInfo) ? 
                Task.FromResult(TypeReaderResult.FromSuccess(command)) : 
                Task.FromResult(TypeReaderResult.FromError(CommandError.ObjectNotFound, $"Command \"{input}\" cannot be found."));
        }
    }
}
