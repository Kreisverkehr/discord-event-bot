using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordEventBot.Model.Migrations
{
    public partial class usedatetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Events");
        }
    }
}
