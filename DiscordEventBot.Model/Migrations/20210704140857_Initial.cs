using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DiscordEventBot.Model.Migrations
{
    public partial class Initial : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendee");

            migrationBuilder.DropTable(
                name: "AttendeeGroup");

            migrationBuilder.DropTable(
                name: "Events");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", nullable: true),
                    Start = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventID);
                });

            migrationBuilder.CreateTable(
                name: "AttendeeGroup",
                columns: table => new
                {
                    GroupID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    MaxCapacity = table.Column<int>(type: "INTEGER", nullable: true),
                    EventID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendeeGroup", x => x.GroupID);
                    table.ForeignKey(
                        name: "FK_AttendeeGroup_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attendee",
                columns: table => new
                {
                    AttendeeID = table.Column<Guid>(type: "TEXT", nullable: false),
                    DiscordUserDiscriminator = table.Column<string>(type: "TEXT", nullable: true),
                    DiscordUserID = table.Column<ulong>(type: "INTEGER", nullable: false),
                    AttendeeGroupGroupID = table.Column<Guid>(type: "TEXT", nullable: true),
                    EventID = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendee", x => x.AttendeeID);
                    table.ForeignKey(
                        name: "FK_Attendee_AttendeeGroup_AttendeeGroupGroupID",
                        column: x => x.AttendeeGroupGroupID,
                        principalTable: "AttendeeGroup",
                        principalColumn: "GroupID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendee_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendee_AttendeeGroupGroupID",
                table: "Attendee",
                column: "AttendeeGroupGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Attendee_EventID",
                table: "Attendee",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_AttendeeGroup_EventID",
                table: "AttendeeGroup",
                column: "EventID");
        }

        #endregion Protected Methods
    }
}