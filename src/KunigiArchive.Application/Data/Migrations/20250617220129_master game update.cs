using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KunigiArchive.Application.Data.Migrations
{
    /// <inheritdoc />
    public partial class mastergameupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameType_GameTypeId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameType",
                table: "GameType");

            migrationBuilder.RenameTable(
                name: "GameType",
                newName: "GameTypes");

            migrationBuilder.RenameIndex(
                name: "IX_GameType_Slug",
                table: "GameTypes",
                newName: "IX_GameTypes_Slug");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MasterGames",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "MasterGames",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameTypes",
                table: "GameTypes",
                column: "GameTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameTypes_GameTypeId",
                table: "Games",
                column: "GameTypeId",
                principalTable: "GameTypes",
                principalColumn: "GameTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameTypes_GameTypeId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameTypes",
                table: "GameTypes");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "MasterGames");

            migrationBuilder.RenameTable(
                name: "GameTypes",
                newName: "GameType");

            migrationBuilder.RenameIndex(
                name: "IX_GameTypes_Slug",
                table: "GameType",
                newName: "IX_GameType_Slug");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MasterGames",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameType",
                table: "GameType",
                column: "GameTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameType_GameTypeId",
                table: "Games",
                column: "GameTypeId",
                principalTable: "GameType",
                principalColumn: "GameTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
