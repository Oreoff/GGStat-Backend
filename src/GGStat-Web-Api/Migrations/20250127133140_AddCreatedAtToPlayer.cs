using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GGStat_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDatas_CountryInfos_countryid",
                table: "PlayerDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDatas_Players_playerId",
                table: "PlayerDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDatas_Ranks_rankId",
                table: "PlayerDatas");

            migrationBuilder.RenameColumn(
                name: "rankId",
                table: "PlayerDatas",
                newName: "RankId");

            migrationBuilder.RenameColumn(
                name: "playerId",
                table: "PlayerDatas",
                newName: "PlayerId");

            migrationBuilder.RenameColumn(
                name: "countryid",
                table: "PlayerDatas",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerDatas_rankId",
                table: "PlayerDatas",
                newName: "IX_PlayerDatas_RankId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerDatas_playerId",
                table: "PlayerDatas",
                newName: "IX_PlayerDatas_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerDatas_countryid",
                table: "PlayerDatas",
                newName: "IX_PlayerDatas_CountryId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "RankId",
                table: "PlayerDatas",
                newName: "rankId");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "PlayerDatas",
                newName: "playerId");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "PlayerDatas",
                newName: "countryid");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerDatas_RankId",
                table: "PlayerDatas",
                newName: "IX_PlayerDatas_rankId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerDatas_PlayerId",
                table: "PlayerDatas",
                newName: "IX_PlayerDatas_playerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerDatas_CountryId",
                table: "PlayerDatas",
                newName: "IX_PlayerDatas_countryid");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDatas_CountryInfos_countryid",
                table: "PlayerDatas",
                column: "countryid",
                principalTable: "CountryInfos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDatas_Players_playerId",
                table: "PlayerDatas",
                column: "playerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDatas_Ranks_rankId",
                table: "PlayerDatas",
                column: "rankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
