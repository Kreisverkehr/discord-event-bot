using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordEventBot.Model.Migrations
{
    public partial class addbotchannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "BotChannelChannelId",
                table: "Guilds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    ChannelId = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.ChannelId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_BotChannelChannelId",
                table: "Guilds",
                column: "BotChannelChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_Channels_BotChannelChannelId",
                table: "Guilds",
                column: "BotChannelChannelId",
                principalTable: "Channels",
                principalColumn: "ChannelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_Channels_BotChannelChannelId",
                table: "Guilds");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_BotChannelChannelId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "BotChannelChannelId",
                table: "Guilds");
        }
    }
}
