using Discord;
using Discord.Commands;
using System.Collections.Generic;

namespace DiscordEventBot.Common.Services
{
    public class ResultReasonService
    {
        #region Private Fields

        private Dictionary<ulong, IResult> _lastMessageResult = new();
        private Dictionary<ulong, (ulong, IResult)> _lastUserResult = new();
        private Queue<ulong> _messageResultsPurgeQueue = new();

        #endregion Private Fields

        #region Public Methods

        public void AddResult(IResult result, IMessage message)
        {
            if (_lastUserResult.ContainsKey(message.Author.Id))
                _lastUserResult[message.Author.Id] = new(message.Id, result);
            else
                _lastUserResult.Add(message.Author.Id, new(message.Id, result));

            _lastMessageResult.Add(message.Id, result);
            _messageResultsPurgeQueue.Enqueue(message.Id);

            // TODO: make this configurable via settings
            int maxBacklog = 100;
            if (_lastMessageResult.Count > maxBacklog)
                _lastMessageResult.Remove(_messageResultsPurgeQueue.Dequeue());
        }

        public (ulong?, IResult) GetLastResultForUser(ulong userId)
        {
            if (!_lastUserResult.ContainsKey(userId)) return new(null, null);

            return _lastUserResult[userId];
        }

        public IResult GetResultForMessage(ulong messageId)
        {
            if (!_lastMessageResult.ContainsKey(messageId)) return null;

            return _lastMessageResult[messageId];
        }

        #endregion Public Methods
    }
}