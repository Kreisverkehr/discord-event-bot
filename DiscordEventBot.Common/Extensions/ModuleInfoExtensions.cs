using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordEventBot.Common.Extensions
{
    public static class ModuleInfoExtensions
    {
        #region Private Fields

        private static Random random = new();

        #endregion Private Fields

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

        public static CommandInfo GetRandomCommand(this ModuleInfo module)
        {
            var cmds = module.GetCommandsRecursive().ToList();
            return cmds[random.Next(cmds.Count - 1)];
        }

        #endregion Public Methods
    }
}