using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DiscordEventBot.Model
{
    public class EventBotContextFactory : IDesignTimeDbContextFactory<EventBotContext>
    {
        #region Public Methods

        public EventBotContext CreateDbContext(string[] args) =>
            new EventBotContext(new DbContextOptionsBuilder<EventBotContext>()
            .UseSqlite("Data Source=EventBot.db")
            .UseLazyLoadingProxies()
            .Options);

        #endregion Public Methods
    }
}