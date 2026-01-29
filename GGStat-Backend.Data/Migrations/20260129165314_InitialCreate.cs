using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GGStat_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    standing = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    region = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    flag = table.Column<string>(type: "text", nullable: true),
                    points = table.Column<int>(type: "integer", nullable: false),
                    league = table.Column<string>(type: "text", nullable: false),
                    race = table.Column<string>(type: "text", nullable: false),
                    wins = table.Column<int>(type: "integer", nullable: false),
                    loses = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    match_id = table.Column<string>(type: "text", nullable: true),
                    match_link = table.Column<string>(type: "text", nullable: true),
                    result = table.Column<string>(type: "text", nullable: true),
                    points = table.Column<int>(type: "integer", nullable: false),
                    timeAgo = table.Column<string>(type: "text", nullable: true),
                    map = table.Column<string>(type: "text", nullable: true),
                    duration = table.Column<string>(type: "text", nullable: true),
                    player_race = table.Column<string>(type: "text", nullable: true),
                    opponent_race = table.Column<string>(type: "text", nullable: true),
                    opponent = table.Column<string>(type: "text", nullable: true),
                    PlayerDataId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_PlayerDatas_PlayerDataId",
                        column: x => x.PlayerDataId,
                        principalTable: "PlayerDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    time = table.Column<string>(type: "text", nullable: true),
                    player = table.Column<string>(type: "text", nullable: true),
                    message = table.Column<string>(type: "text", nullable: true),
                    MatchId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.id);
                    table.ForeignKey(
                        name: "FK_Chats_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_MatchId",
                table: "Chats",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_PlayerDataId",
                table: "Matches",
                column: "PlayerDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "PlayerDatas");
        }
    }
}
