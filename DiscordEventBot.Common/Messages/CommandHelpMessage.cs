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
        private List<string> aliases;

        #endregion Private Fields

        #region Public Constructors

        public CommandHelpMessage(CommandInfo commandInfo)
        {
            _commandInfo = commandInfo;
            aliases = _commandInfo.Aliases.Where(a => a.ToUpperInvariant() != _commandInfo.Name.ToUpperInvariant()).ToList();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder) => embedBuilder
            .WithTitle(_commandInfo.GetSignature())
            .WithColor(Color.DarkGreen)
            .WithDescription(_commandInfo.Summary)
            .AddFieldIf(() => !string.IsNullOrEmpty(_commandInfo.Remarks), builder => builder
                .WithName("Remarks")
                .WithValue(_commandInfo.Remarks))
            .AddFieldIf(() => aliases.Count > 0, builder => builder
                .WithName("Alias".ToQuantity(aliases.Count, ShowQuantityAs.None))
                .WithValue(string.Join(", ", aliases) ?? "None"))
            ;

        protected override string BuildMessageText()
        {
            var msgText = base.BuildMessageText();

            msgText += _commandInfo.GetSignature();

            if (!string.IsNullOrWhiteSpace(_commandInfo.Summary)) msgText += $"\n{_commandInfo.Summary}";
            if (!string.IsNullOrWhiteSpace(_commandInfo.Remarks)) msgText += $"\n{_commandInfo.Remarks}";

            if (aliases.Count > 0) msgText += $"\n{"Alias".ToQuantity(aliases.Count, ShowQuantityAs.None)}: {string.Join(", ", aliases)}";

            return msgText;
        }

        #endregion Protected Methods
    }
}