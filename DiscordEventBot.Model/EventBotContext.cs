using Microsoft.EntityFrameworkCore;

namespace DiscordEventBot.Model
{
    public class EventBotContext : DbContext
    {
        #region Public Constructors

        public EventBotContext(DbContextOptions<EventBotContext> options)
            : base(options) { }

        #endregion Public Constructors

        #region Public Properties

        public DbSet<Event> Events { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Guild> Guilds { get; set; }

        #endregion Public Properties
    }
}