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
        [Remarks("This command only displays commands that you are able to use.")]
        public Task<RuntimeResult> HelpAsync(CommandInfo command = null)
        {
            MessageBase response;

            if(command != null)
            {
                response = new CommandHelpMessage(command);
            } else
            {
                response = new CommandOverviewMessage(_service, Context);
            }

            return ResponseMessageResult.FromMessageAsync(response);
        }
    }
}
