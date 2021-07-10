using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace DiscordEventBot.Tests
{
    [TestClass]
    public class DbTests
    {
        #region Public Constructors

        public DbTests()
        {
            // start fresh
            if (File.Exists("EventBotTest.db")) File.Delete("EventBotTest.db");

            using (EventBotContext DbCtx = new EventBotContext(ConfigureDb()))
                DbCtx.Database.Migrate();
        }

        #endregion Public Constructors

        #region Public Methods

        [TestMethod]
        public void EventCreation()
        {
            using (EventBotContext DbCtx = new EventBotContext(ConfigureDb()))
            {
                Event evt1 = new Event()
                {
                    Subject = "Test",
                    Start = DateTime.Now,
                    Duration = new TimeSpan(1, 30, 0)
                };

                Assert.AreEqual(0UL, evt1.EventID);
                DbCtx.Events.Add(evt1);
                DbCtx.SaveChanges();
                Assert.AreNotEqual(0UL, evt1.EventID);

                Event evt2 = new Event()
                {
                    Subject = "Test2",
                    Start = DateTime.Now,
                    Duration = new TimeSpan(1, 30, 0)
                };

                Assert.AreEqual(0UL, evt2.EventID);
                DbCtx.Events.Add(evt2);
                DbCtx.SaveChanges();
                Assert.AreNotEqual(0UL, evt2.EventID);

                Assert.AreNotEqual(evt1.EventID, evt2.EventID);
            }
        }

        [TestMethod]
        public void EventCreationWithAttendeeGroups()
        {
            using (EventBotContext DbCtx = new EventBotContext(ConfigureDb()))
            {
                Event evt1 = new Event()
                {
                    Subject = "Attendee Group Test",
                    Start = DateTime.Now,
                    Duration = new TimeSpan(1, 30, 0)
                };

                Assert.AreEqual(0UL, evt1.EventID);

                AttendeeGroup grp1 = new AttendeeGroup()
                {
                    Name = "TestGrp1",
                    MaxCapacity = 5
                };
                Assert.AreEqual(0UL, grp1.GroupID);
                evt1.Groups.Add(grp1);

                User att1 = new User()
                {
                    UserId = 123456789
                };
                grp1.Attendees.Add(att1);

                User att2 = new User()
                {
                    UserId = 987654321
                };
                grp1.Attendees.Add(att2);

                DbCtx.Events.Add(evt1);
                DbCtx.SaveChanges();

                Assert.AreNotEqual(0UL, evt1.EventID);
                Assert.AreNotEqual(0UL, grp1.GroupID);
            }
        }

        [TestMethod]
        public void EventCreationWithAttendees()
        {
            using (EventBotContext DbCtx = new EventBotContext(ConfigureDb()))
            {
                Event evt1 = new Event()
                {
                    Subject = "Attendee Test",
                    Start = DateTime.Now,
                    Duration = new TimeSpan(1, 30, 0)
                };

                Assert.AreEqual(0UL, evt1.EventID);

                User att1 = new User()
                {
                    //UserId = 123456789
                };
                evt1.Attendees.Add(att1);

                User att2 = new User()
                {
                    //UserId = 987654321
                };
                evt1.Attendees.Add(att2);

                DbCtx.Events.Add(evt1);

                DbCtx.SaveChanges();

                Assert.AreNotEqual(0UL, evt1.EventID);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private DbContextOptions<EventBotContext> ConfigureDb() =>
                                    new DbContextOptionsBuilder<EventBotContext>()
            .UseSqlite($"Data Source = EventBotTest.db")
            .UseLazyLoadingProxies()
            .Options;

        #endregion Private Methods
    }
}