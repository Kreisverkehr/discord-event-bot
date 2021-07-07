using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordEventBot.Common.Services;
using System.IO;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        #region Public Properties

        // Dependency Injection will fill this value in for us
        public PictureService PictureService { get; set; }

        #endregion Public Properties

        #region Public Methods

        // Ban a user
        [Command("ban")]
        [RequireContext(ContextType.Guild)]
        // make sure the user invoking the command can ban
        [RequireUserPermission(GuildPermission.BanMembers)]
        // make sure the bot itself can ban
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(IGuildUser user, [Remainder] string reason = null)
        {
            await user.Guild.AddBanAsync(user, reason: reason);
            await ReplyAsync("ok!");
        }

        [Command("cat")]
        public async Task CatAsync()
        {
            // Get a stream containing an image of a cat
            var stream = await PictureService.GetCatPictureAsync();
            // Streams must be seeked to their beginning before being uploaded!
            stream.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(stream, "cat.png");
        }

        // [Remainder] takes the rest of the command's arguments as one argument, rather than
        // splitting every space
        [Command("echo")]
        public Task EchoAsync([Remainder] string text)
            // Insert a ZWSP before the text to prevent triggering other bots!
            => ReplyAsync('\u200B' + text);

        // Setting a custom ErrorMessage property will help clarify the precondition error
        [Command("guild_only")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
        public Task GuildOnlyCommand()
            => ReplyAsync("Nothing to see here!");

        // 'params' will parse space-separated elements into a list
        [Command("list")]
        public Task ListAsync(params string[] objects)
            => ReplyAsync("You listed: " + string.Join("; ", objects));

        [Command("ping")]
        [Alias("pong", "hello")]
        public Task PingAsync()
            => ReplyAsync("pong!");

        // Get info on a user, or the user who invoked the command if one is not specified
        [Command("userinfo")]
        public async Task UserInfoAsync(IUser user = null)
        {
            user = user ?? Context.User;
            var guildUser = user as SocketGuildUser;

            var reply = $"Username: {user.Username}\n";
            reply += $"Discriminator:{user.DiscriminatorValue}\n";
            if (guildUser != null) reply += $"Nickname: {guildUser.Nickname}\n";
            reply += $"Registred since:{user.CreatedAt:yyyy-MM-dd HH:mm:ss}\n";
            if (guildUser != null) reply += $"on this server since:{guildUser.JoinedAt:yyyy-MM-dd HH:mm:ss}\n";
            reply += $"Status: {user.Status}\n";

            await ReplyAsync(reply);

            await user.SendMessageAsync("I know more about you, but don't tell anyone!");
        }

        #endregion Public Methods
    }
}