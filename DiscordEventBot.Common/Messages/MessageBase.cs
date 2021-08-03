using Discord;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Messages
{
    public abstract class MessageBase
    {
        #region Private Fields

        private byte[] _attachmentData;
        private string _attachmentName;
        private bool isBuilt = false;

        #endregion Private Fields

        #region Public Properties

        public Embed Embed { get; private set; }

        public virtual bool HasAttachment { get; protected set; } = false;

        public virtual bool HasEmbed { get; protected set; } = false;

        public string MessageText { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public static implicit operator Embed(MessageBase msg)
        {
            msg.Build();
            return msg.Embed;
        }

        public static implicit operator string(MessageBase msg)
        {
            msg.Build();
            return msg.MessageText;
        }

        public void Build()
        {
            if (isBuilt) return;
            MessageText = BuildMessageText();
            Embed = BuildEmbed(new EmbedBuilder()).Build();

            if (HasAttachment)
                using (MemoryStream memoryStream = new())
                {
                    using (Stream attachmentStream = BuildAttachment(ref _attachmentName))
                        attachmentStream.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    _attachmentData = memoryStream.ToArray();
                }

            isBuilt = true;
            AfterBuild(ref isBuilt);
        }

        public Stream GetAttachmentData() => new MemoryStream(_attachmentData);

        public string GetAttachmentName() => _attachmentName;

        public virtual async Task Sent(IUserMessage message, CancellationToken token)
        {
            await Task.CompletedTask;
            return;
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void AfterBuild(ref bool isBuilt)
        {
        }

        protected virtual Stream BuildAttachment(ref string name) => new MemoryStream();

        protected virtual EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder) => embedBuilder;

        protected virtual string BuildMessageText() => string.Empty;

        protected void Rebuild()
        {
            isBuilt = false;
            Build();
        }

        #endregion Protected Methods
    }
}