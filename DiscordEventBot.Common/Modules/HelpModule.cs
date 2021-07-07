using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;

        public HelpModule(CommandService service)
        {
            _service = service;
        }

        [Command("help")]
        [Summary("Displays every command that you can use")]
        [Remarks("This command only displays commands that you are able to use.")]
        public async Task HelpAsync()
        {
            var builder = new EmbedBuilder()
            {
                Color = Color.DarkGreen,
                Title = "Bot Commands",
                Description = "These are the commands you can use",
            };

            foreach (var module in _service.Modules)
            {
                string description = null;
                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                        description += $"{cmd.Aliases.First()}\n";
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync("", false, builder.Build());
        }

    }
}
