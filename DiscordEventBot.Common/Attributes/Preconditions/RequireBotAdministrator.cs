using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Attributes.Preconditions
{
    public class RequireBotAdministratorAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.User is SocketGuildUser gUser)
            {
                // if the user has server administrator rights, don't bother checking for a bot
                // admin role, just be successful
                if (new RequireUserPermissionAttribute(Discord.GuildPermission.Administrator).CheckPermissionsAsync(context, command, services).Result.IsSuccess)
                    return Task.FromResult(PreconditionResult.FromSuccess());

                // check if an admin role is set and wether the invoking user has it.
                using(var dbContext = services.GetRequiredService<IDbContextFactory<EventBotContext>>().CreateDbContext())
                {
                    var botAdminRole = dbContext.Guilds.Find(context.Guild.Id)?.AdminRole;

                    if(botAdminRole == null)
                        return Task.FromResult(PreconditionResult.FromError("There is no specific bot admin role set. Only the administrator can run this command."));

                    if (gUser.Roles.Any(r => r.Id == botAdminRole.RoleId))
                        return Task.FromResult(PreconditionResult.FromSuccess());

                    return Task.FromResult(PreconditionResult.FromError("You don't have the permission to run this command."));
                }
            }
            else
                return Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command."));
        }
    }
}
