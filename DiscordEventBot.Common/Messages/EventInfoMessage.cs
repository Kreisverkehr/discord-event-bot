using Discord;
using Discord.WebSocket;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Model;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Messages
{
    public class EventInfoMessage : MessageBase
    {
        #region Private Fields

        private DiscordSocketClient _client;
        private EventBotContext _dbContext;
        private Dictionary<string, AttendeeGroup> _EmojiToGroup = new();
        private Event _event;
        private Stack<string> _groupEmojiStack = new();
        private Dictionary<AttendeeGroup, string> _groupToEmoji = new();
        private IUserMessage _message;

        #endregion Private Fields

        #region Public Constructors

        public EventInfoMessage(Event evt, DiscordSocketClient client, EventBotContext dbContext)
        {
            _event = evt;
            _client = client;
            _dbContext = dbContext;
            HasEmbed = true;
            InitEmojiStack();
            foreach (var group in _event.Groups)
            {
                _groupToEmoji.Add(group, _groupEmojiStack.Peek());
                _EmojiToGroup.Add(_groupEmojiStack.Pop(), group);
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public override async Task Sent(IUserMessage message)
        {
            await base.Sent(message);

            await message.AddReactionAsync(new Emoji("☑"));
            await message.AddReactionAsync(new Emoji("⏹"));
            foreach (var group in _groupToEmoji)
                await message.AddReactionAsync(new Emoji(group.Value));

            _message = message;
            _client.ReactionAdded += HandleReaction;
            new Thread(() =>
            {
                Thread.Sleep(new TimeSpan(0, 5, 0));
                _client.ReactionAdded -= HandleReaction;
                _message.RemoveAllReactionsAsync().Wait();
            }).Start();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override EmbedBuilder BuildEmbed(EmbedBuilder embedBuilder) => base.BuildEmbed(embedBuilder)
                .WithTitle($"[#{_event.EventID}] {_event.Subject}")
                .WithDescription(_event.Description)
                .WithColor(Color.Orange)
                .WithTimestamp(_event.CreatedOn)
                .WithAuthor(_client.GetUser(_event.Creator.UserId))
                .AddField(fb => fb
                    .WithName(Resources.Resources.txt_word_starttime)
                    .WithValue(_event.Start.ToString("f"))
                )
                .AddField(fb => fb
                    .WithName(Resources.Resources.txt_word_endtime)
                    .WithValue(_event.Start.Add(_event.Duration).ToString("f"))
                )
                .AddFieldIf(() => _event.Attendees != null && _event.Attendees.Count > 0, fb => fb
                    .WithName(Resources.Resources.txt_word_attendee.ToQuantity(_event.Attendees.Count, ShowQuantityAs.None))
                    .WithValue(string.Join('\n', _event.Attendees.Select(at => _client.GetUser(at.UserId).Mention)))
                )
                .WithFields(_event.Groups.Select(grp => new EmbedFieldBuilder()
                    .WithIsInline(true)
                    .WithName(_groupToEmoji[grp] + grp.GetTitle())
                    .WithValue(string.Join('\n', grp.Attendees.Select(at => _client.GetUser(at.UserId).Mention)) + ".")
                ))
                ;

        #endregion Protected Methods

        #region Private Methods

        private async Task HandleReaction(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (msg.Id == _message.Id && reaction.UserId != _client.CurrentUser.Id)
            {
                if (reaction.Emote.Name == "☑" && !_event.Attendees.Contains(await _dbContext.Users.FindOrCreateAsync(reaction.UserId)))
                {
                    _event.Attendees.Add(await _dbContext.Users.FindOrCreateAsync(reaction.UserId));
                }

                if (reaction.Emote.Name == "⏹")
                {
                    _event.Attendees.Remove(await _dbContext.Users.FindOrCreateAsync(reaction.UserId));
                    foreach (var grp in _event.Groups)
                        grp.Attendees.Remove(await _dbContext.Users.FindOrCreateAsync(reaction.UserId));
                }

                if (_groupToEmoji.Values.Contains(reaction.Emote.Name)
                    && !_EmojiToGroup[reaction.Emote.Name].Attendees.Contains(_dbContext.Users.FindOrCreate(reaction.UserId))
                    && ((_EmojiToGroup[reaction.Emote.Name].MaxCapacity.HasValue && _EmojiToGroup[reaction.Emote.Name].Attendees.Count < _EmojiToGroup[reaction.Emote.Name].MaxCapacity.Value) || !_EmojiToGroup[reaction.Emote.Name].MaxCapacity.HasValue))
                {
                    _EmojiToGroup[reaction.Emote.Name].Attendees.Add(await _dbContext.Users.FindOrCreateAsync(reaction.UserId));
                }

                _dbContext.SaveChanges();
                var rawMsg = await msg.GetOrDownloadAsync();
                await rawMsg.RemoveReactionAsync(reaction.Emote, reaction.UserId);
                Rebuild();
                await rawMsg.ModifyAsync((prop) => prop.Embed = Embed);
            }
        }

        private void InitEmojiStack()
        {
            _groupEmojiStack.Push("🔟");
            _groupEmojiStack.Push("9️⃣");
            _groupEmojiStack.Push("8️⃣");
            _groupEmojiStack.Push("7️⃣");
            _groupEmojiStack.Push("6️⃣");
            _groupEmojiStack.Push("5️⃣");
            _groupEmojiStack.Push("4️⃣");
            _groupEmojiStack.Push("3️⃣");
            _groupEmojiStack.Push("2️⃣");
            _groupEmojiStack.Push("1️⃣");
            _groupEmojiStack.Push("0️⃣");
        }

        #endregion Private Methods
    }
}