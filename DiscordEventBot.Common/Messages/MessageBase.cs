using Discord;

namespace DiscordEventBot.Common.Messages
{
    public abstract class MessageBase
    {
        #region Private Fields

        private bool isBuilt = false;

        #endregion Private Fields

        #region Public Properties

        public Embed Embed { get; private set; }
        public string MessageText { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static implicit operator Embed(MessageBase msg)
        {
            msg.Build();
            return msg.Embed;
        }

        public static implicit operator string(MessageBase msg)
        {
            msg.Build();
            return msg.MessageText;
        }

        public void Build()
        {
            if (isBuilt) return;
            MessageText = BuildMessageText();
            Embed = BuildEmbed();
            isBuilt = true;
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual Embed BuildEmbed() => null;

        protected virtual string BuildMessageText() => string.Empty;

        #endregion Protected Methods
    }
}