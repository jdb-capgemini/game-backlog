using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameBacklog.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserSpecificBacklogEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BacklogEntries_CatalogGameId",
                table: "BacklogEntries");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BacklogEntries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GoogleSubjectId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 320, nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BacklogEntries_CatalogGameId",
                table: "BacklogEntries",
                column: "CatalogGameId");

            migrationBuilder.CreateIndex(
                name: "IX_BacklogEntries_UserId_CatalogGameId",
                table: "BacklogEntries",
                columns: new[] { "UserId", "CatalogGameId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_GoogleSubjectId",
                table: "ApplicationUser",
                column: "GoogleSubjectId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BacklogEntries_ApplicationUser_UserId",
                table: "BacklogEntries",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BacklogEntries_ApplicationUser_UserId",
                table: "BacklogEntries");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_BacklogEntries_CatalogGameId",
                table: "BacklogEntries");

            migrationBuilder.DropIndex(
                name: "IX_BacklogEntries_UserId_CatalogGameId",
                table: "BacklogEntries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BacklogEntries");

            migrationBuilder.CreateIndex(
                name: "IX_BacklogEntries_CatalogGameId",
                table: "BacklogEntries",
                column: "CatalogGameId",
                unique: true);
        }
    }
}
