using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.TypeConverters
{
    public class CultureInfoTypeConverter : TypeConverter
    {
        #region Public Methods

        public override bool CanConvertTo(Type type) => type == typeof(CultureInfo);

        public override ApplicationCommandOptionType GetDiscordType() => ApplicationCommandOptionType.String;

        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
        {
            var input = option.Value.ToString();
            try
            {
                var culture = new CultureInfo(input);
                return Task.FromResult(TypeConverterResult.FromSuccess(culture));
            }
            catch (CultureNotFoundException ex)
            {
                return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, ex.Message));
            }
            catch (Exception ex)
            {
                return Task.FromResult(TypeConverterResult.FromError(ex));
            }
        }

        public override void Write(ApplicationCommandOptionProperties properties, IParameterInfo parameter)
        {
            base.Write(properties, parameter);
            foreach (var culture in SupportedLanguages())
            {
                properties.Choices.Add(new()
                {
                    Name = culture.DisplayName,
                    Value = culture.Name
                });
            }
        }

        private IEnumerable<CultureInfo> SupportedLanguages()
        {
            yield return new CultureInfo("en-US");
            yield return new CultureInfo("en");
            yield return new CultureInfo("de");
        }

        #endregion Public Methods
    }
}