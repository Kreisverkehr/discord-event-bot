using Discord;
using Discord.Commands;
using DiscordEventBot.Common.Extensions;
using System.Linq;

namespace DiscordEventBot.Common.Messages
{
    public class CommandModuleOverviewMessage : MessageBase
    {
        #region Private Fields

        private SocketCommandContext _context;
        private ModuleInfo _module;

        #endregion Private Fields

        #region Public Constructors

        public CommandModuleOverviewMessage(ModuleInfo module, SocketCommandContext context)
        {
            _module = module;
            _context = context;
            HasEmbed = true;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder) => embedBuilder
            .WithColor(Color.DarkBlue)
            .WithTitle(_module.Name)
            .WithDescription(_module.Summary)
            .WithFields(
                from cmd in _module.GetCommandsRecursive()
                where cmd.CheckPreconditionsAsync(_context).GetAwaiter().GetResult().IsSuccess
                select new EmbedFieldBuilder()
                    .WithName(cmd.GetSignature())
                    .WithValue(string.IsNullOrWhiteSpace(cmd.Summary) ? Resources.Resources.txt_msg_nosummary : cmd.Summary))
            ;

        #endregion Protected Methods
    }
}