using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordEventBot.Model.Migrations
{
    public partial class announcements : Migration
    {
        #region Protected Methods

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
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "AnnouncementChannelChannelId",
                table: "Guilds",
                type: "INTEGER",
                nullable: true);

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

        #endregion Protected Methods
    }
}