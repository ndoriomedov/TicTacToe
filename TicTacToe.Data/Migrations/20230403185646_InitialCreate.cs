using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.Data.Migrations
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
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
            WinnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Games", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Players",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Points = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Players", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "DbGameDbPlayer",
          columns: table => new
          {
            GamesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            PlayersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_DbGameDbPlayer", x => new { x.GamesId, x.PlayersId });
            table.ForeignKey(
                      name: "FK_DbGameDbPlayer_Games_GamesId",
                      column: x => x.GamesId,
                      principalTable: "Games",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_DbGameDbPlayer_Players_PlayersId",
                      column: x => x.PlayersId,
                      principalTable: "Players",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Moves",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            Row = table.Column<byte>(type: "tinyint", nullable: false),
            Column = table.Column<byte>(type: "tinyint", nullable: false),
            CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Moves", x => x.Id);
            table.ForeignKey(
                      name: "FK_Moves_Games_GameId",
                      column: x => x.GameId,
                      principalTable: "Games",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_Moves_Players_PlayerId",
                      column: x => x.PlayerId,
                      principalTable: "Players",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_DbGameDbPlayer_PlayersId",
          table: "DbGameDbPlayer",
          column: "PlayersId");

      migrationBuilder.CreateIndex(
          name: "IX_Moves_GameId",
          table: "Moves",
          column: "GameId");

      migrationBuilder.CreateIndex(
          name: "IX_Moves_PlayerId",
          table: "Moves",
          column: "PlayerId");

      migrationBuilder.CreateIndex(
          name: "IX_Players_Login",
          table: "Players",
          column: "Login",
          unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "DbGameDbPlayer");

      migrationBuilder.DropTable(
          name: "Moves");

      migrationBuilder.DropTable(
          name: "Games");

      migrationBuilder.DropTable(
          name: "Players");
    }
  }
}
