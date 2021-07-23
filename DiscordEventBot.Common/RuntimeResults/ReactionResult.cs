using Discord;
using Discord.Commands;
using Humanizer;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.RuntimeResults
{
    public enum ReactionIntend
    {
        Success,
        Error
    }

    public class ReactionResult : RuntimeResult
    {
        #region Public Constructors

        public ReactionResult(Emoji emoji, CommandError? error, string reason) : base(error, reason)
        {
            Emoji = emoji;
        }

        public ReactionResult(string emoji, CommandError? error, string reason = null) : this(new Emoji(emoji), error, reason)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public Emoji Emoji { get; }

        #endregion Public Properties

        #region Public Methods

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

        #endregion Public Methods
    }
}