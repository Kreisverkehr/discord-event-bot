using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.TypeReaders
{
    public class CultureInfoTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            try
            {
                var culture = new CultureInfo(input);
                return Task.FromResult(TypeReaderResult.FromSuccess(culture));
            }
            catch (CultureNotFoundException ex)
            {
                return Task.FromResult(TypeReaderResult.FromError(CommandError.ObjectNotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return Task.FromResult(TypeReaderResult.FromError(ex));
            }
        }
    }
}
