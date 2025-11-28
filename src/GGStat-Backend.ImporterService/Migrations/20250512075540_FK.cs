using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GGStatImporterService.Migrations
{
    /// <inheritdoc />
    public partial class FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_PlayerDatas_PlayerDataId",
                table: "Matches");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerDataId",
                table: "Matches",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_PlayerDatas_PlayerDataId",
                table: "Matches",
                column: "PlayerDataId",
                principalTable: "PlayerDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_PlayerDatas_PlayerDataId",
                table: "Matches");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerDataId",
                table: "Matches",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_PlayerDatas_PlayerDataId",
                table: "Matches",
                column: "PlayerDataId",
                principalTable: "PlayerDatas",
                principalColumn: "Id");
        }
    }
}
