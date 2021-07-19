﻿using Discord;
using Discord.Commands;
using DiscordEventBot.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    [LocalizedName("txt_mod_misc_name")]
    [LocalizedSummary("txt_mod_misc_sum")]
    public class MiscModule : ModuleBase<SocketCommandContext>
    {
        private readonly ResultReasonService _reasons;

        public MiscModule(ResultReasonService reasons)
        {
            _reasons = reasons;
        }

        [Command("why")]
        [LocalizedSummary("txt_mod_misc_cmd_why_sum")]
        [LocalizedRemarks("txt_mod_misc_cmd_why_rem")]
        public async Task WhyAsync()
        {
            if (Context.Message.ReferencedMessage == null)
            {
                (ulong?, IResult) result = _reasons.GetLastResultForUser(Context.User.Id);
                if (result.Item1.HasValue)
                {
                    IMessage msg = await Context.Channel.GetMessageAsync(result.Item1.Value);
                    if (msg is IUserMessage userMsg)
                    {
                        await userMsg.ReplyAsync(result.Item2.ToString());
                    }
                } 
            } else
            {
                IResult result = _reasons.GetResultForMessage(Context.Message.ReferencedMessage.Id);
                await Context.Message.ReferencedMessage.ReplyAsync(result.ToString());
            }
        }
    }
}