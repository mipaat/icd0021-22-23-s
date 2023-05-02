using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class CaptionsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCaptions",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "HasCaptions",
                table: "VideoHistories");

            migrationBuilder.AddColumn<string>(
                name: "AutomaticCaptions",
                table: "Videos",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AutomaticCaptions",
                table: "VideoHistories",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutomaticCaptions",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "AutomaticCaptions",
                table: "VideoHistories");

            migrationBuilder.AddColumn<bool>(
                name: "HasCaptions",
                table: "Videos",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasCaptions",
                table: "VideoHistories",
                type: "boolean",
                nullable: true);
        }
    }
}
