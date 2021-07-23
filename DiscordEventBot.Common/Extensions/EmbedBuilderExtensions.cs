using Discord;
using System;

namespace DiscordEventBot.Common.Extensions
{
    public static class EmbedBuilderExtensions
    {
        #region Public Methods

        public static EmbedBuilder AddFieldIf(this EmbedBuilder builder, Func<bool> condition, EmbedFieldBuilder fieldBuilder)
        {
            if (condition()) builder.AddField(fieldBuilder);

            return builder;
        }

        public static EmbedBuilder AddFieldIf(this EmbedBuilder builder, Func<bool> condition, Action<EmbedFieldBuilder> action)
        {
            if (condition()) builder.AddField(action);

            return builder;
        }

        #endregion Public Methods
    }
}