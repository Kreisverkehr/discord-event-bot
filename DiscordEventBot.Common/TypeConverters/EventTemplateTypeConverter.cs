using Discord;
using Discord.Interactions;
using DiscordEventBot.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.TypeConverters
{
    public class EventTemplateTypeConverter : TypeConverter
    {
        #region Public Methods

        public override bool CanConvertTo(Type type) => type == typeof(EventTemplate);

        public override ApplicationCommandOptionType GetDiscordType() => ApplicationCommandOptionType.String;

        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<EventBotContext>();
            var input = option.Value.ToString();

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
                return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"Cannot find template {input}."));

            return Task.FromResult(TypeConverterResult.FromSuccess(resultTemplate));
        }

        #endregion Public Methods
    }
}