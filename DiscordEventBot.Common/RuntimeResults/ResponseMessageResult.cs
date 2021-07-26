using Discord;
using Discord.Commands;
using DiscordEventBot.Common.Messages;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.RuntimeResults
{
    public class ResponseMessageResult : RuntimeResult
    {
        #region Private Constructors

        private ResponseMessageResult(MessageBase message) : base(null, null)
        {
            Message = message;
            Message.Build();
        }

        #endregion Private Constructors

        #region Public Properties

        public MessageBase Message { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static RuntimeResult FromMessage(MessageBase message) => new ResponseMessageResult(message);

        public static RuntimeResult FromMessage(string messageText) => new ResponseMessageResult(TextMessage.FromString(messageText));

        public static Task<RuntimeResult> FromMessageAsync(MessageBase message) => Task.FromResult(FromMessage(message));

        public static Task<RuntimeResult> FromMessageAsync(string messageText) => Task.FromResult(FromMessage(messageText));

        public async Task SendAsync(IMessageChannel channel)
        {
            Message.Build();
            IUserMessage userMessage = null;

            if (!Message.HasEmbed && !Message.HasAttachment)
                userMessage = await channel.SendMessageAsync(text: Message.MessageText);
            else if (Message.HasEmbed && !Message.HasAttachment)
                userMessage = await channel.SendMessageAsync(text: Message.MessageText, embed: Message.Embed);
            else if (!Message.HasEmbed && Message.HasAttachment)
                userMessage = await channel.SendFileAsync(Message.GetAttachmentData(), Message.GetAttachmentName(), text: Message.MessageText);
            else //if (Message.HasEmbed && Message.HasAttachment)
                userMessage = await channel.SendFileAsync(Message.GetAttachmentData(), Message.GetAttachmentName(), text: Message.MessageText, embed: Message.Embed);



            await Message.Sent(userMessage);
        }

        #endregion Public Methods
    }
}