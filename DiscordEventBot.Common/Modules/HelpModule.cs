using Discord;
using Discord.Commands;
using DiscordEventBot.Common.Messages;
using DiscordEventBot.Common.RuntimeResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;

        public HelpModule(CommandService service)
        {
            _service = service;
        }

        [Command("help")]
        [Summary("Displays every command that you can use")]
        [Remarks("This provides a filtered list of commands. You will only see what you can use.")]
        public Task<RuntimeResult> HelpAsync() =>
            ResponseMessageResult.FromMessageAsync(new CommandOverviewMessage(_service, Context));

        [Command("help")]
        [Summary("Displays a detailed explaination of the given command")]
        public Task<RuntimeResult> HelpAsync([Remainder] CommandInfo command) =>
            ResponseMessageResult.FromMessageAsync(new CommandHelpMessage(command));

        [Command("help")]
        [Summary("Displays a more detailed overview of the given module")]
        [Remarks("This provides a filtered list of commands. You will only see what you can use.")]
        public Task<RuntimeResult> HelpAsync(ModuleInfo module) =>
            ResponseMessageResult.FromMessageAsync(new CommandModuleOverviewMessage(module, Context));
    }
}
