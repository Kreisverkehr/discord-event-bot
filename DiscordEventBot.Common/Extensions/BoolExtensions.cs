namespace DiscordEventBot.Common.Extensions
{
    public static class BoolExtensions
    {
        #region Public Methods

        public static string ToYesNo(this bool val)
        {
            return val ? Resources.Resources.txt_word_yes : Resources.Resources.txt_word_no;
        }

        #endregion Public Methods
    }
}