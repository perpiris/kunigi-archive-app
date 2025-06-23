using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KunigiArchive.Application.Data.Migrations
{
    /// <inheritdoc />
    public partial class renamemastergametitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubTitle",
                table: "MasterGames",
                newName: "OrderTitle");

            migrationBuilder.RenameColumn(
                name: "MainTitle",
                table: "MasterGames",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "MasterGames",
                newName: "MainTitle");

            migrationBuilder.RenameColumn(
                name: "OrderTitle",
                table: "MasterGames",
                newName: "SubTitle");
        }
    }
}
