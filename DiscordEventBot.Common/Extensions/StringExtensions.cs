using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToFilename(this string text, string extension)
        {
            StringBuilder sb = new StringBuilder(text);
            foreach (var c in Path.GetInvalidFileNameChars())
                sb.Replace(c.ToString(), null);
            sb.Append('.');
            sb.Append(extension);
            return sb.ToString();
        }
    }
}
