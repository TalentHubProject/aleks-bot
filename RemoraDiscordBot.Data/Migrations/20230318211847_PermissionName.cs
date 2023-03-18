using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoraDiscordBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class PermissionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionDtoPermissionUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Permissions",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "PermissionUserId",
                table: "Permissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                columns: new[] { "Id", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionUserId",
                table: "Permissions",
                column: "PermissionUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_PermissionUsers_PermissionUserId",
                table: "Permissions",
                column: "PermissionUserId",
                principalTable: "PermissionUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_PermissionUsers_PermissionUserId",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_PermissionUserId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "PermissionUserId",
                table: "Permissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                columns: new[] { "Id", "CategoryId" });

            migrationBuilder.CreateTable(
                name: "PermissionDtoPermissionUser",
                columns: table => new
                {
                    PermissionUsersId = table.Column<int>(type: "int", nullable: false),
                    PermissionsId = table.Column<int>(type: "int", nullable: false),
                    PermissionsCategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionDtoPermissionUser", x => new { x.PermissionUsersId, x.PermissionsId, x.PermissionsCategoryId });
                    table.ForeignKey(
                        name: "FK_PermissionDtoPermissionUser_PermissionUsers_PermissionUsersId",
                        column: x => x.PermissionUsersId,
                        principalTable: "PermissionUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionDtoPermissionUser_Permissions_PermissionsId_Permis~",
                        columns: x => new { x.PermissionsId, x.PermissionsCategoryId },
                        principalTable: "Permissions",
                        principalColumns: new[] { "Id", "CategoryId" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionDtoPermissionUser_PermissionsId_PermissionsCategor~",
                table: "PermissionDtoPermissionUser",
                columns: new[] { "PermissionsId", "PermissionsCategoryId" });
        }
    }
}
