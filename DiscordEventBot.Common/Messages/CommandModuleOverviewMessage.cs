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
    public class CommandModuleOverviewMessage : MessageBase
    {
        ModuleInfo _module;
        private SocketCommandContext _context;

        public CommandModuleOverviewMessage(ModuleInfo module, SocketCommandContext context)
        {
            _module = module;
            _context = context;
            HasEmbed = true;
        }
        protected override EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder) => embedBuilder
            .WithColor(Color.DarkBlue)
            .WithTitle(_module.Name)
            .WithDescription(_module.Summary)
            .WithFields(
                from cmd in _module.Commands
                where cmd.CheckPreconditionsAsync(_context).GetAwaiter().GetResult().IsSuccess
                select new EmbedFieldBuilder()
                    .WithName(cmd.GetSignature())
                    .WithValue(string.IsNullOrWhiteSpace(cmd.Summary) ? "No summary :(" : cmd.Summary))
            ;
    }
}
