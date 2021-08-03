using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordEventBot.Model.Migrations
{
    public partial class notifysetting : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinutesBeforeEventNotify",
                table: "Users");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinutesBeforeEventNotify",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        #endregion Protected Methods
    }
}