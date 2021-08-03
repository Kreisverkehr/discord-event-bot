using Discord.Commands;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Common.RuntimeResults;
using DiscordEventBot.Model;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    [Group("set")]
    [Alias("setting")]
    [LocalizedName("txt_mod_set_name")]
    [LocalizedSummary("txt_mod_set_sum")]
    public class UserSettingsModule : ModuleBase<SocketCommandContext>
    {
        #region Public Properties

        public EventBotContext DbContext { get; set; }

        #endregion Public Properties

        #region Public Methods

        [Command("notify-time")]
        [Alias("nt")]
        [LocalizedSummary("txt_mod_set_cmd_nt_sum")]
        public async Task<RuntimeResult> SetNotifyTimeAsync(
            [LocalizedSummary("txt_mod_set_cmd_nt_param_notifytime_sum")]
            int notifyTime
        )
        {
            User user = await DbContext.Users.FindOrCreateAsync(Context.User.Id);
            user.MinutesBeforeEventNotify = notifyTime;
            await DbContext.SaveChangesAsync();

            return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
        }

        #endregion Public Methods
    }
}