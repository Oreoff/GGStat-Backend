using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GGStatImporterService.Migrations
{
    /// <inheritdoc />
    public partial class RemoveJoins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDatas_CountryInfos_CountryId",
                table: "PlayerDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDatas_Players_PlayerId",
                table: "PlayerDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDatas_Ranks_RankId",
                table: "PlayerDatas");

            migrationBuilder.DropTable(
                name: "CountryInfos");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_PlayerDatas_CountryId",
                table: "PlayerDatas");

            migrationBuilder.DropIndex(
                name: "IX_PlayerDatas_PlayerId",
                table: "PlayerDatas");

            migrationBuilder.DropIndex(
                name: "IX_PlayerDatas_RankId",
                table: "PlayerDatas");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "PlayerDatas");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "PlayerDatas");

            migrationBuilder.RenameColumn(
                name: "RankId",
                table: "PlayerDatas",
                newName: "points");

            migrationBuilder.AddColumn<string>(
                name: "alias",
                table: "PlayerDatas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "PlayerDatas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "PlayerDatas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "flag",
                table: "PlayerDatas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "league",
                table: "PlayerDatas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "PlayerDatas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "region",
                table: "PlayerDatas",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "alias",
                table: "PlayerDatas");

            migrationBuilder.DropColumn(
                name: "avatar",
                table: "PlayerDatas");

            migrationBuilder.DropColumn(
                name: "code",
                table: "PlayerDatas");

            migrationBuilder.DropColumn(
                name: "flag",
                table: "PlayerDatas");

            migrationBuilder.DropColumn(
                name: "league",
                table: "PlayerDatas");

            migrationBuilder.DropColumn(
                name: "name",
                table: "PlayerDatas");

            migrationBuilder.DropColumn(
                name: "region",
                table: "PlayerDatas");

            migrationBuilder.RenameColumn(
                name: "points",
                table: "PlayerDatas",
                newName: "RankId");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "PlayerDatas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "PlayerDatas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CountryInfos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: true),
                    flag = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    alias = table.Column<string>(type: "text", nullable: true),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    region = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    league = table.Column<string>(type: "text", nullable: false),
                    points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDatas_CountryId",
                table: "PlayerDatas",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDatas_PlayerId",
                table: "PlayerDatas",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDatas_RankId",
                table: "PlayerDatas",
                column: "RankId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDatas_CountryInfos_CountryId",
                table: "PlayerDatas",
                column: "CountryId",
                principalTable: "CountryInfos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDatas_Players_PlayerId",
                table: "PlayerDatas",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDatas_Ranks_RankId",
                table: "PlayerDatas",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
