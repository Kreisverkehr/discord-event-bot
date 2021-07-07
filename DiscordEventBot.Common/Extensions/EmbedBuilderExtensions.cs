using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Extensions
{
    public static class EmbedBuilderExtensions
    {
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
    }
}
