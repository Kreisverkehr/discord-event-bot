using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common.RuntimeResults;
using DiscordEventBot.Common.TypeReaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Services
{
    public class CommandHandlingService
    {
        #region Private Fields

        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        #endregion Private Fields

        #region Public Constructors

        public CommandHandlingService(IServiceProvider services, CommandService commands, DiscordSocketClient discord)
        {
            _commands = commands;
            _discord = discord;
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
            _discord.MessageReceived += MessageReceivedAsync;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't
            // care about these errors
            if (!command.IsSpecified)
                return;

            if (result.IsSuccess)
            {
                switch (result)
                {
                    case ResponseMessageResult response:
                        await response.SendAsync(context.Channel);
                        break;
                    case ReactionResult reaction:
                        await context.Message.AddReactionAsync(reaction.Emoji);
                        break;
                    default:
                        return;
                }
                return;
            }

            await context.Channel.SendMessageAsync($"error: {result}");
        }

        public async Task InitializeAsync()
        {
            _commands.AddTypeReader<CommandInfo>(new CommandInfoTypeReader());
            _commands.AddTypeReader<ModuleInfo>(new ModuleInfoTypeReader());
            _commands.AddTypeReader<CultureInfo>(new CultureInfoTypeReader());

            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            var argPos = 0;
            if (!message.HasMentionPrefix(_discord.CurrentUser, ref argPos) && !(rawMessage.Channel is IDMChannel)) return;

            var context = new SocketCommandContext(_discord, message);
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        #endregion Public Methods
    }
}