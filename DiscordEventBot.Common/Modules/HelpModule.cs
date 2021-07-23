﻿using Discord.Commands;
using DiscordEventBot.Common.Messages;
using DiscordEventBot.Common.RuntimeResults;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    [LocalizedName("txt_mod_help_name")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        #region Private Fields

        private readonly CommandService _service;

        #endregion Private Fields

        #region Public Constructors

        public HelpModule(CommandService service)
        {
            _service = service;
        }

        #endregion Public Constructors

        #region Public Methods

        [Command("help")]
        [Alias("about", "man", "hlp", "?")]
        [LocalizedSummary("txt_cmd_help_help_sum")]
        [LocalizedRemarks("txt_cmd_help_help_rem")]
        public Task<RuntimeResult> HelpAsync() =>
            ResponseMessageResult.FromMessageAsync(new CommandOverviewMessage(_service, Context));

        [Command("help")]
        [Alias("about", "man", "hlp", "?")]
        [LocalizedRemarks("txt_cmd_help_help_cmd_sum")]
        public Task<RuntimeResult> HelpAsync([Remainder] CommandInfo command) =>
            ResponseMessageResult.FromMessageAsync(new CommandHelpMessage(command));

        [Command("help")]
        [Alias("about", "man", "hlp", "?")]
        [LocalizedSummary("txt_cmd_help_help_mod_sum")]
        [LocalizedRemarks("txt_cmd_help_help_rem")]
        public Task<RuntimeResult> HelpAsync(ModuleInfo module) =>
            ResponseMessageResult.FromMessageAsync(new CommandModuleOverviewMessage(module, Context));

        #endregion Public Methods
    }
}