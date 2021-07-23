namespace DiscordEventBot.Common.Messages
{
    public class TextMessage : MessageBase
    {
        #region Private Fields

        private string _text;

        #endregion Private Fields

        #region Public Constructors

        public TextMessage(string text)
        {
            _text = text;
        }

        #endregion Public Constructors

        #region Public Methods

        public static TextMessage FromString(string text) => new(text);

        #endregion Public Methods

        #region Protected Methods

        protected override string BuildMessageText()
        {
            return _text;
        }

        #endregion Protected Methods
    }
}