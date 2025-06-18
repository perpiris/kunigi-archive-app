using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KunigiArchive.Application.Data.Migrations
{
    /// <inheritdoc />
    public partial class addmediafilestomainentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    MediaFileId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Path = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.MediaFileId);
                });

            migrationBuilder.CreateTable(
                name: "Puzzles",
                columns: table => new
                {
                    PuzzleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Question = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Answer = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puzzles", x => x.PuzzleId);
                    table.ForeignKey(
                        name: "FK_Puzzles_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameMediaFiles",
                columns: table => new
                {
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    MediaFileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameMediaFiles", x => new { x.GameId, x.MediaFileId });
                    table.ForeignKey(
                        name: "FK_GameMediaFiles_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameMediaFiles_MediaFiles_MediaFileId",
                        column: x => x.MediaFileId,
                        principalTable: "MediaFiles",
                        principalColumn: "MediaFileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterGameMediaFiles",
                columns: table => new
                {
                    MasterGameId = table.Column<long>(type: "bigint", nullable: false),
                    MediaFileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterGameMediaFiles", x => new { x.MasterGameId, x.MediaFileId });
                    table.ForeignKey(
                        name: "FK_MasterGameMediaFiles_MasterGames_MasterGameId",
                        column: x => x.MasterGameId,
                        principalTable: "MasterGames",
                        principalColumn: "MasterGameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterGameMediaFiles_MediaFiles_MediaFileId",
                        column: x => x.MediaFileId,
                        principalTable: "MediaFiles",
                        principalColumn: "MediaFileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMediaFiles",
                columns: table => new
                {
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    MediaFileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMediaFiles", x => new { x.TeamId, x.MediaFileId });
                    table.ForeignKey(
                        name: "FK_TeamMediaFiles_MediaFiles_MediaFileId",
                        column: x => x.MediaFileId,
                        principalTable: "MediaFiles",
                        principalColumn: "MediaFileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMediaFiles_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PuzzleMediaFiles",
                columns: table => new
                {
                    PuzzleId = table.Column<long>(type: "bigint", nullable: false),
                    MediaFileId = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuzzleMediaFiles", x => new { x.PuzzleId, x.MediaFileId, x.FileType });
                    table.ForeignKey(
                        name: "FK_PuzzleMediaFiles_MediaFiles_MediaFileId",
                        column: x => x.MediaFileId,
                        principalTable: "MediaFiles",
                        principalColumn: "MediaFileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PuzzleMediaFiles_Puzzles_PuzzleId",
                        column: x => x.PuzzleId,
                        principalTable: "Puzzles",
                        principalColumn: "PuzzleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameMediaFiles_MediaFileId",
                table: "GameMediaFiles",
                column: "MediaFileId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterGameMediaFiles_MediaFileId",
                table: "MasterGameMediaFiles",
                column: "MediaFileId");

            migrationBuilder.CreateIndex(
                name: "IX_PuzzleMediaFiles_MediaFileId",
                table: "PuzzleMediaFiles",
                column: "MediaFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Puzzles_GameId",
                table: "Puzzles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMediaFiles_MediaFileId",
                table: "TeamMediaFiles",
                column: "MediaFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameMediaFiles");

            migrationBuilder.DropTable(
                name: "MasterGameMediaFiles");

            migrationBuilder.DropTable(
                name: "PuzzleMediaFiles");

            migrationBuilder.DropTable(
                name: "TeamMediaFiles");

            migrationBuilder.DropTable(
                name: "Puzzles");

            migrationBuilder.DropTable(
                name: "MediaFiles");
        }
    }
}
