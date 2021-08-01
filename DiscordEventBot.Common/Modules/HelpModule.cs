using Discord.Commands;
using DiscordEventBot.Common.Messages;
using DiscordEventBot.Common.RuntimeResults;
using System;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    [LocalizedName("txt_mod_help_name")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        #region Private Fields

        private readonly CommandService _service;
        private readonly IServiceProvider _serviceProvider;

        #endregion Private Fields

        #region Public Constructors

        public HelpModule(CommandService service, IServiceProvider serviceProvider)
        {
            _service = service;
            _serviceProvider = serviceProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        [Command("help")]
        [Alias("about", "man", "hlp", "?")]
        [LocalizedSummary("txt_cmd_help_help_sum")]
        [LocalizedRemarks("txt_cmd_help_help_rem")]
        public Task<RuntimeResult> HelpAsync() =>
            ResponseMessageResult.FromMessageAsync(new CommandOverviewMessage(_service, Context, _serviceProvider));

        [Command("help")]
        [Alias("about", "man", "hlp", "?")]
        [LocalizedRemarks("txt_cmd_help_help_cmd_sum")]
        public Task<RuntimeResult> HelpAsync(
            [Remainder]
            [LocalizedSummary("txt_mod_help_cmd_help_param_command_sum")]
            CommandInfo command) =>
            ResponseMessageResult.FromMessageAsync(new CommandHelpMessage(command));

        [Command("help")]
        [Alias("about", "man", "hlp", "?")]
        [LocalizedSummary("txt_cmd_help_help_mod_sum")]
        [LocalizedRemarks("txt_cmd_help_help_rem")]
        public Task<RuntimeResult> HelpAsync(
            [LocalizedSummary("txt_mod_help_cmd_help_param_module_sum")]
            ModuleInfo module) =>
            ResponseMessageResult.FromMessageAsync(new CommandModuleOverviewMessage(module, Context, _serviceProvider));

        #endregion Public Methods
    }
}