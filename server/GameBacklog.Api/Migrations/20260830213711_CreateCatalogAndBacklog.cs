using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameBacklog.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateCatalogAndBacklog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.CreateTable(
                name: "CatalogGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RawgId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ReleasedOn = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    BackgroundImageUrl = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    RawgUrl = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    MetacriticScore = table.Column<int>(type: "INTEGER", nullable: true),
                    Platforms = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Genres = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    MetadataFetchedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BacklogEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CatalogGameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    PersonalRating = table.Column<int>(type: "INTEGER", nullable: true),
                    EstimatedHours = table.Column<decimal>(type: "TEXT", precision: 8, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    StartedOn = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CompletedOn = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacklogEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BacklogEntries_CatalogGames_CatalogGameId",
                        column: x => x.CatalogGameId,
                        principalTable: "CatalogGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BacklogEntries_CatalogGameId",
                table: "BacklogEntries",
                column: "CatalogGameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BacklogEntries_Status",
                table: "BacklogEntries",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogGames_Name",
                table: "CatalogGames",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogGames_RawgId",
                table: "CatalogGames",
                column: "RawgId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BacklogEntries");

            migrationBuilder.DropTable(
                name: "CatalogGames");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CoverImageUrl = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EstimatedHours = table.Column<decimal>(type: "TEXT", precision: 8, scale: 2, nullable: true),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    Platform = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: true),
                    ReleaseYear = table.Column<int>(type: "INTEGER", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
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
    }
}
