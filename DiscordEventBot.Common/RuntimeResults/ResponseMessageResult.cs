using Discord;
using Discord.Commands;
using DiscordEventBot.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.RuntimeResults
{
    public class ResponseMessageResult : RuntimeResult
    {
        public MessageBase Message { get; private set; }

        private ResponseMessageResult(MessageBase message) : base(null, null)
        {
            Message = message;
            Message.Build();
        }

        public static RuntimeResult FromMessage(MessageBase message) => new ResponseMessageResult(message);
        public static RuntimeResult FromMessage(string messageText) => new ResponseMessageResult(TextMessage.FromString(messageText));
        public static Task<RuntimeResult> FromMessageAsync(MessageBase message) => Task.FromResult(FromMessage(message));
        public static Task<RuntimeResult> FromMessageAsync(string messageText) => Task.FromResult(FromMessage(messageText));
        public async Task SendAsync(IMessageChannel channel)
        {
            Message.Build();

            if(Message.HasEmbed) await channel.SendMessageAsync(Message.MessageText, embed: Message.Embed);
            else await channel.SendMessageAsync(Message.MessageText);
        }
    }
}
