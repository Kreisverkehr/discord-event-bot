using DiscordEventBot.Model;

namespace DiscordEventBot.Common.Extensions
{
    public static class AttendeeGroupExtensions
    {
        #region Public Methods

        public static string GetTitle(this AttendeeGroup group)
        {
            string result = $"{group.Name} [{group.Attendees?.Count ?? 0}";
            if (group.MaxCapacity.HasValue)
                result += $"/{group.MaxCapacity.Value}";
            result += "]";
            return result;
        }

        #endregion Public Methods
    }
}