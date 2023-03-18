using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoraDiscordBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class PermissionInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => new { x.Id, x.CategoryId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermissionUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GuildId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionUsers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionDtoPermissionUser");

            migrationBuilder.DropTable(
                name: "PermissionUsers");

            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
