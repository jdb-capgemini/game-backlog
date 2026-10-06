using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameBacklog.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Platform = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    ReleaseYear = table.Column<int>(type: "INTEGER", nullable: true),
                    Rating = table.Column<int>(type: "INTEGER", nullable: true),
                    EstimatedHours = table.Column<decimal>(type: "TEXT", precision: 8, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_Platform",
                table: "Games",
                column: "Platform");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Status",
                table: "Games",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Title",
                table: "Games",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
