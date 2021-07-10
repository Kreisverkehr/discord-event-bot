using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.RuntimeResults
{
    public class ReactionResult : RuntimeResult
    {
        public Emoji Emoji { get; }
        public ReactionResult(Emoji emoji) : base(null, null)
        {
            Emoji = emoji;
        }

        public ReactionResult(string emoji) : this(new Emoji(emoji))
        {
        }

        public static Task<ReactionResult> FromReactionIntendAsync(ReactionIntend intend)
        {
            switch(intend)
            {
                case ReactionIntend.Success:
                    return Task.FromResult(new ReactionResult(new Emoji("✅")));
            }

            return null;
        }
    }

    public enum ReactionIntend
    {
        Success
    }
}
