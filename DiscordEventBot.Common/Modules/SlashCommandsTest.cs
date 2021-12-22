using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    public class SlashCommandsTest : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("test-method", "this is a testmethod")]
        public async Task TestMethod()
        {
            //await ReplyAsync("Test successful");
            await this.RespondAsync("Test successful");
        }
    }
}
