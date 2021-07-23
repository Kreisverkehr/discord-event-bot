using Discord;
using Discord.Commands;
using DiscordEventBot.Common.Extensions;
using System.Linq;

namespace DiscordEventBot.Common.Messages
{
    public class CommandOverviewMessage : MessageBase
    {
        #region Private Fields

        private SocketCommandContext _context;
        private CommandService _service;

        #endregion Private Fields

        #region Public Constructors

        public CommandOverviewMessage(CommandService service, SocketCommandContext context)
        {
            _service = service;
            _context = context;
            HasEmbed = true;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder) => embedBuilder
            .WithColor(Color.DarkBlue)
            .WithTitle(Resources.Resources.txt_msg_cmdoverview_title)
            .WithDescription(Resources.Resources.txt_msg_cmdoverview_desc)
            .WithFields(
                from module in _service.Modules
                where module.Parent == null
                from cmd in module.GetCommandsRecursive()
                where cmd.CheckPreconditionsAsync(_context).GetAwaiter().GetResult().IsSuccess
                group cmd by module into cmdByModule
                select new EmbedFieldBuilder()
                    .WithName(cmdByModule.Key.Name)
                    .WithValue(string.Join('\n', cmdByModule.Select(c => "> " + c.GetSignature()))))
            ;

        #endregion Protected Methods
    }
}