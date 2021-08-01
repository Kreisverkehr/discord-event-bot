using Discord.Commands;

namespace DiscordEventBot.Common.Extensions
{
    public static class CommandInfoExtensions
    {
        #region Public Methods

        public static string GetSignature(this CommandInfo command, bool simplified = true)
        {
            var sig = "**";
            if (!string.IsNullOrWhiteSpace(command.Module.Group))
                sig += $"{command.Module.GetPrefixRecursive()} ";
            sig += $"{command.Name}** ";
            foreach (var param in command.Parameters)
                sig += param.FormatMd(withType: !simplified) + " ";
            return sig;
        }

        #endregion Public Methods
    }
}