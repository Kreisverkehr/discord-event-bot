using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordEventBot.Model.Migrations
{
    public partial class announcements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "AnnouncementChannelChannelId",
                table: "Guilds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotificationTime",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 15);

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_AnnouncementChannelChannelId",
                table: "Guilds",
                column: "AnnouncementChannelChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_Channels_AnnouncementChannelChannelId",
                table: "Guilds",
                column: "AnnouncementChannelChannelId",
                principalTable: "Channels",
                principalColumn: "ChannelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_Channels_AnnouncementChannelChannelId",
                table: "Guilds");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_AnnouncementChannelChannelId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "AnnouncementChannelChannelId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "NotificationTime",
                table: "Guilds");
        }
    }
}
