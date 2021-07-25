using Discord.Commands;
using DiscordEventBot.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.TypeReaders
{
    public class EventTemplateTypeReader : TypeReader
    {
        #region Public Methods

        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<EventBotContext>();

            IEnumerable<EventTemplate> templates =
                from template in dbContext.EventTemplates.AsQueryable()
                where template.Guild.GuildId == context.Guild.Id
                select template;

            ulong templateId;
            if (ulong.TryParse(input, out templateId))
                templates =
                    from template1 in templates
                    where template1.EventTemplateID == templateId
                    select template1;
            else
                templates =
                    from template2 in templates.AsEnumerable()
                    where template2.Name.ToLowerInvariant() == input.ToLowerInvariant()
                    select template2;

            var resultTemplate = templates.FirstOrDefault();

            if (resultTemplate == default(EventTemplate))
                return Task.FromResult(TypeReaderResult.FromError(CommandError.ObjectNotFound, $"Cannot find template {input}."));

            return Task.FromResult(TypeReaderResult.FromSuccess(resultTemplate));
        }

        #endregion Public Methods
    }
}