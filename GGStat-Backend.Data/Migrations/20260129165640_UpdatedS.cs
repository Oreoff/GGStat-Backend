using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GGStat_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_PlayerDatas_PlayerDataId",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerDatas",
                table: "PlayerDatas");

            migrationBuilder.RenameTable(
                name: "PlayerDatas",
                newName: "PlayerData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerData",
                table: "PlayerData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_PlayerData_PlayerDataId",
                table: "Matches",
                column: "PlayerDataId",
                principalTable: "PlayerData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_PlayerData_PlayerDataId",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerData",
                table: "PlayerData");

            migrationBuilder.RenameTable(
                name: "PlayerData",
                newName: "PlayerDatas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerDatas",
                table: "PlayerDatas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_PlayerDatas_PlayerDataId",
                table: "Matches",
                column: "PlayerDataId",
                principalTable: "PlayerDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
