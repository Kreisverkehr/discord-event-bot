using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common.Extensions;
using DiscordEventBot.Common.RuntimeResults;
using DiscordEventBot.Common.TypeReaders;
using DiscordEventBot.Model;
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
        private readonly EventBotContext _dbContext;
        private readonly DiscordSocketClient _discord;
        private readonly ResultReasonService _reasons;
        private readonly IServiceProvider _services;

        #endregion Private Fields

        #region Public Constructors

        public CommandHandlingService(IServiceProvider services, CommandService commands, DiscordSocketClient discord, ResultReasonService reasons, EventBotContext dbContext)
        {
            _commands = commands;
            _discord = discord;
            _services = services;
            _reasons = reasons;
            _dbContext = dbContext;

            _commands.CommandExecuted += CommandExecutedAsync;
            _discord.MessageReceived += MessageReceivedAsync;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
                result = await ReactionResult.FromReactionIntendAsync(ReactionIntend.Error, CommandError.UnknownCommand);

            _reasons.AddResult(result, context.Message);

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
        }

        public async Task InitializeAsync()
        {
            _commands.AddTypeReader<CommandInfo>(new CommandInfoTypeReader());
            _commands.AddTypeReader<ModuleInfo>(new ModuleInfoTypeReader());
            _commands.AddTypeReader<CultureInfo>(new CultureInfoTypeReader());
            _commands.AddTypeReader<EventTemplate>(new EventTemplateTypeReader());

            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            // check if the message could contain a vaild command
            var argPos = 0;
            if (!message.HasMentionPrefix(_discord.CurrentUser, ref argPos) && !(rawMessage.Channel is IDMChannel)) return;

            // check if this message was sent in the specified bot channel
            if (rawMessage.Channel is IGuildChannel guildChannel)
            {
                var guild = await _dbContext.Guilds.FindOrCreateAsync(guildChannel.GuildId);
                if (guild.BotChannel != null && guildChannel.Id != guild.BotChannel.ChannelId)
                {
                    // We have found a violator!
                    await rawMessage.DeleteAsync();
                    await rawMessage.Author.SendMessageAsync(Resources.Resources.txt_msg_wrong_channel);
                    return;
                }
            }

            var context = new SocketCommandContext(_discord, message);
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        #endregion Public Methods
    }
}