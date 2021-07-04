using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace DiscordEventBot.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (EventBotContext DbCtx = new EventBotContext())
                DbCtx.Database.Migrate();
        }
    }
}
