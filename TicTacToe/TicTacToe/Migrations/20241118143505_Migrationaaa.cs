using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Migrations
{
    /// <inheritdoc />
    public partial class Migrationaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RunningGames");

            migrationBuilder.AddColumn<string>(
                name: "PlayerUsername",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerUsernames",
                table: "GameOverviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<int>(
                name: "WinnerId",
                table: "GameOverviews",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerUsername",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PlayerUsernames",
                table: "GameOverviews");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "GameOverviews");

            migrationBuilder.CreateTable(
                name: "RunningGames",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardSerialized = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentPlayer = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    GameOver = table.Column<bool>(type: "bit", nullable: false),
                    Players = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunningGames", x => x.GameId);
                });
        }
    }
}
