using Discord;
using Discord.Commands;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Common.RuntimeResults;
using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    [Group("admin")]
    [Alias("adm")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Group("set")]
        public class AdminSettingsModule : ModuleBase<SocketCommandContext>
        {
            public IDbContextFactory<EventBotContext> DbContextFactory { get; set; }
            public ISettings Settings { get; set; }
            [Command("admin-role")]
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
            [RequireOwner]
            public async Task<RuntimeResult> SetAdminRoleAsync(CultureInfo lang)
            {
                Settings.Culture = lang;

                // this is intentionally not localized. Every bot owner should understand this.
                await Context.Message.ReplyAsync("please restart me.");

                return await ReactionResult.FromReactionIntendAsync(ReactionIntend.Success);
            }
        }
        public CancellationTokenSource cancellation { get; set; }
        [Command("shutdown")]
        [Alias("exit", "quit")]
        [RequireOwner]
        public async Task SetAdminRoleAsync(TimeSpan? delay = null)
        {
            // this is intentionally not localized. Every bot owner should understand this.
            await Context.Message.ReplyAsync("shutting down...");
            new Thread(() =>
            {
                if (delay.HasValue)
                    cancellation.CancelAfter(delay.Value);
                else
                    cancellation.Cancel();
            }).Start();
        }
    }
}
