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

        public DbSet<Channel> Channels { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventTemplate> EventTemplates { get; set; }

        public DbSet<Guild> Guilds { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        #endregion Public Properties

        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasMany(x => x.Attendees).WithMany(x => x.Events);
            modelBuilder.Entity<Event>().HasOne(x => x.Creator);
        }

        #endregion Protected Methods
    }
}