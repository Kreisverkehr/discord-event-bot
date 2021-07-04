using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiscordEventBot.Tests
{
    [TestClass]
    public class DbTests
    {
        public DbTests()
        {
            using (EventBotContext DbCtx = new EventBotContext())
                DbCtx.Database.Migrate();
        }

        [TestMethod]
        public void EventCreation()
        {
            using (EventBotContext DbCtx = new EventBotContext())
            {
                Event evt1 = new Event()
                {
                    Subject = "Test",
                    Start = DateTimeOffset.Now,
                    Duration = new TimeSpan(1, 30, 0)
                };

                Assert.AreEqual(Guid.Empty, evt1.EventID);
                DbCtx.Events.Add(evt1);
                DbCtx.SaveChanges();
                Assert.AreNotEqual(Guid.Empty, evt1.EventID);


                Event evt2 = new Event()
                {
                    Subject = "Test2",
                    Start = DateTimeOffset.Now,
                    Duration = new TimeSpan(1, 30, 0)
                };

                Assert.AreEqual(Guid.Empty, evt2.EventID);
                DbCtx.Events.Add(evt2);
                DbCtx.SaveChanges();
                Assert.AreNotEqual(Guid.Empty, evt2.EventID);

                Assert.AreNotEqual(evt1.EventID, evt2.EventID);
            }
        }

        [TestMethod]
        public void EventCreationWithAttendees()
        {
            using (EventBotContext DbCtx = new EventBotContext())
            {
                Event evt1 = new Event()
                {
                    Subject = "Attendee Test",
                    Start = DateTimeOffset.Now,
                    Duration = new TimeSpan(1, 30, 0)
                };

                Assert.AreEqual(Guid.Empty, evt1.EventID);

                Attendee att1 = new Attendee()
                {
                    DiscordUserDiscriminator = "Testuser#0666",
                    DiscordUserID = 123456789
                };
                Assert.AreEqual(Guid.Empty, att1.AttendeeID);
                evt1.Attendees.Add(att1);

                Attendee att2 = new Attendee()
                {
                    DiscordUserDiscriminator = "Testuser#6660",
                    DiscordUserID = 987654321
                };
                Assert.AreEqual(Guid.Empty, att2.AttendeeID);
                evt1.Attendees.Add(att2);

                DbCtx.Events.Add(evt1);

                DbCtx.SaveChanges();

                Assert.AreNotEqual(Guid.Empty, evt1.EventID);
                Assert.AreNotEqual(Guid.Empty, att1.AttendeeID);
                Assert.AreNotEqual(Guid.Empty, att2.AttendeeID);
            }
        }
        [TestMethod]
        public void EventCreationWithAttendeeGroups()
        {
            using (EventBotContext DbCtx = new EventBotContext())
            {
                Event evt1 = new Event()
                {
                    Subject = "Attendee Group Test",
                    Start = DateTimeOffset.Now,
                    Duration = new TimeSpan(1, 30, 0)
                };

                Assert.AreEqual(Guid.Empty, evt1.EventID);

                AttendeeGroup grp1 = new AttendeeGroup()
                {
                    Name = "TestGrp1",
                    MaxCapacity = 5
                };
                Assert.AreEqual(Guid.Empty, grp1.GroupID);
                evt1.Groups.Add(grp1);

                Attendee att1 = new Attendee()
                {
                    DiscordUserDiscriminator = "Testuser#0666",
                    DiscordUserID = 123456789
                };
                Assert.AreEqual(Guid.Empty, att1.AttendeeID);
                grp1.Attendees.Add(att1);

                Attendee att2 = new Attendee()
                {
                    DiscordUserDiscriminator = "Testuser#6660",
                    DiscordUserID = 987654321
                };
                Assert.AreEqual(Guid.Empty, att2.AttendeeID);
                grp1.Attendees.Add(att2);

                DbCtx.Events.Add(evt1);
                DbCtx.SaveChanges();

                Assert.AreNotEqual(Guid.Empty, evt1.EventID);
                Assert.AreNotEqual(Guid.Empty, grp1.GroupID);
                Assert.AreNotEqual(Guid.Empty, att1.AttendeeID);
                Assert.AreNotEqual(Guid.Empty, att2.AttendeeID);
            }
        }
    }
}
