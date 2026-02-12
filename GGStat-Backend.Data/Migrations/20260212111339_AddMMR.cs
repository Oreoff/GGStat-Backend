using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GGStat_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMMR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "accounts",
                table: "PlayerData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "current_mmr",
                table: "PlayerData",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "max_mmr",
                table: "PlayerData",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accounts",
                table: "PlayerData");

            migrationBuilder.DropColumn(
                name: "current_mmr",
                table: "PlayerData");

            migrationBuilder.DropColumn(
                name: "max_mmr",
                table: "PlayerData");
        }
    }
}
