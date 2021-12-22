using Discord;
using Discord.Interactions;
using DiscordEventBot.Common.Attributes.Preconditions;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Model;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    [LocalizedGroup("admin", "txt_mod_adm_sum")]
    public class AdminModule : InteractionModuleBase<SocketInteractionContext>
    {
        #region Public Properties

        public IHostApplicationLifetime _appLifetime { get; set; }

        #endregion Public Properties

        #region Public Methods

        [RequireOwner]
        [LocalizedSlashCommand("shutdown", "txt_mod_adm_cmd_exit_sum")]
        public async Task ShutdownAsync(
            [LocalizedParamSummary("delay", "txt_mod_adm_cmd_exit_param_delay_sum")]
            TimeSpan delay = default(TimeSpan)
            )
        {
            // this is intentionally not localized. Every bot owner should understand this.
            await RespondAsync("shutting down...");
            new Thread(() =>
            {
                Thread.Sleep(delay);
                _appLifetime.StopApplication();
            }).Start();
        }

        #endregion Public Methods

        #region Public Classes

        [LocalizedGroup("set", "txt_mod_adm_set_sum")]
        public class AdminSettingsModule : InteractionModuleBase<SocketInteractionContext>
        {
            #region Public Properties

            public EventBotContext DbContext { get; set; }

            public ISettings Settings { get; set; }

            #endregion Public Properties

            #region Public Methods

            [LocalizedSlashCommand("admin-role", "txt_mod_admset_cmd_setadminrole_sum")]
            [RequireContext(ContextType.Guild)]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task SetAdminRoleAsync(
                [LocalizedParamSummary("role", "txt_mod_admset_cmd_setadminrole_param_role_sum")]
                IRole role)
            {
                await DeferAsync(ephemeral: true);
                var guild = await DbContext.Guilds.FindOrCreateAsync(Context.Guild.Id);
                guild.AdminRole = await DbContext.Roles.FindOrCreateAsync(role.Id);
                await DbContext.SaveChangesAsync();
                await FollowupAsync("Done.", ephemeral: true);
            }

            [LocalizedSlashCommand("announcement-channel", "txt_mod_admset_cmd_setac_sum")]
            [RequireContext(ContextType.Guild)]
            [RequireBotAdministrator]
            public async Task SetAnnouncementChannelAsync(
                [LocalizedParamSummary("channel", "txt_mod_admset_cmd_setac_param_channel_sum")]
                ITextChannel channel)
            {
                await DeferAsync(ephemeral: true);
                var dbGuild = await DbContext.Guilds.FindOrCreateAsync(Context.Guild.Id);
                dbGuild.AnnouncementChannel = await DbContext.Channels.FindOrCreateAsync(channel.Id);
                await DbContext.SaveChangesAsync();
                await FollowupAsync("Done.", ephemeral: true);
            }

            [LocalizedSlashCommand("bot-channel", "txt_mod_admset_cmd_setbc_sum")]
            [RequireContext(ContextType.Guild)]
            [RequireBotAdministrator]
            public async Task SetBotChannelAsync(
                [LocalizedParamSummary("channel", "txt_mod_admset_cmd_setbc_param_channel_sum")]
                ITextChannel channel)
            {
                await DeferAsync(ephemeral: true);
                var dbGuild = await DbContext.Guilds.FindOrCreateAsync(Context.Guild.Id);
                dbGuild.BotChannel = await DbContext.Channels.FindOrCreateAsync(channel.Id);
                await DbContext.SaveChangesAsync();
                await FollowupAsync("Done.", ephemeral: true);
            }

            [LocalizedSlashCommand("language", "txt_mod_admset_cmd_setlang_sum")]
            [RequireOwner]
            public async Task SetLanguageAsync(
                [LocalizedParamSummary("lang", "txt_mod_admset_cmd_setlang_param_lang_sum")]
                CultureInfo lang)
            {
                await DeferAsync(ephemeral: true);
                Settings.Culture = lang;
                Settings.Save();

                // this is intentionally not localized. Every bot owner should understand this.
                await FollowupAsync("please restart me.", ephemeral: true);
            }

            #endregion Public Methods
        }

        #endregion Public Classes
    }
}