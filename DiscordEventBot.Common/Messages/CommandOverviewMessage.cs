using Discord;
using Discord.Commands;
using DiscordEventBot.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Messages
{
    public class CommandOverviewMessage : MessageBase
    {
        CommandService _service;
        private SocketCommandContext _context;

        public CommandOverviewMessage(CommandService service, SocketCommandContext context)
        {
            _service = service;
            _context = context;
            HasEmbed = true;
        }
        protected override EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder) => embedBuilder
            .WithColor(Color.DarkBlue)
            .WithTitle("Bot command reference")
            .WithDescription("Here is a list of commands that the bot understands. You may want more details on specific commands. Use `help *command*`")
            .WithFields(
                from module in _service.Modules
                from cmd in module.Commands
                where cmd.CheckPreconditionsAsync(_context).GetAwaiter().GetResult().IsSuccess
                group cmd by module into cmdByModule
                select new EmbedFieldBuilder()
                    .WithName(cmdByModule.Key.Name)
                    .WithValue(string.Join('\n', cmdByModule.Select(c => "> " + c.GetSignature()))))
            ;
    }
}
