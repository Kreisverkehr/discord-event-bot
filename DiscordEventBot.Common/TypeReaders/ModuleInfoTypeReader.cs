using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.TypeReaders
{
    public class ModuleInfoTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var commandService = services.GetRequiredService<CommandService>();
            var modules =
                from mod in commandService.Modules
                from alias in mod.Aliases
                where alias.ToUpperInvariant() == input.ToUpperInvariant()
                select mod;

            var module = modules.FirstOrDefault();

            return module != default(ModuleInfo) ? 
                Task.FromResult(TypeReaderResult.FromSuccess(module)) : 
                Task.FromResult(TypeReaderResult.FromError(CommandError.ObjectNotFound, $"Module \"{input}\" cannot be found."));
        }
    }
}
