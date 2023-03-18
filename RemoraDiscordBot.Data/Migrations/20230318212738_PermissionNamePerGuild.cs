using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoraDiscordBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class PermissionNamePerGuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.AddColumn<long>(
                name: "GuildId",
                table: "Permissions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                columns: new[] { "Id", "Name", "GuildId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "Permissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                columns: new[] { "Id", "Name" });
        }
    }
}
