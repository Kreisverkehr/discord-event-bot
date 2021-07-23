using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordEventBot.Model.Migrations
{
    public partial class guild_settings : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_Roles_AdminRoleRoleId",
                table: "Guilds");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_AdminRoleRoleId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "AdminRoleRoleId",
                table: "Guilds");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "AdminRoleRoleId",
                table: "Guilds",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_AdminRoleRoleId",
                table: "Guilds",
                column: "AdminRoleRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_Roles_AdminRoleRoleId",
                table: "Guilds",
                column: "AdminRoleRoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }

        #endregion Protected Methods
    }
}