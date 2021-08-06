using Discord.Commands;
using System;
using System.Linq;

namespace DiscordEventBot.Common.Extensions
{
    public static class CommandServiceExtensions
    {
        #region Private Fields

        private static Random random = new();

        #endregion Private Fields

        #region Public Methods

        public static ModuleInfo GetRandomModule(this CommandService commandService)
        {
            var modules = commandService.Modules.Where(m => !m.Aliases.Contains("help")).ToList();
            return modules[random.Next(modules.Count - 1)];
        }

        #endregion Public Methods
    }
}