using Discord;
using Discord.Commands;
using DiscordEventBot.Common.Attributes.Preconditions;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Common.RuntimeResults;
using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    [Group("admin")]
    [Alias("adm")]
    [LocalizedName("txt_mod_adm_name")]
    [LocalizedSummary("txt_mod_adm_sum")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        #region Public Properties

        public CancellationTokenSource cancellation { get; set; }

        #endregion Public Properties

        #region Public Methods

        [Command("shutdown")]
        [Alias("exit", "quit")]
        [LocalizedSummary("txt_mod_adm_cmd_exit_sum")]
        [RequireOwner]
        public async Task ShutdownAsync(TimeSpan delay = default(TimeSpan))
        {
            // this is intentionally not localized. Every bot owner should understand this.
            await Context.Message.ReplyAsync("shutting down...");
            new Thread(() => cancellation.CancelAfter(delay)).Start();
        }

        #endregion Public Methods

        #region Public Classes

        [Group("set")]
        public class AdminSettingsModule : ModuleBase<SocketCommandContext>
        {
            #region Public Properties

            public IDbContextFactory<EventBotContext> DbContextFactory { get; set; }

            public ISettings Settings { get; set; }

            #endregion Public Properties

            #region Public Methods

            [Command("admin-role")]
            [LocalizedSummary("txt_mod_admset_cmd_setadminrole_sum")]
            [RequireContext(ContextType.Guild)]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task<RuntimeResult> SetAdminRoleAsync(IRole role)
            {
                using (var dbContext = DbContextFactory.CreateDbContext())
                {
                    var guild = await dbContext.Guilds.FindOrCreateAsync(Context.Guild.Id);
                    guild.AdminRole = await dbContext.Roles.FindOrCreateAsync(role.Id);
                    await dbContext.SaveChangesAsync();
                }
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
            }

            [Command("language")]
            [Alias("lang")]
            [LocalizedSummary("txt_mod_admset_cmd_setlang_sum")]
            [RequireOwner]
            public async Task<RuntimeResult> SetLanguageAsync(CultureInfo lang)
            {
                Settings.Culture = lang;
                Settings.Save();

                // this is intentionally not localized. Every bot owner should understand this.
                await Context.Message.ReplyAsync("please restart me.");

                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
            }

            [Command("bot-channel")]
            [Alias("bot-chan", "bc")]
            [LocalizedSummary("txt_mod_admset_cmd_setbc_sum")]
            [RequireContext(ContextType.Guild)]
            [RequireBotAdministrator]
            public async Task<RuntimeResult> SetBotChannelAsync(IGuildChannel channel)
            {
                using (var dbContext = DbContextFactory.CreateDbContext())
                {
                    var dbGuild = await dbContext.Guilds.FindOrCreateAsync(Context.Guild.Id);
                    dbGuild.BotChannel = await dbContext.Channels.FindOrCreateAsync(channel.Id);
                    await dbContext.SaveChangesAsync();
                }
                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
            }

            #endregion Public Methods
        }

        #endregion Public Classes
    }
}