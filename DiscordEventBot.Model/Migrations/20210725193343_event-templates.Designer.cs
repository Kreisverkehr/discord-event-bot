﻿// <auto-generated />
using System;
using DiscordEventBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DiscordEventBot.Model.Migrations
{
    [DbContext(typeof(EventBotContext))]
    [Migration("20210725193343_event-templates")]
    partial class eventtemplates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("AttendeeGroupUser", b =>
                {
                    b.Property<ulong>("AttendeesUserId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("GroupsGroupID")
                        .HasColumnType("INTEGER");

                    b.HasKey("AttendeesUserId", "GroupsGroupID");

                    b.HasIndex("GroupsGroupID");

                    b.ToTable("AttendeeGroupUser");
                });

            modelBuilder.Entity("DiscordEventBot.Model.AttendeeGroup", b =>
                {
                    b.Property<ulong>("GroupID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("EventID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MaxCapacity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("GroupID");

                    b.HasIndex("EventID");

                    b.ToTable("AttendeeGroup");
                });

            modelBuilder.Entity("DiscordEventBot.Model.AttendeeGroupTemplate", b =>
                {
                    b.Property<ulong>("GroupTemplateID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("EventTemplateID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MaxCapacity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("GroupTemplateID");

                    b.HasIndex("EventTemplateID");

                    b.ToTable("AttendeeGroupTemplate");
                });

            modelBuilder.Entity("DiscordEventBot.Model.Channel", b =>
                {
                    b.Property<ulong>("ChannelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("ChannelId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("DiscordEventBot.Model.Event", b =>
                {
                    b.Property<ulong>("EventID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<ulong?>("CreatorUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<ulong?>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Start")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .HasColumnType("TEXT");

                    b.HasKey("EventID");

                    b.HasIndex("CreatorUserId");

                    b.HasIndex("GuildId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("DiscordEventBot.Model.EventTemplate", b =>
                {
                    b.Property<ulong>("EventTemplateID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<ulong?>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .HasColumnType("TEXT");

                    b.HasKey("EventTemplateID");

                    b.HasIndex("GuildId");

                    b.ToTable("EventTemplates");
                });

            modelBuilder.Entity("DiscordEventBot.Model.Guild", b =>
                {
                    b.Property<ulong>("GuildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("AdminRoleRoleId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("BotChannelChannelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GuildId");

                    b.HasIndex("AdminRoleRoleId");

                    b.HasIndex("BotChannelChannelId");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("DiscordEventBot.Model.Role", b =>
                {
                    b.Property<ulong>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DiscordEventBot.Model.User", b =>
                {
                    b.Property<ulong>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EventUser", b =>
                {
                    b.Property<ulong>("AttendeesUserId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("EventsEventID")
                        .HasColumnType("INTEGER");

                    b.HasKey("AttendeesUserId", "EventsEventID");

                    b.HasIndex("EventsEventID");

                    b.ToTable("EventUser");
                });

            modelBuilder.Entity("AttendeeGroupUser", b =>
                {
                    b.HasOne("DiscordEventBot.Model.User", null)
                        .WithMany()
                        .HasForeignKey("AttendeesUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiscordEventBot.Model.AttendeeGroup", null)
                        .WithMany()
                        .HasForeignKey("GroupsGroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DiscordEventBot.Model.AttendeeGroup", b =>
                {
                    b.HasOne("DiscordEventBot.Model.Event", null)
                        .WithMany("Groups")
                        .HasForeignKey("EventID");
                });

            modelBuilder.Entity("DiscordEventBot.Model.AttendeeGroupTemplate", b =>
                {
                    b.HasOne("DiscordEventBot.Model.EventTemplate", null)
                        .WithMany("Groups")
                        .HasForeignKey("EventTemplateID");
                });

            modelBuilder.Entity("DiscordEventBot.Model.Event", b =>
                {
                    b.HasOne("DiscordEventBot.Model.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorUserId");

                    b.HasOne("DiscordEventBot.Model.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId");

                    b.Navigation("Creator");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("DiscordEventBot.Model.EventTemplate", b =>
                {
                    b.HasOne("DiscordEventBot.Model.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("DiscordEventBot.Model.Guild", b =>
                {
                    b.HasOne("DiscordEventBot.Model.Role", "AdminRole")
                        .WithMany()
                        .HasForeignKey("AdminRoleRoleId");

                    b.HasOne("DiscordEventBot.Model.Channel", "BotChannel")
                        .WithMany()
                        .HasForeignKey("BotChannelChannelId");

                    b.Navigation("AdminRole");

                    b.Navigation("BotChannel");
                });

            modelBuilder.Entity("EventUser", b =>
                {
                    b.HasOne("DiscordEventBot.Model.User", null)
                        .WithMany()
                        .HasForeignKey("AttendeesUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiscordEventBot.Model.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsEventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DiscordEventBot.Model.Event", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("DiscordEventBot.Model.EventTemplate", b =>
                {
                    b.Navigation("Groups");
                });
#pragma warning restore 612, 618
        }
    }
}
