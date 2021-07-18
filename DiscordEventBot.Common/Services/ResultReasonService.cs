using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Services
{
    public class ResultReasonService
    {
        private Dictionary<ulong, (ulong, IResult)> _lastUserResult = new();
        private Dictionary<ulong, IResult> _lastMessageResult = new();
        private Queue<ulong> _messageResultsPurgeQueue = new();

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
    }
}
