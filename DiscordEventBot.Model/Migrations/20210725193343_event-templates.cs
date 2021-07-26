using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DiscordEventBot.Model.Migrations
{
    public partial class eventtemplates : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendeeGroupTemplate");

            migrationBuilder.DropTable(
                name: "EventTemplates");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventTemplates",
                columns: table => new
                {
                    EventTemplateID = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    GuildId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Subject = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTemplates", x => x.EventTemplateID);
                    table.ForeignKey(
                        name: "FK_EventTemplates_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttendeeGroupTemplate",
                columns: table => new
                {
                    GroupTemplateID = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaxCapacity = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    EventTemplateID = table.Column<ulong>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendeeGroupTemplate", x => x.GroupTemplateID);
                    table.ForeignKey(
                        name: "FK_AttendeeGroupTemplate_EventTemplates_EventTemplateID",
                        column: x => x.EventTemplateID,
                        principalTable: "EventTemplates",
                        principalColumn: "EventTemplateID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendeeGroupTemplate_EventTemplateID",
                table: "AttendeeGroupTemplate",
                column: "EventTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_EventTemplates_GuildId",
                table: "EventTemplates",
                column: "GuildId");
        }

        #endregion Protected Methods
    }
}