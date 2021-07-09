using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordEventBot.Model
{
    public class EventBotContextFactory : IDesignTimeDbContextFactory<EventBotContext>
    {
        public EventBotContext CreateDbContext(string[] args) => 
            new EventBotContext(new DbContextOptionsBuilder<EventBotContext>()
            .UseSqlite("Data Source=EventBot.db")
            .UseLazyLoadingProxies()
            .Options);
    }
}
