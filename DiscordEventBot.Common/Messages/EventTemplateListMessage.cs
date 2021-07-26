using Discord;
using DiscordEventBot.Model;
using Humanizer;
using System.Collections.Generic;
using System.Linq;

namespace DiscordEventBot.Common.Messages
{
    public class EventTemplateListMessage : MessageBase
    {
        #region Private Fields

        private IEnumerable<EventTemplate> _templates;

        #endregion Private Fields

        #region Public Constructors

        public EventTemplateListMessage(IEnumerable<EventTemplate> templates)
        {
            _templates = templates;
            HasEmbed = true;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder) =>
            base.BuildEmbed(embedBuilder)
            .WithFields(
                from template in _templates
                select new EmbedFieldBuilder()
                    .WithName($"#{template.EventTemplateID} {template.Name}")
                    .WithValue($"{Resources.Resources.txt_word_subject}: {template.Subject}\r\n"
                        + $"{Resources.Resources.txt_word_duration}: {template.Duration.Humanize()}\r\n"
                        + string.Join(' ', template.Groups.Select(g => $"{g.Name} [{g.MaxCapacity ?? 0}]"))
                    )
                )
            ;

        #endregion Protected Methods
    }
}