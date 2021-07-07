using Discord;
using Discord.Commands;
using DiscordEventBot.Common.Messages;
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
        [Remarks("This command only displays commands that you are able to use.")]
        public async Task HelpAsync(CommandInfo command = null)
        {
            if(command != null)
            {
                await ReplyAsync(embed: new CommandHelpMessage(command));
                return;
            }

            await ReplyAsync(embed: new CommandOverviewMessage(_service, Context));
        }
    }
}
