using Discord;
using Discord.Commands;
using DiscordEventBot.Common.Extensions;
using Humanizer;
using System.Collections.Generic;
using System.Linq;

namespace DiscordEventBot.Common.Messages
{
    public class CommandHelpMessage : MessageBase
    {
        #region Private Fields

        private CommandInfo _commandInfo;
        private List<string> _aliases;

        #endregion Private Fields

        #region Public Constructors

        public CommandHelpMessage(CommandInfo commandInfo)
        {
            _commandInfo = commandInfo;
            _aliases = _commandInfo.Aliases.Where(a => a.ToUpperInvariant() != _commandInfo.Name.ToUpperInvariant()).ToList();
            HasEmbed = true;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder) => embedBuilder
            .WithTitle(_commandInfo.GetSignature(simplified:false))
            .WithColor(Color.DarkGreen)
            .WithDescription(_commandInfo.Summary)
            .AddFieldIf(() => !string.IsNullOrEmpty(_commandInfo.Remarks), builder => builder
                .WithName(Resources.Resources.txt__word_remarks)
                .WithValue(_commandInfo.Remarks))
            .AddFieldIf(() => _aliases.Count > 0, builder => builder
                .WithName(Resources.Resources.txt_word_alias.ToQuantity(_aliases.Count, ShowQuantityAs.None))
                .WithValue(string.Join(", ", _aliases) ?? Resources.Resources.txt_word_none))
            ;

        #endregion Protected Methods
    }
}