using Discord;
using Discord.Commands;
using Humanizer;
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
        public ReactionResult(Emoji emoji, CommandError? error, string reason) : base(error, reason)
        {
            Emoji = emoji;
        }

        public ReactionResult(string emoji, CommandError? error, string reason = null) : this(new Emoji(emoji), error, reason)
        {
        }

        public static Task<ReactionResult> FromReactionIntendAsync(ReactionIntend intend, CommandError? error = null, string reason = null)
        {
            switch (intend)
            {
                case ReactionIntend.Success:
                    return Task.FromResult(new ReactionResult(new Emoji("✅"), error, reason));
                case ReactionIntend.Error:
                    return Task.FromResult(new ReactionResult(new Emoji("❌"), error, reason));
            }

            return null;
        }

        public override string ToString()
        {
            if (IsSuccess)
                return base.ToString();

            if (!string.IsNullOrWhiteSpace(Reason))
                return Reason;

            return Error.Humanize();
        }
    }

    public enum ReactionIntend
    {
        Success,
        Error
    }
}
