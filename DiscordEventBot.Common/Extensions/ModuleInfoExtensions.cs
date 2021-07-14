using Discord.Commands;
using System.Collections.Generic;

namespace DiscordEventBot.Common.Extensions
{
    public static class ModuleInfoExtensions
    {
        #region Public Methods

        public static IEnumerable<CommandInfo> GetCommandsRecursive(this ModuleInfo module)
        {
            foreach (var cmd in module.Commands)
                yield return cmd;
            if (module.Submodules != null && module.Submodules.Count > 0)
                foreach (var subModule in module.Submodules)
                    foreach (var cmd in subModule.GetCommandsRecursive())
                        yield return cmd;
        }

        public static string GetPrefixRecursive(this ModuleInfo module)
        {
            if (module.Parent != null)
                return $"{module.Parent.GetPrefixRecursive()} {module.Group}";
            else return module.Group;
        }

        #endregion Public Methods
    }
}